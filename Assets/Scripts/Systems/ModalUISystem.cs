using System.Collections.Generic;
using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class ModalUISystem : MonoBehaviour, IEventSource
    {
        private static ModalUISystem _instance;

        [SerializeField, ReadOnly]
        private int activeModalCount = 0;

        public string SourceName => nameof(ModalUISystem);
        public GameSystemType SystemType => GameSystemType.System;

        private HashSet<IEventSource> openModals = new HashSet<IEventSource>();

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

        public void OpenModal(IEventSource source)
        {
            if (openModals.Contains(source)) // Modal is already being tracked
            {
                return;
            }

            activeModalCount++;
            openModals.Add(source);

            if (activeModalCount == 1)
            {
                GameEventBus.Publish(
                    new PauseRequestEvent(source, Time.frameCount));
            }
        }

        public void CloseModal(IEventSource source)
        {
            if (!openModals.Contains(source))
            {
                return;
            }

            openModals.Remove(source);
            activeModalCount = Mathf.Max(0, activeModalCount - 1);

            if (activeModalCount == 0)
            {
                GameEventBus.Publish(
                    new ResumeRequestEvent(source, Time.frameCount));
            }
        }
    }
}