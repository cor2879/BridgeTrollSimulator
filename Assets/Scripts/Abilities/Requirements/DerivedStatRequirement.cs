using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Requirements
{
    [System.Serializable]
    public class DerivedStatRequirement : AbilityRequirement
    {
        [SerializeField]
        private DerivedStatType stat;
        [SerializeField]
        private int minimumValue;

        public override bool IsMet(IActor actor, AbilityNode node)
        {
            return stat switch
            {
                DerivedStatType.Attack => actor.Attack >= minimumValue,
                DerivedStatType.Defense => actor.Defense >= minimumValue,
                _ => true
            };
        }
    }
}