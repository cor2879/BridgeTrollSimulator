using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class SocialDuelPreSummaryUI : ModalUIBase
    {
        [Header("Root")]
        [SerializeField] private GameObject panelRoot;

        [Header("Portraits")]
        [SerializeField] private Image playerPortrait;
        [SerializeField] private Image npcPortrait;

        [Header("Names")]
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private TMP_Text npcNameText;

        [Header("Resolve")]
        [SerializeField] private TMP_Text playerResolveText;
        [SerializeField] private TMP_Text npcResolveText;

        [Header("UI Text")]
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private TMP_Text continueText;

        private bool awaitingInput;
        private EntityController Player { get; set; }
        private EntityController Npc { get; set; }

        public override string SourceName => nameof(SocialDuelPreSummaryUI);
        public override GameSystemType SystemType => GameSystemType.UI;
        public override bool IsBlockingUI => true;

        private void Awake()
        {
            panelRoot.SetActive(false);
        }

        public void Show(
            EntityController player,
            EntityController npc)
        {
            Player = player;
            Npc = npc;

            playerPortrait.sprite =
                Player.SocialDuelIntroSprite ?? Player.SpriteRenderer.sprite;

            npcPortrait.sprite =
                Npc.SocialDuelIntroSprite ?? Npc.SpriteRenderer.sprite;

            playerPortrait.preserveAspect = true;
            npcPortrait.preserveAspect = true;

            playerNameText.text = Player.Name;
            npcNameText.text = Npc.Name;

            playerResolveText.text = $"Resolve: {Player.Resolve}";
            npcResolveText.text = $"Resolve: {Npc.Resolve}";

            resultText.text =
                $"{Npc.Name} resists your demand.";

            continueText.text = "Press Any Key to Begin";

            ShowModal(panelRoot);

            panelRoot.transform.localScale = Vector3.zero;
            StartCoroutine(PopIn());
        }

        private void Update()
        {
            if (!awaitingInput)
                return;

            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                awaitingInput = false;

                HideModal(panelRoot);

                GameEventBus.Publish(
                    new SocialDuelPreSummaryConfirmedEvent(
                        Player,
                        Npc,
                        Time.frameCount));
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
            awaitingInput = true;
        }
    }
}