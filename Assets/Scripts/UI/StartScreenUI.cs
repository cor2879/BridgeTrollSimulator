using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Platform;
using UnityEngine;

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
            WebGLDiagnostics.Trace($"StartScreen.Start rootNull={startScreenRoot == null}");

            if (startScreenRoot != null)
            {
                startScreenRoot.SetActive(true);
            }

            GameEventBus.Publish(
                new PauseRequestEvent(
                    this,
                    Time.frameCount));
        }

        public void Play()
        {
            WebGLDiagnostics.Trace("StartScreen.Play invoked.");

            GameEventBus.Publish(
                new ResumeRequestEvent(
                    this,
                    Time.frameCount));

            if (startScreenRoot != null)
            {
                startScreenRoot.SetActive(false);
            }

            WebGLDiagnostics.SnapshotUI("after StartScreen.Play");
        }
    }
}
