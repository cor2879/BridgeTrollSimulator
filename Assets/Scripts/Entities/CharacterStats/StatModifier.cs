using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats
{
    [System.Serializable]
    public class StatModifier
    {
        public StatType baseStat;
        public DerivedStatType derivedStat;

        public int flatAmount;
        [Range(0f, 1f)]
        public float percentageAmount;
        public int duration;

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (baseStat != StatType.None)
            {
                derivedStat = DerivedStatType.None;
            }
            else if (derivedStat != DerivedStatType.None)
            {
                baseStat = StatType.None;
            }
        }
#endif
    }
}