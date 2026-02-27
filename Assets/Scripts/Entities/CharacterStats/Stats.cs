using System;
using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats
{
    [Serializable]
    public class Stats
    {
        [Header("Primary Stats")]
        [SerializeField] private int strength = 5;
        [SerializeField] private int dexterity = 5;
        [SerializeField] private int constitution = 5;
        [SerializeField] private int charisma = 5;
        [SerializeField] private int intelligence = 5;
        [SerializeField] private int luck = 5;

        #region Properties (Read-Only External Access)

        public int Strength => strength;
        public int Dexterity => dexterity;
        public int Constitution => constitution;
        public int Charisma => charisma;
        public int Intelligence => intelligence;
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
                StatType.Luck => luck,
                _ => 0
            };
        }

        public void Set(StatType type, int value)
        {
            switch (type)
            {
                case StatType.Strength: strength = value; break;
                case StatType.Dexterity: dexterity = value; break;
                case StatType.Constitution: constitution = value; break;
                case StatType.Charisma: charisma = value; break;
                case StatType.Intelligence: intelligence = value; break;
                case StatType.Luck: luck = value; break;
            }
        }

        public void Add(StatType type, int amount)
        {
            switch (type)
            {
                case StatType.Strength: strength += amount; break;
                case StatType.Dexterity: dexterity += amount; break;
                case StatType.Constitution: constitution += amount; break;
                case StatType.Charisma: charisma += amount; break;
                case StatType.Intelligence: intelligence += amount; break;
                case StatType.Luck: luck += amount; break;
            }
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
            luck += other.luck;
        }

        public Stats Clone()
        {
            return new Stats
            {
                strength = this.strength,
                dexterity = this.dexterity,
                constitution = this.constitution,
                charisma = this.charisma,
                intelligence = this.intelligence,
                luck = this.luck
            };
        }

        #endregion
    }
}