using System;
using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Audio;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement
{
    public class GameStateController : MonoBehaviour
    {
        private void Awake()
        {
            AudioSystem.Instance.PlayOverworldMusic();    
        }

        private void OnEnable()
        {
            GameEventBus.Subscribe<CombatEndedEvent>(OnCombatEnded);
            GameEventBus.Subscribe<CombatResolutionCompletedEvent>(OnCombatResolutionCompleted);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<CombatEndedEvent>(OnCombatEnded);
            GameEventBus.Unsubscribe<CombatResolutionCompletedEvent>(OnCombatResolutionCompleted);
        }

        private void OnCombatEnded(CombatEndedEvent evt)
        {
            if (evt.Outcome <= CombatOutcome.PlayerVictory_EnemyKilled)
            {
                AudioSystem.Instance.PlayVictoryMusic();
            }
            else
            {
                AudioSystem.Instance.PlayDefeatMusic();
            }
        }

        private void OnCombatResolutionCompleted(CombatResolutionCompletedEvent evt)
        {
            AudioSystem.Instance.PlayOverworldMusic();
        }
    }
}