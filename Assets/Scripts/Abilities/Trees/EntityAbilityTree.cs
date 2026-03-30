using UnityEngine;
using System.Collections.Generic;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Requirements;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees
{
    [System.Serializable]
    public class EntityAbilityTree
    {
        [SerializeField]
        private string abilityTreeId;
        [SerializeField]
        private List<HashSet<Ability>> tiers = new();

        public string TreeId => abilityTreeId;

        public EntityAbilityTree(string treeId)
        {
            abilityTreeId = treeId;
        }

        public void AddAbility(int tier, Ability ability)
        {
            if (tier < 0)
            {
                Debug.LogError($"Invalid tier {tier}");
                return;
            }

            if (tier >= tiers.Count)
            {
                for (var i = tiers.Count; i <= tier; i++)
                {
                    tiers.Add(new HashSet<Ability>());
                }
            }

            tiers[tier].Add(ability);
        }

        public IReadOnlyCollection<Ability> GetTier(int tier)
        {
            if (tier < 0 || tier >= tiers.Count)
            {
                Debug.Log($"Invalid Ability Tier {tier}");
                return null;
            }

            return tiers[tier];
        }

        public bool HasAbility(AbilityNode node)
        {
            if (node == null || node.ability == null)
                return false;

            int tier = node.tier;

            if (tier < 0 || tier >= tiers.Count)
                return false;

            return tiers[node.tier].Contains(node.ability);
        }

        public bool HasAbility(int tier, Ability ability)
        {
            if (ability == null)
            {
                return false;
            }

            if (tier < 0 || tier >= tiers.Count)
            {
                return false;
            }

            return tiers[tier].Contains(ability);
        }
    }
}