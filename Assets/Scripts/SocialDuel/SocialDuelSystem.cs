using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events;
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

        #endregion

        #region Private State

        private SocialDuelState state = SocialDuelState.Inactive;
        private SocialDuelContext context;

        private SpeechBubbleUI activeBubble;

        private EntityController currentAttacker;
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

        #region Initialization

        private void OnEnable()
        {
            GameEventBus.Subscribe<SocialDuelStartedEvent>(OnSocialDuelStarted);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<SocialDuelStartedEvent>(OnSocialDuelStarted);
        }

        #endregion

        #region Event Handling

        private void OnSocialDuelStarted(SocialDuelStartedEvent evt)
        {
            var initiator = evt.Initiator as EntityController;
            var target = evt.Target as EntityController;

            var player = initiator.IsPlayerControlled ? initiator : target;
            var npc = initiator.IsPlayerControlled ? target : initiator;

            StartDuel(player, npc);
        }

        #endregion

        #region Public API

        public void StartDuel(EntityController player, EntityController npc)
        {
            context = new SocialDuelContext(player, npc);

            duelUI.Initialize(context);
            duelUI.Show();

            state = SocialDuelState.PlayerTurn;
            duelUI.EnableInput(true);
        }

        public void PlayerUseAbility(SocialAbility ability)
        {
            if (state != SocialDuelState.PlayerTurn || ability == null)
                return;

            duelUI.EnableInput(false);

            currentAttacker = context.Player;
            currentDefender = context.Npc;

            pendingOutcome = ability.ResolveExchange(currentAttacker, currentDefender);

            ApplyOutcome(currentAttacker, currentDefender, pendingOutcome);

            if (CheckForEnd())
                return;

            ShowAttackerLine(ability);
            context.LastSkillUsed = ability.GoverningSkill;

            GameEventBus.Publish(
                new SocialActionAttemptedEvent(
                    currentAttacker,
                    currentDefender,
                    ability.AbilityName,
                    pendingOutcome.Result <= SocialExchangeResult.WeakSuccess,
                    Time.frameCount));
        }

        #endregion

        #region Exchange Flow

        private void ShowAttackerLine(SocialAbility ability)
        {
            exchangePhase = SocialExchangePhase.AttackerLine;

            string line = ability.GetPlayerLine(pendingOutcome);

            currentAttacker.SpeechBubble.Show(
                line);

            SetActiveSpeaker(currentAttacker);
        }

        private void ShowDefenderResponse()
        {
            exchangePhase = SocialExchangePhase.DefenderResponse;

            int resolve = currentDefender == context.Player
                ? context.PlayerCurrentResolve
                : context.NpcCurrentResolve;

            int maxResolve = currentDefender == context.Player
                ? context.PlayerMaxResolve
                : context.NpcMaxResolve;

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
            exchangePhase = SocialExchangePhase.None;

            if (CheckForEnd())
                return;

            if (state == SocialDuelState.PlayerTurn)
                BeginNpcTurn();
            else
                BeginPlayerTurn();
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

            pendingOutcome = ability.ResolveExchange(currentAttacker, currentDefender);

            ApplyOutcome(currentAttacker, currentDefender, pendingOutcome);

            if (CheckForEnd())
                return;

            ShowAttackerLine(ability);
            context.LastSkillUsed = ability.GoverningSkill;
        }

        #endregion

        #region Resolve & Outcome

        private void ApplyOutcome(
            EntityController attacker,
            EntityController defender,
            SocialExchangeOutcome outcome)
        {
            // -------------------------
            // 1️⃣ Resolve Damage
            // -------------------------

            var target = outcome.DamageSelf ? attacker : defender;

            if (target == context.Player)
            {
                context.PlayerCurrentResolve -= outcome.ResolveAmount;
                duelUI.AnimateResolveChange(context.Player, context.PlayerCurrentResolve);
            }
            else
            {
                context.NpcCurrentResolve -= outcome.ResolveAmount;
                duelUI.AnimateResolveChange(context.Npc, context.NpcCurrentResolve);
            }

            GameEventBus.Publish(
                new ResolveChangedEvent(
                    target,
                    -outcome.ResolveAmount,
                    Time.frameCount));

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

        private bool CheckForEnd()
        {
            if (context.NpcCurrentResolve <= 0)
            {
                EndDuel(SocialDuelOutcome.PlayerVictory);
                return true;
            }

            if (context.PlayerCurrentResolve <= 0)
            {
                EndDuel(SocialDuelOutcome.NpcVictory);
                return true;
            }

            return false;
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
                (float)context.NpcCurrentResolve / context.NpcMaxResolve;

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

            context.Player.SpeechBubble.Hide();
            context.Npc.SpeechBubble.Hide();

            state = SocialDuelState.Completed;
            duelUI.Hide();

            switch (outcome)
            {
                case SocialDuelOutcome.PlayerVictory:
                    Debug.Log("Player wins social duel.");
                    break;

                case SocialDuelOutcome.NpcVictory:
                    Debug.Log("NPC wins social duel.");
                    break;

                case SocialDuelOutcome.Escalation:
                    Debug.Log("Escalates to combat.");
                    break;
            }

            GameEventBus.Publish(
                new SocialDuelEndedEvent(this, outcome, Time.frameCount));
        }

        #endregion
    }
}