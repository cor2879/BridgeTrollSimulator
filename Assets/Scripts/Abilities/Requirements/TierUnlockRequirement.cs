using UnityEngine;
using System.Linq;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Components;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Requirements
{
    [System.Serializable]
    public class TierUnlockRequirement : AbilityRequirement
    {
        [SerializeField]
        private int requiredCount;

        [SerializeField]
        private int tierOffset = -1; // -1 = previous tier

        public override bool IsMet(IActor actor, AbilityNode node)
        {
            if (actor == null || node == null)
                return false;

            var component = actor as Component;
            if (component == null)
                return false;

            var abilities = component.GetComponent<AbilityComponent>();
            if (abilities == null)
                return false;

            // 🔥 Use node.treeId (your new addition)
            var tree = abilities.GetTree(node.treeId);
            if (tree == null)
                return false;

            int targetTier = node.tier + tierOffset;

            if (targetTier < 0)
                return false;

            var tierAbilities = tree.GetTier(targetTier);

            if (tierAbilities == null)
                return false;

            return tierAbilities.Count() >= requiredCount;
        }
    }
}