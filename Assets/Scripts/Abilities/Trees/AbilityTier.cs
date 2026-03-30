using UnityEngine;
using System.Collections.Generic;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Requirements;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees
{
    [System.Serializable]
    public class AbilityTier
    {
        public int tierId;
        public string name;

        public List<AbilityNode> nodes;
        
        [SerializeReference, RequirementDropdown]
        public List<AbilityRequirement> requirements = new();
    }
}