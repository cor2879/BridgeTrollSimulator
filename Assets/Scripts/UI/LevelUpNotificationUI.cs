using UnityEngine;
using TMPro;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class LevelUpNotificationUI : MonoBehaviour, IEventSource
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text messageText;
        [SerializeField]
        private LevelUpScreenUI levelUpScreenUI;

        private bool awaitingInput = false;

        public string SourceName => nameof(LevelUpNotificationUI);
        public GameSystemType SystemType => GameSystemType.UI;

        private void Awake()
        {
            panel.SetActive(false);
        }

        private void OnEnable()
        {
            GameEventBus.Subscribe<LevelUpEvent>(OnLevelUp);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<LevelUpEvent>(OnLevelUp);
        }

        private void Update()
        {
            if (!awaitingInput)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                awaitingInput = false;
                OpenLevelUpScreen();
            }
        }

        private void OnLevelUp(LevelUpEvent evt)
        {
            if (!evt.Target.IsPlayerControlled)
                return;

            UpdateNotification(evt.Target);
        }

        private void UpdateNotification(EntityController player)
        {
            if (player.ProgressionPoints <= 0)
            {
                panel.SetActive(false);
                return;
            }

            messageText.text = $"LEVEL UP! ({player.ProgressionPoints} points available)\nPress L to allocate";
            GameEventBus.Publish(
                new PauseRequestEvent(this, Time.frameCount));
            panel.SetActive(true);
            awaitingInput = true;
        }

        private void OpenLevelUpScreen()
        {
            panel.SetActive(false);
            GameEventBus.Publish(
                new ResumeRequestEvent(this, Time.frameCount));

            // levelUpScreenUI.Show(GameDatabase.Instance.Player);
            // GameStateSystem.Instance.SetState(GameState.LevelUp);
        }
    }
}