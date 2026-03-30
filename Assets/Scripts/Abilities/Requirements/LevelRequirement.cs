using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Requirements
{
    [System.Serializable]
    public class LevelRequirement : AbilityRequirement
    {
        [SerializeField]
        private int minimumLevel;

        public override bool IsMet(IActor actor, AbilityNode node)
        {
            return actor.Level >= minimumLevel;
        }
    }
}