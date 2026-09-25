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
            tiers = new List<HashSet<Ability>>();
        }

        private void EnsureTiers()
        {
            tiers ??= new List<HashSet<Ability>>();
        }

        private void EnsureTier(int tier)
        {
            EnsureTiers();

            while (tiers.Count <= tier)
            {
                tiers.Add(new HashSet<Ability>());
            }

            tiers[tier] ??= new HashSet<Ability>();
        }

        public void AddAbility(int tier, Ability ability)
        {
            if (tier < 0)
            {
                Debug.LogError($"Invalid tier {tier}");
                return;
            }

            if (ability == null)
            {
                return;
            }

            EnsureTier(tier);
            tiers[tier].Add(ability);
        }

        public IReadOnlyCollection<Ability> GetTier(int tier)
        {
            EnsureTiers();

            if (tier < 0 || tier >= tiers.Count)
            {
                Debug.Log($"Invalid Ability Tier {tier}");
                return null;
            }

            tiers[tier] ??= new HashSet<Ability>();
            return tiers[tier];
        }

        public bool HasAbility(AbilityNode node)
        {
            if (node == null || node.ability == null)
                return false;

            return HasAbility(node.tier, node.ability);
        }

        public bool HasAbility(int tier, Ability ability)
        {
            if (ability == null)
            {
                return false;
            }

            EnsureTiers();

            if (tier < 0 || tier >= tiers.Count)
            {
                return false;
            }

            tiers[tier] ??= new HashSet<Ability>();
            return tiers[tier].Contains(ability);
        }
    }
}
