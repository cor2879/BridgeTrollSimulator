using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class StatRowUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text statNameText;
        [SerializeField] private TMP_Text statValueText;
        [SerializeField] private Button addButton;

        private StatType statType;
        private EntityController player;

        public event Action OnStatChanged;

        public void Initialize(StatType type, EntityController entity)
        {
            statType = type;
            player = entity;

            statNameText.text = statType.ToString();
            addButton.onClick.AddListener(AddPoint);

            Refresh();
        }

        public void Refresh()
        {
            int value = player.BaseStats.Get(statType);
            statValueText.text = value.ToString();

            addButton.interactable = player.ProgressionPoints > 0;
        }

        private void AddPoint()
        {
            if (player.TrySpendPoint(statType))
            {
                OnStatChanged?.Invoke();
            }
        }
    }    
}