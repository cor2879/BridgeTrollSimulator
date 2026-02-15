using System;
using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameState
{
    public class GameStateController : MonoBehaviour
    {
        private void OnEnable()
        {
            GameEventBus.Subscribe<PauseRequestEvent>(OnPauseRequested);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<PauseRequestEvent>(OnPauseRequested);
        }

        private void OnPauseRequested(PauseRequestEvent evt)
        {
            Time.timeScale = 0;
            Debug.Log($"{evt}");
        }
    }
}