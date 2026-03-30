using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Requirements
{
    [System.Serializable]
    public class StatRequirement : AbilityRequirement
    {
        [SerializeField]
        private StatType stat;
        [SerializeField]
        private int minimumValue;

        public override bool IsMet(IActor actor, AbilityNode node)
        {
            return actor.BaseStats.Get(stat) >= minimumValue;
        }
    }
}