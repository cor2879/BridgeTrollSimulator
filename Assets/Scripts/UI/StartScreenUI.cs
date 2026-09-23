using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class StartScreenUI : MonoBehaviour, IEventSource
    {
        [SerializeField]
        private GameObject startScreenRoot;

        public string SourceName => nameof(StartScreenUI);

        public GameSystemType SystemType => GameSystemType.UI;

        private void Start()
        {
            startScreenRoot.SetActive(true);

            GameEventBus.Publish(
                new PauseRequestEvent(
                    this,
                    Time.frameCount));
        }

        public void Play()
        {
            GameEventBus.Publish(
                new ResumeRequestEvent(
                    this,
                    Time.frameCount));

            startScreenRoot.SetActive(false);
        }
    }
}