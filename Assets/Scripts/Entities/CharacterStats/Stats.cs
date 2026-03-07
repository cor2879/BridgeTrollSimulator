using System;
using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats
{
    [Serializable]
    public class Stats
    {
        [Header("Primary Stats (D20 Model)")]
        [SerializeField] private int strength = 10;
        [SerializeField] private int dexterity = 10;
        [SerializeField] private int constitution = 10;
        [SerializeField] private int charisma = 10;
        [SerializeField] private int intelligence = 10;
        [SerializeField] private int wisdom = 10;
        [SerializeField] private int luck = 10;

        #region Properties

        public int Strength => strength;
        public int Dexterity => dexterity;
        public int Constitution => constitution;
        public int Charisma => charisma;
        public int Intelligence => intelligence;
        public int Wisdom => wisdom;
        public int Luck => luck;

        #endregion

        #region Generic Accessors

        public int Get(StatType type)
        {
            return type switch
            {
                StatType.Strength => strength,
                StatType.Dexterity => dexterity,
                StatType.Constitution => constitution,
                StatType.Charisma => charisma,
                StatType.Intelligence => intelligence,
                StatType.Wisdom => wisdom,
                StatType.Luck => luck,
                _ => 0
            };
        }

        public int GetModifier(StatType type)
        {
            return CalculateModifier(Get(type));
        }

        public static int CalculateModifier(int statValue)
        {
            return Mathf.FloorToInt((statValue - 10) / 2f);
        }

        public void Set(StatType type, int value)
        {
            value = Mathf.Max(1, value); // Optional safety floor

            switch (type)
            {
                case StatType.Strength: strength = value; break;
                case StatType.Dexterity: dexterity = value; break;
                case StatType.Constitution: constitution = value; break;
                case StatType.Charisma: charisma = value; break;
                case StatType.Intelligence: intelligence = value; break;
                case StatType.Wisdom: wisdom = value; break;
                case StatType.Luck: luck = value; break;
            }
        }

        public void Add(StatType type, int amount)
        {
            Set(type, Get(type) + amount);
        }

        #endregion

        #region Utility

        public void Add(Stats other)
        {
            strength += other.strength;
            dexterity += other.dexterity;
            constitution += other.constitution;
            charisma += other.charisma;
            intelligence += other.intelligence;
            wisdom += other.wisdom;
            luck += other.luck;
        }

        public Stats Clone()
        {
            return new Stats
            {
                strength = strength,
                dexterity = dexterity,
                constitution = constitution,
                charisma = charisma,
                intelligence = intelligence,
                wisdom = wisdom,
                luck = luck
            };
        }

        #endregion
    }
}