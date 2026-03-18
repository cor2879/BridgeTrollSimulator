using UnityEngine;
using System.Collections;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Scenarios;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Rewards;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Phases;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Rewards.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel
{
    public class SocialDuelSystem : MonoBehaviour, IEventSource
    {
        #region Singleton

        private static SocialDuelSystem _instance;

        public static SocialDuelSystem Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindFirstObjectByType<SocialDuelSystem>();

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        #endregion

        #region Serialized Fields

        [SerializeField] private SocialDuelUI duelUI;
        [SerializeField] private SocialDuelIntroUI introUI;
        [SerializeField] private SocialDuelPreSummaryUI preSummaryUI;
        [SerializeField] private SocialDuelResolutionUI resolutionUI;

        #endregion

        #region Private State

        private ISocialDuelPhase currentPhase;
        private SocialDuelState state = SocialDuelState.Inactive;
        private SocialDuelContext context;

        private SpeechBubbleUI activeBubble;

        [SerializeField, ReadOnly]
        private EntityController currentAttacker;
        [SerializeField, ReadOnly]
        private EntityController currentDefender;
        private SocialExchangeOutcome pendingOutcome;

        private SocialExchangePhase exchangePhase = SocialExchangePhase.None;

        private enum SocialExchangePhase
        {
            None,
            AttackerLine,
            DefenderResponse
        }

        #endregion

        #region IEventSource

        public string SourceName => nameof(SocialDuelSystem);
        public GameSystemType SystemType => GameSystemType.System;

        #endregion

        #region Public Properties

        public SocialDuelUI UI => duelUI;
        public SocialDuelIntroUI IntroUI => introUI;
        public SocialDuelPreSummaryUI PreSummaryUI => preSummaryUI;
        public SocialDuelResolutionUI ResolutionUI => resolutionUI;

        #endregion

        #region Initialization

        private void OnEnable()
        {
            GameEventBus.Subscribe<SocialDuelStartedEvent>(OnSocialDuelStarted);
            GameEventBus.Subscribe<SocialDuelConfirmedEvent>(OnSocialDuelConfirmed);
            GameEventBus.Subscribe<SocialDuelPreSummaryConfirmedEvent>(OnSocialDuelPreSummaryConfirmed);
            GameEventBus.Subscribe<SocialDuelEndedEvent>(OnSocialDuelEnded);
            GameEventBus.Subscribe<SocialDuelResolutionCompletedEvent>(OnSocialDuelResolutionCompleted);
            GameEventBus.Subscribe<ConcedeSocialDuelEvent>(OnConcededSocialDuel);
            GameEventBus.Subscribe<SocialDuelSurrenderAcceptedEvent>(OnSocialDuelSurrenderAccepted);
            GameEventBus.Subscribe<SocialDuelSurrenderDeniedEvent>(OnSocialDuelSurrenderDenied);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<SocialDuelStartedEvent>(OnSocialDuelStarted);
            GameEventBus.Unsubscribe<SocialDuelConfirmedEvent>(OnSocialDuelConfirmed);
            GameEventBus.Unsubscribe<SocialDuelPreSummaryConfirmedEvent>(OnSocialDuelPreSummaryConfirmed);
            GameEventBus.Unsubscribe<SocialDuelEndedEvent>(OnSocialDuelEnded);
            GameEventBus.Unsubscribe<SocialDuelResolutionCompletedEvent>(OnSocialDuelResolutionCompleted);
            GameEventBus.Unsubscribe<ConcedeSocialDuelEvent>(OnConcededSocialDuel);
            GameEventBus.Unsubscribe<SocialDuelSurrenderAcceptedEvent>(OnSocialDuelSurrenderAccepted);
            GameEventBus.Unsubscribe<SocialDuelSurrenderDeniedEvent>(OnSocialDuelSurrenderDenied);
        }

        #endregion

        #region Event Handling

        private void OnSocialDuelStarted(SocialDuelStartedEvent evt)
        {
            var initiator = evt.Initiator as EntityController;
            var target = evt.Target as EntityController;

            var player = initiator.IsPlayerControlled ? initiator : target;
            var npc = initiator.IsPlayerControlled ? target : initiator;
            
            context = new SocialDuelContext(player, npc);

            SetPhase(SocialDuelPhaseBase.GetInstance<SocialDuelIntroPhase>());
        }

        private void OnSocialDuelConfirmed(SocialDuelConfirmedEvent evt)
        {
            if (context == null)
            {
                return;
            }

            SetPhase(SocialDuelPhaseBase.GetInstance<SocialDuelPreSummaryPhase>());
        }

        private void OnSocialDuelPreSummaryConfirmed(SocialDuelPreSummaryConfirmedEvent evt)
        {
            var initiator = evt.Initiator as EntityController;
            var target = evt.Target as EntityController;

            var player = initiator.IsPlayerControlled ? initiator : target;
            var npc = initiator.IsPlayerControlled ? target : initiator;

            StartDuel(player, npc);
        }

        private void OnSocialDuelEnded(SocialDuelEndedEvent evt)
        {
            SetPhase(SocialDuelPhaseBase.GetInstance<SocialDuelPostSummaryPhase>());
        }

        private void OnSocialDuelResolutionCompleted(SocialDuelResolutionCompletedEvent evt)
        {
            var data = evt.Data;

            FinalizeDuel(data);
        }

        private void OnConcededSocialDuel(ConcedeSocialDuelEvent evt)
        {
            var initiator = evt.Initiator as EntityController;
            var target = evt.Target as EntityController;

            if (initiator == null || target == null)
                return;

            // Only resolve reactions if player initiated
            if (!initiator.IsPlayerControlled)
                return;

            ResolveSurrenderReaction(initiator, target, evt); 
        }

        private void OnSocialDuelSurrenderAccepted(SocialDuelSurrenderAcceptedEvent evt)
        {
            var winner = evt.Initiator as EntityController;
            var loser = evt.Target as EntityController;

            var outcome = winner.IsPlayerControlled ?
                SocialDuelOutcome.PlayerVictory :
                SocialDuelOutcome.NpcVictory;

            EndDuel(outcome);
        }

        private void OnSocialDuelSurrenderDenied(SocialDuelSurrenderDeniedEvent evt)
        {
            BeginPlayerTurn();
        }

        #endregion

        #region Public API

        public void StartDuel(EntityController player, EntityController npc)
        {
            duelUI.Initialize(context);
            duelUI.Show();

            currentAttacker = player;
            currentDefender = npc;

            SetPhase(SocialDuelPhaseBase
                .GetInstance<SocialDuelExchangePhase>());
        }

        public void HandleAbility(SocialAbility ability)
        {
            currentPhase?.OnAbilityChosen(ability);
        }

        #endregion

        #region Exchange Flow

        public void ShowAttackerLine(
            EntityController attacker,
            SocialAbility ability, 
            SocialExchangeOutcome outcome)
        {
            pendingOutcome = outcome;
            exchangePhase = SocialExchangePhase.AttackerLine;

            string line = ability.GetPlayerLine(outcome);

            attacker.SpeechBubble.Show(
                line);

            SetActiveSpeaker(attacker);
        }

        private void ShowDefenderResponse()
        {
            exchangePhase = SocialExchangePhase.DefenderResponse;

            int resolve = currentDefender == context.Player
                ? context.Player.Resolve
                : context.Npc.Resolve;

            int maxResolve = currentDefender == context.Player
                ? context.Player.MaxResolve
                : context.Npc.MaxResolve;

            string response = currentDefender.GetSocialResponse(
                pendingOutcome,
                resolve,
                maxResolve);

            currentDefender.SpeechBubble.Show(
                response);

            SetActiveSpeaker(currentDefender);
        }

        private void AdvanceExchange()
        {
            switch (exchangePhase)
            {
                case SocialExchangePhase.AttackerLine:
                    ShowDefenderResponse();
                    break;

                case SocialExchangePhase.DefenderResponse:
                    CompleteTurn();
                    break;
            }
        }

        private void CompleteTurn()
        {
            StartCoroutine(CompleteTurnRoutine());
        }

        private IEnumerator CompleteTurnRoutine()
        {
            exchangePhase = SocialExchangePhase.None;

            yield return new WaitUntil(() => duelUI.Busy == 0);

            if (CheckForEnd())
            {
                yield break;
            }

            if (state == SocialDuelState.PlayerTurn)
            {
                BeginNpcTurn();
            }
            else
            {
                BeginPlayerTurn();
            }
        }

        #endregion

        #region Turn Management

        private void BeginPlayerTurn()
        {
            state = SocialDuelState.PlayerTurn;
            duelUI.EnableInput(true);
        }

        private void BeginNpcTurn()
        {
            state = SocialDuelState.NpcTurn;

            var ability = ChooseNpcAbility(context.Npc);
            if (ability == null)
            {
                BeginPlayerTurn();
                return;
            }

            currentAttacker = context.Npc;
            currentDefender = context.Player;

            if (!ability.TryExecuteSpecial(currentAttacker, currentDefender))
            {
                pendingOutcome = ability.ResolveExchange(currentAttacker, currentDefender);

                ApplyOutcome(currentAttacker, currentDefender, pendingOutcome);

                if (CheckForEnd())
                    return;

                ShowAttackerLine(currentAttacker, ability, pendingOutcome);
                context.LastSkillUsed = ability.GoverningSkill;
            }
        }

        #endregion

        #region Resolve & Outcome

        private void ResolveSurrenderReaction(
            EntityController player,
            EntityController npc,
            ConcedeSocialDuelEvent evt)
        {
            var chosen = ReactionResolver.Resolve(
                SurrenderScenario.Instance,
                npc, 
                player,
                evt);

            chosen.Execute(npc, player, evt);
        }

        public void ApplyOutcome(
            EntityController attacker,
            EntityController defender,
            SocialExchangeOutcome outcome)
        {
            // -------------------------
            // 1️⃣ Resolve Damage
            // -------------------------

            var target = outcome.DamageSelf ? attacker : defender;

            target.TakeResolveDamage(outcome.ResolveAmount, outcome.IsCritical);
            duelUI.AnimateResolveChange(target, target.Resolve);

            // -------------------------
            // 2️⃣ Momentum Adjustment
            // -------------------------

            int momentumShift = 1;

            if (outcome.IsCritical)
                momentumShift = 2;

            if (outcome.Result == SocialExchangeResult.StrongSuccess)
            {
                attacker.ModifyMomentum(+momentumShift);
                defender.ModifyMomentum(-momentumShift);
            }
            else if (outcome.Result == SocialExchangeResult.StrongFailure)
            {
                attacker.ModifyMomentum(-momentumShift);
                defender.ModifyMomentum(+momentumShift);
            }
            // Weak results do not affect momentum
        }

        public bool CheckForEnd()
        {
            if (context.Npc.Resolve <= 0)
            {
                StartCoroutine(EndDuelRoutine(SocialDuelOutcome.PlayerVictory));
                return true;
            }

            if (context.Player.Resolve <= 0)
            {
                StartCoroutine(EndDuelRoutine(SocialDuelOutcome.NpcVictory));
                return true;
            }

            return false;
        }

        private IEnumerator EndDuelRoutine(SocialDuelOutcome outcome)
        {
            yield return new WaitUntil(() => duelUI.Busy == 0);

            EndDuel(outcome);
        }

        private RewardBundle CalculateRewards(SocialDuelOutcome outcome)
        {
            if (outcome != SocialDuelOutcome.PlayerVictory)
            {
                return RewardBundle.Empty;
            }

            var experience = (context.Npc.ExperienceReward / 2) * context.Npc.Level;

            return new RewardBundle(
                experience: experience,
                gold: Random.Range(1, 5),
                fameDelta: 1,
                respectDelta: 1,
                reputationDelta: 0,
                resolve: context.Player.MaxResolve / 2);
        }

        private void StartCombat(SocialDuelResolutionData data)
        {
            GameEventBus.Publish(
                new CombatStartedEvent(
                    data.Player,
                    data.Npc,
                    Time.frameCount));
        }

        #endregion

        #region Bubble Control

        private void SetActiveSpeaker(EntityController speaker)
        {
            ClearActiveSpeaker();

            activeBubble = speaker.SpeechBubble;

            if (activeBubble != null)
                activeBubble.OnAdvanceRequested += HandleAdvanceRequested;
        }

        private void ClearActiveSpeaker()
        {
            if (activeBubble != null)
            {
                activeBubble.OnAdvanceRequested -= HandleAdvanceRequested;
                activeBubble = null;
            }
        }

        private void HandleAdvanceRequested()
        {
            ClearActiveSpeaker();
            AdvanceExchange();
        }
        

        #endregion

        #region NPC Logic

        private SocialAbility ChooseNpcAbility(EntityController npc)
        {
            var abilities = npc.SocialAbilities;

            if (abilities == null || abilities.Length == 0)
                return null;

            float resolvePercent =
                (float)context.Npc.Resolve / context.Npc.MaxResolve;

            SocialAbility bestChoice = abilities[0];
            int bestScore = int.MinValue;

            foreach (var ability in abilities)
            {
                int score = 0;

                // 1️⃣ Base stat affinity (NPC prefers what they are good at)
                score += npc.BaseStats.GetModifier(ability.OffensiveStat) * 2;

                score += npc.BaseSkills.Get(ability.GoverningSkill);

                // 2️⃣ Counter last used skill
                if (context.LastSkillUsed.HasValue &&
                    ability.GoverningSkill == context.LastSkillUsed.Value)
                {
                    score += 2;
                }

                // 3️⃣ Resolve influence (THIS is where resolve matters)

                if (resolvePercent < 0.3f)
                {
                    // Low resolve → desperate or defensive

                    // Example logic:
                    // Prefer strongest ability (highest stat)

                    score += 3;
                }
                else if (resolvePercent > 0.7f)
                {
                    // High resolve → confident
                    // More aggressive
                    score += 1;
                }

                // 4️⃣ Momentum influence (optional)

                if (context.Npc.Momentum > context.Player.Momentum)
                {
                    score += 1;
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    bestChoice = ability;
                }
            }

            return bestChoice;
        }

        #endregion

        #region End Duel

        private void EndDuel(SocialDuelOutcome outcome)
        {
            ClearActiveSpeaker();

            state = SocialDuelState.Completed;
            duelUI.Hide();

            var reward = CalculateRewards(outcome);
            context.Resolution = new SocialDuelResolutionData(
                context.Player,
                context.Npc,
                outcome,
                reward);

            Debug.Log($"{nameof(EndDuel)}::reward:{reward}");
            GameEventBus.Publish(
                new SocialDuelEndedEvent(this, outcome, Time.frameCount));
        }

        private void FinalizeDuel(SocialDuelResolutionData data)
        {
            SetPhase(null);

            var player = data.Player as TrollController;
            var npc = data.Npc as NpcController;

            switch (data.Outcome)
            {
                case SocialDuelOutcome.PlayerVictory:
                    player.OnSocialDuelVictory(data);
                    npc.OnSocialDuelLoss(data);
                    break;
                case SocialDuelOutcome.NpcVictory:
                    player.OnSocialDuelLoss(data);
                    npc.OnSocialDuelVictory(data);
                    break;
                case SocialDuelOutcome.Escalation:
                    StartCombat(data);
                    break;
            }

            state = SocialDuelState.Inactive;
            GameDatabase.Instance.Player.ResetControlMode(overrideDeath: false);

            GameEventBus.Publish(
                new RewardEvent(
                    this,
                    data.Player,
                    data.Reward,
                    Time.frameCount));

            context = null;
        }

        #endregion

        #region Phase Management

        public void SetPhase(ISocialDuelPhase phase)
        {
            currentPhase?.Exit();
            currentPhase = phase;
            currentPhase?.Enter(this, context);
        }

        #endregion
    }
}