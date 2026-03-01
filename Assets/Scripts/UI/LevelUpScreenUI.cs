using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class LevelUpScreenUI : ModalUIBase, IEventSource
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text pointsText;
        [SerializeField] private Transform statsContainer;
        [SerializeField] private StatRowUI statRowPrefab;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        private EntityController player;
        private List<StatRowUI> statRows = new();
        private Dictionary<StatType, int> pendingAllocations = new();
        private int availablePoints;

        public override string SourceName => nameof(LevelUpScreenUI);
        public override GameSystemType SystemType => GameSystemType.UI;

        private void Start()
        {
            panel.SetActive(false);
        } 

        public void Show(EntityController entity)
        {
            player = entity;

            availablePoints = player.ProgressionPoints;
            pendingAllocations.Clear();

            foreach (StatType stat in System.Enum.GetValues(typeof(StatType)))
            {
                pendingAllocations[stat] = 0;
            }

            ShowModal(panel);
            BuildStatRows();
            RefreshUI();
            AddButtons();
        }

        private void AddButtons()
        {
            confirmButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(OnConfirm);
            cancelButton.onClick.AddListener(OnCancel);
        }

        private void OnConfirm()
        {
            if (availablePoints > 0)
            {
                return;
            }

            foreach (var kvp in pendingAllocations)
            {
                for (int i = 0; i < kvp.Value; i++)
                {
                    player.BaseStats.Add(kvp.Key, 1);
                }
            }

            player.ConsumeProgressionPoints(pendingAllocations.Values.Sum());

            Close();
        }

        private void OnCancel()
        {

        }

        private void BuildStatRows()
        {
            foreach (Transform child in statsContainer)
                Destroy(child.gameObject);

            statRows.Clear();

            foreach (StatType statType in System.Enum.GetValues(typeof(StatType)))
            {
                var row = Instantiate(statRowPrefab, statsContainer);
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

        private void RefreshUI()
        {
            pointsText.text = $"Unspent Points: {availablePoints}";

            confirmButton.interactable = availablePoints == 0;

            foreach (var row in statRows)
            {
                row.Refresh();
            }
        }

        private bool TryIncreasePoint(StatType stat)
        {
            if (availablePoints <= 0)
            {
                return false;
            }

            pendingAllocations[stat]++;
            availablePoints--;

            return true;
        }

        private bool TryDecreasePoint(StatType stat)
        {
            if (pendingAllocations[stat] <= 0)
            {
                return false;
            }

            pendingAllocations[stat]--;
            availablePoints++;

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
            return availablePoints;
        }

        public void Close()
        {
            HideModal(panel);
            GameStateSystem.Instance.SetState(GameState.World);
            GameEventBus.Publish(
                new LevelUpConfirmedEvent(this, player, Time.frameCount));
        }
    }
}