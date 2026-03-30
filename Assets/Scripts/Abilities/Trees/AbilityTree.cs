using UnityEngine;
using System.Collections.Generic;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees
{
    [CreateAssetMenu(menuName = "BridgeTroll/Abilities/AbilityTree")]
    public class AbilityTree : ScriptableObject
    {
        [SerializeField]
        private string id;

        [SerializeField]
        private List<AbilityTier> tiers = new();  

        public string Id => id;

        public IReadOnlyList<AbilityTier> Tiers => tiers;

        public void AddTier()
        {
            tiers.Add(new AbilityTier());
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            for (int i = 0; i < tiers.Count; i++)
            {
                var tier = tiers[i];

                if (tier == null || tier.nodes == null)
                    continue;

                // ✅ Enforce tierId
                tier.tierId = i;

                foreach (var node in tier.nodes)
                {
                    if (node == null)
                        continue;

                    // ✅ Enforce node tier
                    if (node.tier != i)
                    {
                        Debug.LogWarning(
                            $"Fixing node '{node.id}' tier from {node.tier} → {i}",
                            this);

                        node.tier = i;
                    }

                    // 🔥 Enforce treeId
                    if (string.IsNullOrWhiteSpace(node.treeId) || node.treeId != id)
                    {
                        Debug.LogWarning(
                            $"Fixing node '{node.id}' treeId from '{node.treeId}' → '{id}'",
                            this);

                        node.treeId = id;
                    }

                    // 🧠 Optional: enforce node id
                    if (string.IsNullOrWhiteSpace(node.id))
                    {
                        Debug.LogWarning(
                            $"Node in tree '{id}' is missing an id",
                            this);
                    }

                    // 🧠 Optional: enforce ability reference
                    if (node.ability == null)
                    {
                        Debug.LogWarning(
                            $"Node '{node.id}' has no Ability assigned",
                            this);
                    }
                }
            }
        }
        #endif
    }
}