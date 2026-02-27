using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class CombatResolutionUI : MonoBehaviour, IEventSource
    {
        [Header("Core")]
        [SerializeField]
        private GameObject panelRoot;

        [Header("Outcome")]
        [SerializeField]
        private TMP_Text outcomeText;

        [Header("Portraits")]
        [SerializeField]
        private Transform playerSideContainer;
        [SerializeField]
        private Transform enemySideContainer;
        [SerializeField]
        private GameObject portraitPrefab;

        [Header("Rewards")]
        [SerializeField]
        private TMP_Text xpText;
        [SerializeField]
        private TMP_Text goldText;
        [SerializeField]
        private TMP_Text fameText;
        [SerializeField]
        private TMP_Text respectText;
        [SerializeField]
        private TMP_Text reputationText;

        private bool awaitingInput;
        private CombatResolutionData resolutionData;

        public string SourceName => "CombatResolutionUI";
        public GameSystemType SystemType => GameSystemType.UI;

        private void Awake()
        {
            panelRoot.SetActive(false);
        }

        public void Show(CombatResolutionData data)
        {
            ClearSides();

            PopulateSide(
                playerSideContainer, 
                data.PlayerSide, 
                data.WinningFaction == CombatFaction.Player);
            PopulateSide(
                enemySideContainer, 
                data.EnemySide,
                data.WinningFaction == CombatFaction.Enemy);

            outcomeText.text = GetOutcomeText(data.Outcome);

            xpText.text = $"{data.Experience} XP";
            goldText.text = $"{data.GoldReward} Gold";
            fameText.text = $"{data.FameDelta} Fame";
            respectText.text = $"{data.RespectDelta} Respect";
            reputationText.text = $"{data.ReputationDelta} Reputation";

            panelRoot.SetActive(true);
            GameEventBus.Publish(
                new PauseRequestEvent(this, Time.frameCount));
            awaitingInput = true;
            resolutionData = data;
        }

        private void PopulateSide(Transform container, IEnumerable<EntityController> entities, bool isWinner)
        {
            foreach (var entity in entities)
            {
                var portrait = Instantiate(portraitPrefab, container);
                var image = portrait.transform
                    .Find("Portrait")
                    .GetComponent<Image>();
                var nameText = portrait.transform
                    .Find("NameText")
                    .GetComponent<TMP_Text>();
                image.sprite = isWinner ? entity.VictorySprite :
                    entity.CurrentHealth > 0 ? entity.DefeatedSprite : entity.DeadSprite;
                image.preserveAspect = true;
                nameText.text = entity.Name;
                
            }
        }

        private void ClearSides()
        {
            foreach (Transform child in playerSideContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in enemySideContainer)
            {
                Destroy(child.gameObject);
            }
        }

        private string GetOutcomeText(CombatOutcome outcome)
        {
            return outcome switch
            {
                CombatOutcome.PlayerVictory_EnemyKilled => "Enemy Slain",
                CombatOutcome.PlayerVictory_EnemyAlive => "Enemy Defeated",
                CombatOutcome.PlayerDefeated => "Defeat",
                CombatOutcome.PlayerKilled => "You Have Fallen",
                _ => "Combat Ended"
            };
        }

        private void Update()
        {
            if (!awaitingInput) return;

            if (Input.anyKeyDown)
            {
                awaitingInput = false;
                panelRoot.SetActive(false);

                GameEventBus.Publish(
                    new CombatResolutionCompletedEvent(this, resolutionData, Time.frameCount));
                resolutionData = null;
            }
        }
    }
}