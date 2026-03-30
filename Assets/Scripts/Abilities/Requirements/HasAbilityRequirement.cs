
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Requirements
{
    [System.Serializable]
    public class HasAbilityRequirement : AbilityRequirement
    {
        [SerializeField]
        private AbilityNode requiredAbility;

        public AbilityNode RequiredNode => requiredAbility;

        public override bool IsMet(IActor actor, AbilityNode node)
        {
            if (actor?.AbilityComponent == null || requiredAbility == null)
                return false;

            var tree = actor.AbilityComponent.GetTree(requiredAbility.treeId);

            if (tree == null)
                return false;

            // 🔥 Uses your O(1) lookup
            return tree.HasAbility(requiredAbility);
        }
    }
}