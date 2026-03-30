using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class LevelUpScreenUI : ModalUIBase
    {
        [Header("Children")]
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text pointsText;
        [SerializeField] private GameObject statsContainer;
        [SerializeField] private GameObject abilitiesContainer;
        [SerializeField] private TMP_Text abilityPointsText;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;
        [Header("Prefabs")]
        [SerializeField] private AbilityRowUI abilityRowPrefab;
        [SerializeField] private StatRowUI statRowPrefab;

        private enum PageState
        {
            Stats,
            Abilities
        };

        private PageState currentPage;
        private EntityController player;
        private List<StatRowUI> statRows = new();
        private List<AbilityRowUI> abilityRows = new();
        private Dictionary<StatType, int> pendingAllocations = new();
        private List<AbilityNode> abilityChoices = new();
        private List<AbilityNode> selectedAbilities = new();
        private int availableStatPoints;
        private int availableAbilityPoints;

        public override string SourceName => nameof(LevelUpScreenUI);
        public override GameSystemType SystemType => GameSystemType.UI;
        public override bool IsBlockingUI => true;

        private void Start()
        {
            panel.SetActive(false);
        } 

        public void Show(EntityController entity)
        {
            player = entity;

            availableStatPoints = player.StatProgressionPoints;
            availableAbilityPoints = player.AbilityProgressionPoints;
            pendingAllocations.Clear();

            foreach (StatType stat in System.Enum.GetValues(typeof(StatType)))
            {
                pendingAllocations[stat] = 0;
            }

            abilityChoices = AbilityTreeService.GetLevelUpChoices(entity, "combat", 3);
            selectedAbilities.Clear();

            ShowModal(panel);

            if (availableStatPoints > 0)
            {
                ShowStatsPage();
            }
            else if (availableAbilityPoints > 0)
            {
                ShowAbilityPage();
            }

            BuildStatRows();
            BuildAbilityRows();
            RefreshAbilitySelection();
            RefreshUI();
            AddButtons();
        }

        private void ShowStatsPage()
        {
            currentPage = PageState.Stats;
            statsContainer.SetActive(true);
            abilitiesContainer.SetActive(false);
        }

        private void ShowAbilityPage()
        {
            currentPage = PageState.Abilities;
            statsContainer.SetActive(false);
            abilitiesContainer.SetActive(true);

            if (abilityPointsText != null)
            {
                abilityPointsText.text =
                    $"Ability Points: {availableAbilityPoints - selectedAbilities.Count}";
            }
        }

        private void OnNext()
        {
            if (currentPage == PageState.Stats)
            {
                ShowAbilityPage();
                RefreshButtons();
            }
        }

        private void AddButtons()
        {
            confirmButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
            nextButton.onClick.RemoveAllListeners();

            confirmButton.onClick.AddListener(OnConfirm);
            cancelButton.onClick.AddListener(OnCancel);
            nextButton.onClick.AddListener(OnNext);
        }

        private void OnConfirm()
        {
            if (availableStatPoints > 0)
            {
                return;
            }

            if (availableAbilityPoints > 0 &&
                selectedAbilities.Count < availableAbilityPoints)
            {
                return;
            }

            GameEventBus.Publish(
                new LevelUpConfirmedEvent(
                    this,
                    player,
                    new Dictionary<StatType, int>(pendingAllocations),
                    new List<AbilityNode>(selectedAbilities),
                    Time.frameCount));

            Close();
        }

        private void OnCancel()
        {

        }

        private void OnAbilitySelected(AbilityNode node)
        {
            Debug.Log($"Ability {node.id} selected");
            if (selectedAbilities.Contains(node))
            {
                selectedAbilities.Remove(node);
            }
            else
            {
                if (selectedAbilities.Count >= availableAbilityPoints)
                    return;

                selectedAbilities.Add(node);
            }

            RefreshAbilitySelection();
        }

        private void RefreshAbilitySelection()
        {
            foreach (var row in abilityRows)
            {
                bool selected = selectedAbilities.Contains(row.Node);
                row.SetSelected(selected);
            }

            if (abilityPointsText != null)
            {
                abilityPointsText.text =
                    $"Ability Points: {availableAbilityPoints - selectedAbilities.Count}";
            }

            confirmButton.interactable =
                availableStatPoints == 0 &&
                selectedAbilities.Count == availableAbilityPoints;

            RefreshButtons();
        }

        private void BuildStatRows()
        {
            foreach (Transform child in statsContainer.transform)
                Destroy(child.gameObject);

            statRows.Clear();

            foreach (StatType statType in System.Enum.GetValues(typeof(StatType)))
            {
                if (statType == StatType.None)
                {
                    continue;
                }

                var row = Instantiate(statRowPrefab, statsContainer.transform);
                row.Initialize(
                    statType,
                    TryIncreasePoint,
                    TryDecreasePoint,
                    GetPendingPoints,
                    GetBaseStatValue,
                    GetAvailablePoints);

                row.OnStatChanged += RefreshUI;
                statRows.Add(row);
            }
        }

        private void BuildAbilityRows()
        {
            foreach (Transform child in abilitiesContainer.transform)
                Destroy(child.gameObject);

            abilityRows.Clear();

            if (abilityChoices == null)
                return;

            Debug.Log($"{nameof(BuildAbilityRows)}:: Ability Choices:{abilityChoices.Count}");

            foreach (var node in abilityChoices)
            {
                if (node?.ability == null)
                    continue;

                var row = Instantiate(abilityRowPrefab, abilitiesContainer.transform);

                row.Initialize(node, OnAbilitySelected);

                abilityRows.Add(row);
            }

            RefreshAbilitySelection();
        }

        private void RefreshUI()
        {
            pointsText.text = $"Unspent Points: {availableStatPoints}";

            foreach (var row in statRows)
            {
                row.Refresh();
            }

            RefreshButtons();
        }

        private void RefreshButtons()
        {
            bool canGoNext =
                currentPage == PageState.Stats &&
                availableStatPoints == 0 &&
                availableAbilityPoints > 0;

            bool canConfirm =
                currentPage == PageState.Abilities &&
                availableStatPoints == 0 &&
                selectedAbilities.Count == availableAbilityPoints;

            // Visibility
            nextButton.gameObject.SetActive(canGoNext);
            confirmButton.gameObject.SetActive(!canGoNext && canConfirm);

            // Interactability
            nextButton.interactable = canGoNext;
            confirmButton.interactable = canConfirm;
        }

        private bool TryIncreasePoint(StatType stat)
        {
            if (availableStatPoints <= 0)
            {
                return false;
            }

            pendingAllocations[stat]++;
            availableStatPoints--;

            return true;
        }

        private bool TryDecreasePoint(StatType stat)
        {
            if (pendingAllocations[stat] <= 0)
            {
                return false;
            }

            pendingAllocations[stat]--;
            availableStatPoints++;

            return true;
        }

        private int GetPendingPoints(StatType stat)
        {
            return pendingAllocations[stat];
        }

        private int GetBaseStatValue(StatType stat)
        {
            return player.BaseStats.Get(stat);
        }

        private int GetAvailablePoints()
        {
            return availableStatPoints;
        }

        public void Close()
        {
            HideModal(panel);
            GameStateSystem.Instance.SetState(GameState.World);
        }
    }
}