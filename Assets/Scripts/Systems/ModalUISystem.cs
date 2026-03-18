using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class ModalUISystem : MonoBehaviour, IEventSource
    {
        private static ModalUISystem _instance;

        [SerializeField, ReadOnly]
        private int activeModalCount = 0;

        [SerializeField]
        private ConfirmationDialogUI confirmationDialogUI;

        public string SourceName => nameof(ModalUISystem);
        public GameSystemType SystemType => GameSystemType.System;

        private HashSet<IModalUI> openModals = new HashSet<IModalUI>();

        public bool IsBlockingWorldUI
        {
            get
            {
                return openModals.Any(m => m.IsBlockingUI);
            }
        }

        public static ModalUISystem Instance 
        { 
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<ModalUISystem>();
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        public void OpenModal(IModalUI source)
        {
            if (openModals.Contains(source)) // Modal is already being tracked
            {
                #if UNITY_EDITOR
                Debug.Log($"{nameof(OpenModal)}::SourceName:{source.SourceName}::'Duplicate'");
                #endif
                return;
            }

            #if UNITY_EDITOR
            Debug.Log($"{nameof(OpenModal)}::SourceName:{source.SourceName}");
            #endif

            activeModalCount++;
            openModals.Add(source);

            if (activeModalCount == 1)
            {
                GameEventBus.Publish(
                    new PauseRequestEvent(source, Time.frameCount));
            }
        }

        public void CloseModal(IModalUI source)
        {
            if (!openModals.Contains(source))
            {
                return;
            }

            #if UNITY_EDITOR
            Debug.Log($"{nameof(CloseModal)}::SourceName:{source.SourceName}");
            #endif

            openModals.Remove(source);
            activeModalCount = Mathf.Max(0, activeModalCount - 1);

            if (activeModalCount == 0)
            {
                GameEventBus.Publish(
                    new ResumeRequestEvent(source, Time.frameCount));
            }
        }

        public void ShowConfirmationDialog(
            string message,
            System.Action onYes,
            System.Action onNo = null)
        {
            if (confirmationDialogUI == null)
            {
                Debug.LogError("ConfirmationDialogUI not assigned in Editor.");
                return;
            }

            confirmationDialogUI.Show(message, onYes, onNo);
        }
    }
}