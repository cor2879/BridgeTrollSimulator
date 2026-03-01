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
        [SerializeField] private Button subtractButton;

        private System.Func<StatType, bool> tryIncrease;
        private System.Func<StatType, bool> tryDecrease;
        private System.Func<StatType, int> getPending;
        private System.Func<StatType, int> getBaseValue;
        private System.Func<int> getAvailablePoints;

        private StatType statType;

        public event Action OnStatChanged;

        public void Initialize(
            StatType type,
            System.Func<StatType, bool> increaseCallback,
            System.Func<StatType, bool> decreaseCallback,
            System.Func<StatType, int> getPendingCallback,
            System.Func<StatType, int> getBaseValueCallback,
            System.Func<int> getAvailablePointsCallback)
        {
            statType = type;
            tryIncrease = increaseCallback;
            tryDecrease = decreaseCallback;
            getPending = getPendingCallback;
            getBaseValue = getBaseValueCallback;
            getAvailablePoints = getAvailablePointsCallback;

            statNameText.text = statType.ToString();
            addButton.onClick.RemoveAllListeners();
            subtractButton.onClick.RemoveAllListeners();
            addButton.onClick.AddListener(OnIncrease);
            subtractButton.onClick.AddListener(OnDecrease);

            Refresh();
        }

        public void Refresh()
        {
            int baseValue = getBaseValue(statType);
            int pending = getPending(statType);

            statValueText.text = pending > 0 ?
                $"{baseValue} (+{pending})" :
                baseValue.ToString();
            
            if (pending > 0)
            {
                statValueText.text = $"{baseValue} <color=#6FFF6F>(+{pending})</color>";
            }
            else
            {
                statValueText.text = baseValue.ToString();
            }

            subtractButton.interactable = pending > 0;
            addButton.interactable = getAvailablePoints() > 0;
        }

        private void OnIncrease()
        {
            if (tryIncrease(statType))
            {
                OnStatChanged?.Invoke();
            }
        }

        private void OnDecrease()
        {
            if (tryDecrease(statType))
            {
                OnStatChanged?.Invoke();
            }
        }
    }    
}