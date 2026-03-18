using System.Collections;

using TMPro;
using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class CombatIntroUI : ModalUIBase, IEventSource
    {
        [SerializeField] private GameObject panelRoot;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text subText;
        private CombatStartedEvent combatStartedEvent;
        private int showFrame;

        private bool awaitingInput;

        public override string SourceName => this.name;
        public override GameSystemType SystemType => GameSystemType.UI;
        public override bool IsBlockingUI => true;

        private void Awake()
        {
            panelRoot.SetActive(false);
        }

        public void Show(string title, string subTitle, CombatStartedEvent combatStartedEvent)
        {
            titleText.text = title;
            subText.text = subTitle;
            this.combatStartedEvent = combatStartedEvent;

            ShowModal(panelRoot);

            awaitingInput = true;
            showFrame = Time.frameCount;

            panelRoot.transform.localScale = Vector3.zero;
            StartCoroutine(PopIn());
        }

        private void Update()
        {
            if (!awaitingInput)
            {
                return;
            }

            if (Time.frameCount == showFrame)
            {
                return;
            }

            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                awaitingInput = false;
                HideModal(panelRoot);

                var player = combatStartedEvent.Initiator.IsPlayerControlled ? 
                    combatStartedEvent.Initiator :
                    combatStartedEvent.Target;
                var npc = combatStartedEvent.Initiator.IsPlayerControlled ?
                    combatStartedEvent.Target :
                    combatStartedEvent.Initiator;

                GameEventBus.Publish(
                    new CombatConfirmedEvent(player, npc, Time.frameCount));
                combatStartedEvent = null;
            }
        }

        private IEnumerator PopIn()
        {
            float duration = 0.50f;
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;

                panelRoot.transform.localScale =
                    Vector3.Lerp(Vector3.zero, Vector3.one, t);

                yield return null;
            }

            panelRoot.transform.localScale = Vector3.one;
        }
    }
}