using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Audio;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class CombatSystem : MonoBehaviour, IEventSource
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

        public string SourceName => nameof(CombatSystem);
        public GameSystemType SystemType => GameSystemType.Combat;

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
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<CombatStartedEvent>(OnCombatStarted);
            GameEventBus.Unsubscribe<CombatConfirmedEvent>(OnCombatConfirmed);
            GameEventBus.Unsubscribe<CombatPreSummaryConfirmedEvent>(OnCombatPreSummaryConfirmed);
            GameEventBus.Unsubscribe<CombatEndedEvent>(OnCombatEnded);
            GameEventBus.Unsubscribe<CombatResolutionCompletedEvent>(OnCombatResolutionCompleted);
        }

        #region Event Handlers

        private void OnCombatStarted(CombatStartedEvent evt)
        {
            battleIntroUI.Show(
                "BATTLE BEGINS",
                "Press Any Key to Continue",
                evt);

            GameStateSystem.Instance.SetState(GameState.Combat);
            AudioSystem.Instance.PlayCombatMusic();
        }

        private void OnCombatConfirmed(CombatConfirmedEvent evt)
        {
            evt.Initiator.EnterCombat();
            evt.Target.EnterCombat();

            combatants = new List<EntityController>
            {
                evt.Initiator,
                evt.Target
            };

            BuildInitiativeOrder();
            var teamA = new List<EntityController>() { evt.Initiator };
            var teamB = new List<EntityController>() { evt.Target };

            battlePreSummaryUI.Show(
                teamA, teamB, initiativeOrder.First().Entity, evt);
        }

        private void OnCombatPreSummaryConfirmed(CombatPreSummaryConfirmedEvent evt)
        {
            combatUI.Initialize(
                this, 
                GameDatabase.Instance.Player);
            combatUI.Show();

            currentTurnIndex = 0;
            roundNumber = 1;

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

        #endregion

        private CombatResolutionData BuildResolution(CombatEndedEvent evt)
        {
            var player = evt.Initiator;
            var enemy = evt.Target;
            var outcome = evt.Outcome;
            CombatFaction winningFaction = CombatFaction.Neutral;
            var experience = 0;
            var fame = 0;
            var respect = 0;
            var reputation = 0;
            var gold = 0;

            switch (outcome)
            {
                case CombatOutcome.PlayerVictory_EnemyKilled:
                    winningFaction = CombatFaction.Player;
                    experience = enemy.ExperienceReward;
                    fame = enemy.Fame / 2;
                    respect = enemy.Respect;
                    reputation = -enemy.Reputation;
                    gold = enemy.DeductGold(enemy.Gold);
                    break;
                case CombatOutcome.PlayerVictory_EnemyAlive:
                    winningFaction = CombatFaction.Player;
                    experience = enemy.ExperienceReward;
                    fame = enemy.Fame;
                    respect = enemy.Respect;
                    reputation = System.Math.Max(0, enemy.Reputation);
                    gold = 0;
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
                experience,
                fame,
                respect,
                reputation,
                gold);
            
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
                combatUI.EnableInput(true);
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
            var target = ChooseTarget(entity);
            var ability = entity.ChooseBestCombatAbility(target);

            UseAbility(entity, target, ability);

            yield return new WaitForSeconds(actionDelay);

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

            yield return new WaitForSeconds(actionDelay);

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
            GameEventBus.Publish(
                new ResumeRequestEvent(this, Time.frameCount));
            var snapshot = combatants.ToList();

            GameDatabase.Instance.Player.ResetControlMode(overrideDeath: false);

            GameEventBus.Publish<CombatRewardEvent>(
                new CombatRewardEvent(
                    this,
                    GameDatabase.Instance.Player,
                    resolutionData.Experience,
                    resolutionData.FameDelta,
                    resolutionData.RespectDelta,
                    resolutionData.ReputationDelta,
                    resolutionData.GoldReward,
                    Time.frameCount));
            
            foreach (var entity in snapshot.Where(e => e.CurrentControlMode == ControlMode.Dead))
            {
                entity.BeginDespawn();
            }

            combatants.Clear();
            initiativeOrder.Clear();
        }
    }
}