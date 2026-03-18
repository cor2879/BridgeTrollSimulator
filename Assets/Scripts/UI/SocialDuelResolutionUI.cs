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
    public class SocialDuelResolutionUI : ModalUIBase
    {
        [Header("Root")]
        [SerializeField] private GameObject panelRoot;

        [Header("Outcome")]
        [SerializeField] private TMP_Text outcomeText;

        [Header("Portraits")]
        [SerializeField] private Transform playerContainer;
        [SerializeField] private Transform npcContainer;
        [SerializeField] private GameObject portraitPrefab;

        [Header("Rewards")]
        [SerializeField] private TMP_Text xpText;
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private TMP_Text fameText;
        [SerializeField] private TMP_Text respectText;
        [SerializeField] private TMP_Text reputationText;

        private bool awaitingInput;
        private SocialDuelResolutionData resolutionData;

        public override string SourceName => nameof(SocialDuelResolutionUI);
        public override GameSystemType SystemType => GameSystemType.UI;
        public override bool IsBlockingUI => true;

        private void Awake()
        {
            panelRoot.SetActive(false);
        }

        public void Show(SocialDuelResolutionData data)
        {
            Debug.Log($"{nameof(SocialDuelResolutionUI)}::Show @ Frame {Time.frameCount}");
            gameObject.SetActive(true);
            ClearSides();

            PopulateSide(playerContainer, data.Player, data.Outcome == SocialDuelOutcome.PlayerVictory);
            PopulateSide(npcContainer, data.Npc, data.Outcome == SocialDuelOutcome.NpcVictory);

            outcomeText.text = GetOutcomeText(data.Outcome);

            xpText.text = $"{data.Reward.Experience} XP";
            goldText.text = $"{data.Reward.Gold} Gold";
            fameText.text = $"{data.Reward.FameDelta} Fame";
            respectText.text = $"{data.Reward.RespectDelta} Respect";
            reputationText.text = $"{data.Reward.ReputationDelta} Reputation";

            ShowModal(panelRoot);

            panelRoot.transform.localScale = Vector3.zero;
            StartCoroutine(PopIn());

            awaitingInput = true;
            resolutionData = data;
        }

        private void PopulateSide(Transform container, EntityController entity, bool isWinner)
        {
            var portrait = Instantiate(portraitPrefab, container);

            var image = portrait.transform
                .Find("Portrait")
                .GetComponent<Image>();

            var nameText = portrait.transform
                .Find("NameText")
                .GetComponent<TMP_Text>();

            image.sprite = isWinner
                ? entity.VictorySprite
                : entity.DefeatedSprite ?? entity.SpriteRenderer.sprite;

            image.preserveAspect = true;

            nameText.text = entity.Name;
        }

        private void ClearSides()
        {
            foreach (Transform child in playerContainer)
                Destroy(child.gameObject);

            foreach (Transform child in npcContainer)
                Destroy(child.gameObject);
        }

        private string GetOutcomeText(SocialDuelOutcome outcome)
        {
            return outcome switch
            {
                SocialDuelOutcome.PlayerVictory => "You Win the Argument",
                SocialDuelOutcome.NpcVictory => "Your Argument Fails",
                SocialDuelOutcome.Escalation => "The Situation Escalates",
                _ => "Social Duel Concluded"
            };
        }

        private void Update()
        {
            if (!awaitingInput)
                return;

            if (Input.anyKeyDown)
            {
                awaitingInput = false;

                HideModal(panelRoot);

                if (resolutionData != null)
                {
                    GameEventBus.Publish(
                        new SocialDuelResolutionCompletedEvent(
                            this,
                            resolutionData,
                            Time.frameCount));
                    resolutionData = null;
                }
            }
        }

        #region Coroutines

        private IEnumerator PopIn()
        {
            float duration = 0.35f;
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

        #endregion
    }
}