using System.Collections.Generic;
using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class LevelUpQueueSystem : MonoBehaviour
    {
        private readonly Queue<LevelUpEvent> queue = new();

        private void OnEnable()
        {
            GameEventBus.Subscribe<LevelUpEvent>(OnLevelUp);
            GameEventBus.Subscribe<GameStateChangedEvent>(OnGameStateChanged);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<LevelUpEvent>(OnLevelUp);
            GameEventBus.Unsubscribe<GameStateChangedEvent>(OnGameStateChanged);
        }

        private void OnLevelUp(LevelUpEvent evt)
        {
            queue.Enqueue(evt);
        }

        private void OnGameStateChanged(GameStateChangedEvent evt)
        {
            if (evt.Current == GameState.World)
            {
                ProcessQueue();
            }
        }

        private void ProcessQueue()
        {
            while (queue.Count > 0)
            {
                var levelUpEvent = queue.Dequeue();

                // For now just log
                Debug.Log($"LEVEL UP! Level {levelUpEvent.NewLevel}");

                // Later:
                // Open LevelUp UI
                // Grant stat points
                // Trigger animation
            }
        }
    }
}