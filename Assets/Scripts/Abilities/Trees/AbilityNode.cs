using UnityEngine;
using System.Collections.Generic;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Requirements;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees
{
    [System.Serializable]
    public class AbilityNode
    {
        public string id;
        public int tier;
        public string treeId;
        public Ability ability;

        [SerializeReference, RequirementDropdown]
        public List<AbilityRequirement> requirements = new();
    }
}