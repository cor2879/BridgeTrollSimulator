using System.Collections;
using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class CombatSystem : MonoBehaviour, IEventSource
    {
        [SerializeField] private CombatUIController combatUI;
        [SerializeField] private float actionDelay = 1.2f;

        private EntityController player;
        private EntityController enemy;

        private CombatState state = CombatState.Inactive;
        private bool isResolving;

        public string SourceName => nameof(CombatSystem);
        public GameSystemType SystemType => GameSystemType.Combat;

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
            player = evt.Initiator;
            enemy = evt.Target;

            player.EnterCombat();
            enemy.EnterCombat();

            combatUI.Initialize(this, player);
            combatUI.Show();

            state = CombatState.PlayerTurn;
            BeginPlayerTurn();
        }

        private void BeginPlayerTurn()
        {
            player.ClearTurnFlags();

            state = CombatState.PlayerTurn;
            player.ProcessTurnStartEffects();
            combatUI.EnableInput(true);
        }

        private IEnumerator ResolvePlayerAbility(Ability ability)
        {
            isResolving = true;
            state = CombatState.ResolvingPlayerAction;
            combatUI.EnableInput(false);

            UseAbility(player, enemy, ability);

            yield return new WaitForSeconds(actionDelay);

            if (enemy.CurrentHealth <= 0)
            {
                state = CombatState.Victory;
                EndCombat(true);
                yield break;
            }

            player.ProcessTurnEndEffects();
            state = CombatState.EnemyTurn;
            yield return StartCoroutine(ResolveEnemyTurn());
        }

        private IEnumerator ResolveEnemyTurn()
        {
            enemy.ClearTurnFlags();
            enemy.ProcessTurnStartEffects();
            state = CombatState.ResolvingEnemyAction;

            var chosenAbility = enemy.ChooseCombatAbility();

            UseAbility(enemy, player, chosenAbility);

            yield return new WaitForSeconds(actionDelay);

            if (player.CurrentHealth <= 0)
            {
                state = CombatState.Defeat;
                EndCombat(false);
                yield break;
            }

            enemy.ProcessTurnEndEffects();
            BeginPlayerTurn();
            isResolving = false;
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

            StartCoroutine(ResolvePlayerAbility(ability));
        }

        private void EndCombat(bool playerWon)
        {
            string combatLog = playerWon ? "Victory!" : "Defeat!";

            GameEventBus.Publish(
                new CombatLogEvent(combatLog, this, Time.frameCount));

            state = CombatState.Inactive;
            combatUI.Hide();

            GameEventBus.Publish(
                new CombatEndedEvent(player, enemy, Time.frameCount));
        }
    }
}