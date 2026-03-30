using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class ConfirmationDialogUI : ModalUIBase
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;

        private Action onYes;
        private Action onNo;

        public override string SourceName => nameof(ConfirmationDialogUI);
        public override GameSystemType SystemType => GameSystemType.UI;
        public override bool IsBlockingUI => true;

        private void Awake()
        {
            panel.SetActive(false);

            yesButton.onClick.AddListener(OnYesClicked);
            noButton.onClick.AddListener(OnNoClicked);
        }

        public void Show(string message, Action yesCallback, Action noCallback = null)
        {
            messageText.text = message;

            onYes = yesCallback;
            onNo = noCallback;

            ShowModal(panel);
        }

        public void Cancel()
        {
            OnNoClicked();
        }

        private void OnYesClicked()
        {
            HideModal(panel);
            onYes?.Invoke();
        }

        private void OnNoClicked()
        {
            HideModal(panel);
            onNo?.Invoke();
        }
    }
}