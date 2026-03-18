using UnityEngine;
using TMPro;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class LevelUpNotificationUI : ModalUIBase
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text messageText;

        private bool awaitingInput = false;

        public override string SourceName => nameof(LevelUpNotificationUI);
        public override GameSystemType SystemType => GameSystemType.UI;
        public override bool IsBlockingUI => true;

        private void Awake()
        {
            panel.SetActive(false);
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

        public void Show(EntityController target)
        {
            if (!target.IsPlayerControlled)
                return;

            UpdateNotification(target);
        }

        private void UpdateNotification(EntityController player)
        {
            if (player.ProgressionPoints <= 0)
            {
                panel.SetActive(false);
                return;
            }

            messageText.text = $"LEVEL UP! ({player.ProgressionPoints} points available)";
            ShowModal(panel);
            awaitingInput = true;
        }

        private void OpenLevelUpScreen()
        {
            HideModal(panel);

            GameEventBus.Publish(
                new LevelUpNotificationDismissedEvent(
                    this,
                    GameDatabase.Instance.Player,
                    Time.frameCount));
        }
    }
}