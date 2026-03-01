using System.Collections.Generic;
using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class LevelUpQueueSystem : MonoBehaviour
    {
        [SerializeField]
        private LevelUpNotificationUI notificationUI;
        [SerializeField]
        private LevelUpScreenUI levelUpScreenUI;

        private readonly Queue<LevelUpEvent> queue = new();
        private LevelUpEvent activeLevelUp;

        private void OnEnable()
        {
            GameEventBus.Subscribe<LevelUpEvent>(OnLevelUp);
            GameEventBus.Subscribe<GameStateChangedEvent>(OnGameStateChanged);
            GameEventBus.Subscribe<LevelUpNotificationDismissedEvent>(OnNotificationDismissed);
            GameEventBus.Subscribe<LevelUpConfirmedEvent>(OnLevelUpConfirmed);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<LevelUpEvent>(OnLevelUp);
            GameEventBus.Unsubscribe<GameStateChangedEvent>(OnGameStateChanged);
            GameEventBus.Unsubscribe<LevelUpNotificationDismissedEvent>(OnNotificationDismissed);
            GameEventBus.Unsubscribe<LevelUpConfirmedEvent>(OnLevelUpConfirmed);
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

        private void OnNotificationDismissed(LevelUpNotificationDismissedEvent evt)
        {
            if (activeLevelUp == null)
            {
                return;
            }

            GameStateSystem.Instance.SetState(GameState.LevelUp);
            levelUpScreenUI.Show(evt.Target);
        }

        private void OnLevelUpConfirmed(LevelUpConfirmedEvent evt)
        {
            activeLevelUp = null;

            ProcessQueue();
        }

        private void ProcessQueue()
        {
            if (activeLevelUp != null)
            {
                return;
            }

            if (queue.Count == 0)
            {
                return;
            }

            activeLevelUp = queue.Dequeue();
            notificationUI.Show(activeLevelUp.Target);
        }
    }
}