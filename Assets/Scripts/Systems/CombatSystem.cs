using System;

using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class CombatSystem : MonoBehaviour
    {
        [SerializeField]
        private CombatUIController combatUI;

        private EntityController player;
        private EntityController enemy;

        private CombatState state = CombatState.Inactive;

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

            combatUI.Initialize(this);
            combatUI.Show();

            state = CombatState.PlayerTurn;
            Debug.Log($"Combat Started: {evt}");

            UpdateUI();
            BeginPlayerTurn();
        }

        private void BeginPlayerTurn()
        {
            Debug.Log("Player Turn");
            ShowCombatMenu();
        }

        private void UpdateUI()
        {
            combatUI.UpdateStats(player, enemy);
        }

        public void PlayerAttack()
        {
            if (state != CombatState.PlayerTurn)
            {
                return;
            }

            ExecuteAttack(player, enemy);

            if (enemy.CurrentHealth <= 0)
            {
                state = CombatState.Victory;
                EndCombat(true);
                return;
            }
            state = CombatState.EnemyTurn;
            BeginEnemyTurn();
        }

        private void ExecuteAttack(EntityController attacker, EntityController defender)
        {
            var damage = Mathf.Max(1, attacker.Attack - defender.Defense);
            defender.CurrentHealth -= damage;

            Debug.Log($"{attacker.Name} deals {damage} to {defender.Name}");
        }

        private void BeginEnemyTurn()
        {
            Debug.Log("EnemyTurn");

            ExecuteAttack(enemy, player);

            if (player.CurrentHealth <= 0)
            {
                state = CombatState.Defeat;
                EndCombat(false);
                return;
            }

            state = CombatState.PlayerTurn;
            BeginPlayerTurn();
        }

        private void EndCombat(bool playerWon)
        {
            Debug.Log(playerWon ? "Victory!" : "Defeat!");

            state = CombatState.Inactive;

            combatUI.Hide();

            GameEventBus.Publish(
                new CombatEndedEvent(player, enemy, Time.frameCount));
        }

        private void ShowCombatMenu()
        {
            combatUI.Show();
        }
    }
}