using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameState;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class CombatSystem : MonoBehaviour, IEventSource
    {
        [SerializeField] private CombatUIController combatUI;
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
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<CombatStartedEvent>(OnCombatStarted);
        }

        private void OnCombatStarted(CombatStartedEvent evt)
        {
            evt.Initiator.EnterCombat();
            evt.Target.EnterCombat();

            combatants = new List<EntityController>
            {
                evt.Initiator,
                evt.Target
            };

            BuildInitiativeOrder();

            combatUI.Initialize(
                this, 
                GameDatabase.Instance.Player);
            combatUI.Show();

            currentTurnIndex = 0;
            roundNumber = 1;

            BeginTurn(GetCurrentCombatant());
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

            if (aliveFactions.Count <= 1)
            {
                var player = GameDatabase.Instance.Player;

                bool playerWon = player != null &&
                                player.CurrentHealth > 0 &&
                                aliveFactions.Contains(player.Faction);

                EndCombat(playerWon);
                return true;
            }

            return false;
        }

        private void EndCombat(bool playerWon)
        {
            state = CombatState.Inactive;
            combatUI.Hide();

            combatants.Clear();
            initiativeOrder.Clear();

            var log = playerWon ? "Victory!" : "Defeat!";

            GameEventBus.Publish(
                new CombatLogEvent(log, this, Time.frameCount));
        }
    }
}