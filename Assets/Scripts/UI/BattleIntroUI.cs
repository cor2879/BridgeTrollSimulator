using System.Collections;

using TMPro;
using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class BattleIntroUI : MonoBehaviour, IEventSource
    {
        [SerializeField] private GameObject panelRoot;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text subText;
        private CombatStartedEvent combatStartedEvent;

        private bool awaitingInput;

        public string SourceName => this.name;
        public GameSystemType SystemType => GameSystemType.UI;

        private void Awake()
        {
            panelRoot.SetActive(false);
        }

        public void Show(string title, string subTitle, CombatStartedEvent combatStartedEvent)
        {
            titleText.text = title;
            subText.text = subTitle;
            this.combatStartedEvent = combatStartedEvent;

            panelRoot.SetActive(true);
            awaitingInput = true;
            panelRoot.transform.localScale = Vector3.zero;
            StartCoroutine(PopIn());
        }

        private void Update()
        {
            if (!awaitingInput)
            {
                return;
            }

            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                awaitingInput = false;
                panelRoot.SetActive(false);

                GameEventBus.Publish(
                    new CombatConfirmedEvent(combatStartedEvent.Initiator, combatStartedEvent.Target, Time.frameCount));
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