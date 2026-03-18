using System.Collections;
using TMPro;
using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class SocialDuelIntroUI : ModalUIBase, IEventSource
    {
        [Header("Root")]
        [SerializeField] private GameObject panelRoot;

        [Header("Text Fields")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text subText;

        private EntityController player;
        private EntityController npc;

        private int showFrame;
        private bool awaitingInput;

        public override string SourceName => this.name;
        public override GameSystemType SystemType => GameSystemType.UI;
        public override bool IsBlockingUI => true;

        private void Awake()
        {
            panelRoot.SetActive(false);
        }

        public void Show(EntityController player, EntityController npc)
        {
            this.player = player;
            this.npc = npc;

            titleText.text = "Social Duel";
            subText.text = $"{npc.Name} approaches the bridge.";

            ShowModal(panelRoot);

            awaitingInput = true;
            showFrame = Time.frameCount;

            panelRoot.transform.localScale = Vector3.zero;
            StartCoroutine(PopIn());
        }

        private void Update()
        {
            if (!awaitingInput)
                return;

            if (Time.frameCount == showFrame)
                return;

            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                awaitingInput = false;

                HideModal(panelRoot);

                GameEventBus.Publish(
                    new SocialDuelConfirmedEvent(
                        player,
                        npc,
                        Time.frameCount));

                player = null;
                npc = null;
            }
        }

        private IEnumerator PopIn()
        {
            float duration = 0.5f;
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