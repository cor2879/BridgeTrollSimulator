using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class LevelUpScreenUI : MonoBehaviour, IEventSource
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text pointsText;
        [SerializeField] private Transform statsContainer;
        [SerializeField] private StatRowUI statRowPrefab;

        private EntityController player;
        private List<StatRowUI> statRows = new();

        public string SourceName => nameof(LevelUpScreenUI);
        public GameSystemType SystemType => GameSystemType.UI;

/*
        private void Start()
        {
            panel.SetActive(false);
        } */

        public void Show(EntityController entity)
        {
            GameEventBus.Publish(
                new PauseRequestEvent(this, Time.frameCount));
            player = entity;
            panel.SetActive(true);

            BuildStatRows();
            RefreshUI();
        }

        private void BuildStatRows()
        {
            foreach (Transform child in statsContainer)
                Destroy(child.gameObject);

            statRows.Clear();

            foreach (StatType statType in System.Enum.GetValues(typeof(StatType)))
            {
                var row = Instantiate(statRowPrefab, statsContainer);
                row.Initialize(statType, player);
                row.OnStatChanged += RefreshUI;
                statRows.Add(row);
            }
        }

        private void RefreshUI()
        {
            pointsText.text = $"Unspent Points: {player.ProgressionPoints}";

            foreach (var row in statRows)
            {
                row.Refresh();
            }
        }

        public void Close()
        {
            panel.SetActive(false);
            GameStateSystem.Instance.SetState(GameState.World);
        }
    }
}