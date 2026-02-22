using System;
using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Audio;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameState
{
    public class GameStateController : MonoBehaviour
    {
        private void Awake()
        {
            AudioSystem.Instance.PlayOverworldMusic();    
        }

        private void OnEnable()
        {
            GameEventBus.Subscribe<PauseRequestEvent>(OnPauseRequested);
            GameEventBus.Subscribe<CombatEndedEvent>(OnCombatEnded);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<PauseRequestEvent>(OnPauseRequested);
            GameEventBus.Unsubscribe<CombatEndedEvent>(OnCombatEnded);
        }

        private void OnPauseRequested(PauseRequestEvent evt)
        {
            Time.timeScale = 0;
            Debug.Log($"{evt}");
        }

        private void OnCombatEnded(CombatEndedEvent evt)
        {
            AudioSystem.Instance.PlayOverworldMusic();
        }
    }
}