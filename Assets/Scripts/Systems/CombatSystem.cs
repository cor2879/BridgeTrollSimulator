using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Audio;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Scenarios;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Rewards;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Rewards.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class CombatSystem : MonoBehaviour, IModalUI
    {
        [SerializeField] 
        private CombatUIController combatUI;
        [SerializeField] 
        private CombatIntroUI battleIntroUI;
        [SerializeField]
        private CombatPreSummaryUI battlePreSummaryUI;
        [SerializeField]
        private CombatResolutionUI combatResolutionUI;

        [SerializeField] private float actionDelay = 1.2f;

        private IFactionDispositionResolver dispositionResolver;

        private CombatState state = CombatState.Inactive;
        private bool isResolving;

        [SerializeField]
        private List<EntityController> combatants;
        [SerializeField, ReadOnly]
        private OrderedCollection<InitiativeEntry> initiativeOrder = new();
        [SerializeField, ReadOnly]
        private int currentTurnIndex = 0;
        [SerializeField, ReadOnly]
        private int roundNumber = 1;
        [SerializeField, ReadOnly]
        private bool awaitingPlayerDecision = false;
        [SerializeField, ReadOnly]
        private bool playerCancelledAction = false;
        [SerializeField, ReadOnly]
        private bool showCombatControls = true;

        public string SourceName => nameof(CombatSystem);
        public GameSystemType SystemType => GameSystemType.Combat;

        #region IModalUI

        public bool IsBlockingUI => false;

        #endregion

        #region Singleton

        private static CombatSystem _instance;

        public static CombatSystem Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindFirstObjectByType<CombatSystem>();

                return _instance;
            }
        }

        #endregion

        #region Initialization

        private void Awake()
        {
            dispositionResolver = new MatrixDispositionResolver(
                GameDatabase.Instance.Dispositions);
        }

        private void OnEnable()
        {
            GameEventBus.Subscribe<CombatStartedEvent>(OnCombatStarted);
            GameEventBus.Subscribe<CombatConfirmedEvent>(OnCombatConfirmed);
            GameEventBus.Subscribe<CombatPreSummaryConfirmedEvent>(OnCombatPreSummaryConfirmed);
            GameEventBus.Subscribe<CombatEndedEvent>(OnCombatEnded);
            GameEventBus.Subscribe<CombatResolutionCompletedEvent>(OnCombatResolutionCompleted);
            GameEventBus.Subscribe<CombatSurrenderAcceptedEvent>(OnCombatSurrenderAccepted);
            GameEventBus.Subscribe<ConcedeCombatEvent>(OnConcededCombat);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<CombatStartedEvent>(OnCombatStarted);
            GameEventBus.Unsubscribe<CombatConfirmedEvent>(OnCombatConfirmed);
            GameEventBus.Unsubscribe<CombatPreSummaryConfirmedEvent>(OnCombatPreSummaryConfirmed);
            GameEventBus.Unsubscribe<CombatEndedEvent>(OnCombatEnded);
            GameEventBus.Unsubscribe<CombatResolutionCompletedEvent>(OnCombatResolutionCompleted);
            GameEventBus.Unsubscribe<CombatSurrenderAcceptedEvent>(OnCombatSurrenderAccepted);
            GameEventBus.Unsubscribe<ConcedeCombatEvent>(OnConcededCombat);
        }

        #endregion

        #region Event Handlers

        private void OnCombatStarted(CombatStartedEvent evt)
        {
            battleIntroUI.Show(
                "BATTLE BEGINS",
                "Press Any Key to Continue",
                evt);

            GameStateSystem.Instance.SetState(GameState.Combat);
            ModalUISystem.Instance.OpenModal(this);
            AudioSystem.Instance.PlayCombatMusic();
        }

        private void OnCombatConfirmed(CombatConfirmedEvent evt)
        {
            var initiator = evt.Initiator as EntityController;
            var target = evt.Target as EntityController;

            initiator.EnterCombat();
            target.EnterCombat();

            combatants = new List<EntityController>
            {
                initiator,
                target
            };

            BuildInitiativeOrder();
            var teamA = new List<EntityController>() { initiator };
            var teamB = new List<EntityController>() { target };

            battlePreSummaryUI.Show(
                teamA, teamB, initiativeOrder.First().Entity, evt);
        }

        private void OnCombatPreSummaryConfirmed(CombatPreSummaryConfirmedEvent evt)
        {
            combatUI.Initialize(
                this, 
                GameDatabase.Instance.Player);

            currentTurnIndex = 0;
            roundNumber = 1;

            isResolving = false;
            awaitingPlayerDecision = false;
            playerCancelledAction = false;

            BeginTurn(GetCurrentCombatant());
        }

        private void OnCombatEnded(CombatEndedEvent evt)
        {
            var currentResolutionData = BuildResolution(evt);

            GameStateSystem.Instance.SetState(GameState.CombatResolution);
            combatResolutionUI.Show(currentResolutionData);
        }

        private void OnCombatResolutionCompleted(CombatResolutionCompletedEvent evt)
        {
            FinalizeCombat(evt.Data);
            GameStateSystem.Instance.SetState(GameState.World);
        }

        private void OnCombatSurrenderAccepted(CombatSurrenderAcceptedEvent evt)
        {
            var winner = evt.Initiator as EntityController;
            var loser = evt.Target as EntityController;

            var outcome = winner.IsPlayerControlled ?
                CombatOutcome.PlayerVictory_EnemyAlive :
                CombatOutcome.PlayerDefeated;

            EndCombat(outcome);
        }

        private void OnConcededCombat(ConcedeCombatEvent evt)
        {
            var initiator = evt.Initiator as EntityController;
            var target = evt.Target as EntityController;

            if (initiator == null || target == null)
                return;

            if (initiator.IsPlayerControlled)
            {
                ResolveSurrenderReaction(initiator, target, evt); 
            }
            else
            {
                HandleNpcSurrenderOffer(evt);
            }
        }


        #endregion

        #region Public API

        public void ConcedeCombat(IReactor initiator, IReceiver target)
        {
            Debug.Log($"{nameof(ConcedeCombat)}::initiator:{initiator.Name}::target:{target.Name}");

            if (initiator.IsPlayerControlled)
            {
                awaitingPlayerDecision = true;

                ModalUISystem.Instance.ShowConfirmationDialog(
                    "Really concede this battle?",
                    () => 
                    {
                        awaitingPlayerDecision = false;
                        initiator.ConcedeCombat(target);
                    },
                    () =>
                    {
                        awaitingPlayerDecision = false;
                        playerCancelledAction = true;
                    });
            }
            else
            {
                initiator.ConcedeCombat(target);
            }
        }

        #endregion

        private void ResolveSurrenderReaction(
            EntityController player,
            EntityController npc,
            ConcedeCombatEvent evt)
        {
            var chosen = ReactionResolver.Resolve(
                SurrenderScenario.Instance,
                npc, 
                player,
                evt);

            chosen.Execute(npc, player, evt);
        }

        private void HandleNpcSurrenderOffer(ConcedeCombatEvent evt)
        {
            var npc = evt.Initiator as IResolver;
            var player = evt.Target;

            showCombatControls = false;
            combatUI.EnableInput(showCombatControls);
            var text = !string.IsNullOrEmpty(evt.Concession?.Description) ?
                evt.Concession.Description :
                $"{npc.Name} is offering to surrender. Do you accept?";

            ModalUISystem.Instance.ShowConfirmationDialog(
                text,
                onYes: () =>
                {
                    npc.DemandComponent.AddDemand(evt.Concession);
                    showCombatControls = true;
                    GameEventBus.Publish(
                        new CombatSurrenderAcceptedEvent(player, npc, Time.frameCount));
                },
                onNo: () =>
                {
                    showCombatControls = true;
                    GameEventBus.Publish(
                        new CombatSurrenderDeniedEvent(player, npc, Time.frameCount));
                    BeginTurn(player as EntityController);
                });
        }

        private CombatResolutionData BuildResolution(CombatEndedEvent evt)
        {
            var player = evt.Initiator as EntityController;
            var enemy = evt.Target as EntityController;
            var outcome = evt.Outcome;
            CombatFaction winningFaction = CombatFaction.Neutral;
            var experience = 0;
            var fame = 0;
            var respect = 0;
            var reputation = 0;
            var gold = 0;
            var resolve = 0;

            switch (outcome)
            {
                case CombatOutcome.PlayerVictory_EnemyKilled:
                    winningFaction = CombatFaction.Player;
                    experience = enemy.ExperienceReward;
                    fame = enemy.Fame / 2;
                    respect = enemy.Respect;
                    reputation = -enemy.Reputation;
                    gold = enemy.DeductGold(enemy.Gold);
                    resolve = player.MaxResolve / 2;
                    break;
                case CombatOutcome.PlayerVictory_EnemyAlive:
                    winningFaction = CombatFaction.Player;
                    experience = enemy.ExperienceReward;
                    fame = enemy.Fame;
                    respect = enemy.Respect;
                    reputation = System.Math.Max(0, enemy.Reputation);
                    gold = 0;
                    resolve = player.MaxResolve / 2;
                    break;
                case CombatOutcome.PlayerDefeated:
                    winningFaction = CombatFaction.Enemy;
                    experience = (int)(enemy.ExperienceReward * 0.1f);
                    fame = -enemy.Fame / 2;
                    respect = -enemy.Respect;
                    reputation = 0;
                    gold = 0;
                    break;
                case CombatOutcome.PlayerKilled:
                    winningFaction = CombatFaction.Enemy;
                    experience = 0;
                    fame = -enemy.Fame;
                    respect = -enemy.Respect;
                    reputation = 0;
                    gold = 0;
                    break;
            }

            var resolutionData = new CombatResolutionData(
                outcome,
                new List<EntityController>() { player },
                new List<EntityController>() { enemy },
                winningFaction,
                new RewardBundle(
                    experience,
                    gold,
                    fame,
                    respect,
                    reputation,
                    resolve));
            
            return resolutionData;
        }

        private void RollInitiative()
        {
            foreach (var entity in combatants)
            {
                entity.RollInitiative();
            }
        }

        private void BuildInitiativeOrder()
        {
            initiativeOrder.Clear();

            foreach (var entity in combatants)
            {
                entity.RollInitiative();
                initiativeOrder.Add(
                    new InitiativeEntry(entity));
            }
        }

        private EntityController GetCurrentCombatant()
        {
            return initiativeOrder[currentTurnIndex].Entity;
        }

        private void BeginTurn(EntityController entity)
        {
            entity.ClearTurnFlags();

            if (entity.IsPlayerControlled)
            {
                state = CombatState.PlayerTurn;
                combatUI.EnableInput(showCombatControls);
            }
            else
            {
                state = CombatState.ResolvingEnemyAction;
                StartCoroutine(ResolveAI(entity));
            }
        }

        private IEnumerator ResolveAI(EntityController entity)
        {
            isResolving = true;

            // 🔥 EARLY EXIT if combat already ended
            if (state == CombatState.Resolving || state == CombatState.Inactive)
            {
                isResolving = false;
                yield break;
            }

            var target = ChooseTarget(entity);
            var ability = entity.ChooseBestCombatAbility(target);

            UseAbility(entity, target, ability);

            while (awaitingPlayerDecision)
            {
                yield return null;

                // 🔥 ALSO GUARD HERE
                if (state == CombatState.Resolving || state == CombatState.Inactive)
                {
                    isResolving = false;
                    yield break;
                }
            }

            yield return new WaitForSeconds(actionDelay);

            // 🔥 FINAL GUARD before applying effects
            if (state == CombatState.Resolving || state == CombatState.Inactive)
            {
                isResolving = false;
                yield break;
            }

            entity.ProcessTurnEndEffects();

            isResolving = false;
            AdvanceTurn();
        }

        private EntityController ChooseTarget(EntityController attacker)
        {
            return combatants
                .Where(e =>
                    e.CurrentHealth > 0 &&
                    dispositionResolver.IsHostile(attacker.Faction, e.Faction))
                .FirstOrDefault();
        }

        private void UseAbility(EntityController initiator, EntityController target, Ability ability)
        {
            CombatResolver.ResolveAbility(initiator, target, ability, this);
        }

        public void PlayerUseAbility(Ability ability)
        {
            if (state != CombatState.PlayerTurn || isResolving)
            {
                return;
            }

            StartCoroutine(ResolvePlayerTurn(ability));
        }

        private IEnumerator ResolvePlayerTurn(Ability ability)
        {
            isResolving = true;

            var player = GameDatabase.Instance.Player;
            var target = ChooseTarget(player);

            combatUI.EnableInput(false);
            UseAbility(player, target, ability);

            while (awaitingPlayerDecision)
            {
                yield return null;

                if (state == CombatState.Resolving || state == CombatState.Inactive)
                {
                    isResolving = false;
                    yield break;
                }
            }

            if (playerCancelledAction)
            {
                playerCancelledAction = false;
                isResolving = false;
                BeginTurn(player);
                yield break;
            }

            yield return new WaitForSeconds(actionDelay);

            // 🔥 CRITICAL GUARD
            if (state == CombatState.Resolving || state == CombatState.Inactive)
            {
                isResolving = false;
                yield break;
            }

            player.ProcessTurnEndEffects();
            isResolving = false;
            AdvanceTurn();
        }

        private void AdvanceTurn()
        {
            currentTurnIndex++;

            if (currentTurnIndex >= initiativeOrder.Count)
            {
                currentTurnIndex = 0;
                roundNumber++;
            }

            if (CheckCombatEnd())
            {
                return;
            }

            var combatant = GetCurrentCombatant();
            combatant.ProcessTurnStartEffects();
            BeginTurn(combatant);
        }

        private bool CheckCombatEnd()
        {
            var aliveCombatants = combatants
                .Where(e => e.CurrentHealth > 0)
                .ToList();

            var aliveFactions = aliveCombatants
                .Select(e => e.Faction)
                .Distinct()
                .ToList();

            if (aliveFactions.Count == 0)
            {
                EndCombat(CombatOutcome.PlayerKilled);
                return true;
            }

            if (aliveFactions.Count == 1)
            {
                var player = GameDatabase.Instance.Player;
                var remainingFaction = aliveFactions.First();
                
                if (remainingFaction != CombatFaction.Player)
                {
                    EndCombat(CombatOutcome.PlayerKilled);
                    return true;
                }

                EndCombat(CombatOutcome.PlayerVictory_EnemyKilled);
                return true;
            }

            return false;
        }

        private void EndCombat(CombatOutcome combatOutcome)
        {
            state = CombatState.Resolving;

            GameEventBus.Publish(
                new CombatEndedEvent(
                    combatants[0], 
                    combatants[1], 
                    combatOutcome,
                    Time.frameCount));
        }

        private void FinalizeCombat(CombatResolutionData resolutionData)
        {
            state = CombatState.Inactive;
            combatUI.Hide();

            var outcome = resolutionData.Outcome;
            var snapshot = combatants.ToList();
            var player = resolutionData.PlayerSide.First();
            var npc = resolutionData.EnemySide.First();

            player.ResetControlMode(overrideDeath: false);

            GameEventBus.Publish<RewardEvent>(
                new RewardEvent(
                    this,
                    GameDatabase.Instance.Player,
                    resolutionData.Reward,
                    Time.frameCount));
            
            switch (outcome)
            {
                case CombatOutcome.PlayerVictory_EnemyAlive:
                case CombatOutcome.PlayerVictory_EnemyKilled:
                    player.OnCombatVictory(resolutionData);
                    npc?.OnCombatDefeat(resolutionData);
                    break;

                case CombatOutcome.PlayerDefeated:
                    player.OnCombatDefeat(resolutionData);
                    npc?.OnCombatVictory(resolutionData);
                    break;

                case CombatOutcome.PlayerKilled:
                    player.OnCombatDefeat(resolutionData);
                    npc?.OnCombatVictory(resolutionData);
                    break;
            }

            foreach (var entity in snapshot.Where(e => e.CurrentControlMode == ControlMode.Dead))
            {
                entity.BeginDespawn();
            }

            combatants.Clear();
            initiativeOrder.Clear();
            ModalUISystem.Instance.CloseModal(this);
        }
    }
}