using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Requirements;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Extensions;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees
{
    public static class AbilityTreeService
    {
        private static readonly Dictionary<string, AbilityTree> _trees = new();

        public static void Register(AbilityTree tree)
        {
            Debug.Log($"Ability Tree {tree.Id} Registering.");

            if (tree == null || string.IsNullOrEmpty(tree.Id))
                return;

            _trees[tree.Id] = tree;

            Debug.Log($"Ability Tree {tree.Id} Registered.");
        }

        public static AbilityTree GetTree(string id)
        {
            return _trees.TryGetValue(id, out var tree) ? tree : null;
        }

        public static bool CanUnlock(AbilityNode node, IActor actor)
        {
            if (node == null || actor == null)
                return false;

            // Already unlocked?
            var tree = actor.AbilityComponent.GetTree(node.treeId);

            if (tree != null && tree.HasAbility(node.tier, node.ability))
                return false;

            // Check requirements
            if (node.requirements != null)
            {
                foreach (var req in node.requirements)
                {
                    if (!req.IsMet(actor, node))
                        return false;
                }
            }

            return true;
        }

        public static void Unlock(AbilityNode node, IActor actor)
        {
            var tree = actor.AbilityComponent.GetTree(node.treeId);

            if (tree == null)
            {
                tree = new EntityAbilityTree(node.treeId);
                actor.AbilityComponent.AddTree(tree);
            }

            tree.AddAbility(node.tier, node.ability);
        }

        public static IEnumerable<AbilityNode> GetUnlockableNodes(
            IActor actor,
            string treeId)
        {
            var abilityTree = GetTree(treeId);

            if (abilityTree == null || actor?.AbilityComponent == null)
                return Enumerable.Empty<AbilityNode>();

            var result = new List<AbilityNode>();

            foreach (var tier in abilityTree.Tiers)
            {
                if (tier?.nodes == null)
                    continue;

                foreach (var node in tier.nodes)
                {
                    if (CanUnlock(node, actor))
                    {
                        result.Add(node);
                    }
                }
            }

            return result;
        }

        public static List<AbilityNode> GetUnlockableNodesForActor(IActor actor, string treeId)
        {
            var tree = GetTree(treeId);

            if (tree == null || actor?.AbilityComponent == null)
                return new List<AbilityNode>();

            var result = new List<AbilityNode>();

            foreach (var tier in tree.Tiers)
            {
                if (tier?.nodes == null)
                    continue;

                foreach (var node in tier.nodes)
                {
                    if (CanUnlock(node, actor))
                    {
                        result.Add(node);
                    }
                }
            }

            return result;
        }

        public static List<AbilityNode> GetLevelUpChoices(
            IActor actor,
            string treeId,
            int count)
        {
            var result = new List<AbilityNode>();

            var tree = GetTree(treeId);
            
            if (tree == null)
            {
                Debug.LogError($"AbilityTreeService: Tree '{treeId}' not found!");
                return result;
            }

            if (actor?.AbilityComponent == null)
            {
                Debug.LogError($"AbilityTreeService: Actor '{actor}' has no AbilityComponent!");
                return result;
            }

            var instance = actor.AbilityComponent.GetTree(treeId);
            
            if (instance == null)
            {
                instance = new EntityAbilityTree(treeId);
                actor.AbilityComponent.AddTree(instance);
            }

            Debug.Log($"{nameof(GetLevelUpChoices)}::Global Tree Id: {tree.Id} :: " +
                $"Instance tree Id: {instance.TreeId}");

            int highestTier = GetHighestUnlockedTier(actor, tree);

            // 🔹 STEP 1 — Gather candidates
            var candidates = new List<AbilityNode>();

            for (int i = 0; i < tree.Tiers.Count; i++)
            {
                var tier = tree.Tiers[i];
                if (tier?.nodes == null) continue;

                foreach (var node in tier.nodes)
                {
                    Debug.Log(
                        $"Node: {node.id} | " +
                        $"CanUnlock: {CanUnlock(node, actor)} | " +
                        $"HasAbility: {instance?.HasAbility(node)}");

                    if (node == null || node.ability == null)
                        continue;

                    if (!CanUnlock(node, actor))
                        continue;

                    if (instance != null && instance.HasAbility(node))
                        continue;

                    candidates.Add(node);
                }
            }

            Debug.Log($"Candidates: {candidates.Count} | HighestTier: {highestTier}");

            if (candidates.Count == 0)
            {
                Debug.LogWarning("No ability candidates found!");

                return result;
            }

            // 🔹 STEP 2 — Partition
            var currentTier = candidates
                .Where(n => n.tier == highestTier)
                .ToList();

            var nextTier = candidates
                .Where(n => n.tier == highestTier + 1)
                .ToList();

            if (UnityEngine.Random.value < 0.05f)
            {
                var legendary = candidates
                    .Where(n => n.ability.Rarity == AbilityRarity.Legendary)
                    .ToList();

                if (legendary.Count() > 0)
                {
                    result.Add(legendary.GetRandom());
                }
            }

            // 🔹 STEP 3 — Build weighted pool
            var weightedPool = new List<WeightedNode>();

            AddWeighted(weightedPool, currentTier, 3f, actor);
            AddWeighted(weightedPool, nextTier, 1.5f, actor);

            // fallback
            if (weightedPool.Count == 0)
            {
                Debug.LogWarning($"Weighted pool empty. Candidates: {candidates.Count}," +
                    $" HighestTier: {highestTier}");

                AddWeighted(weightedPool, candidates, 1f, actor);
            }

            // 🔹 STEP 4 — Select unique results
            return WeightedPickUnique(weightedPool, count);
        }

        private static void AddWeighted(
            List<WeightedNode> pool,
            List<AbilityNode> nodes,
            float baseWeight,
            IActor actor)
        {
            foreach (var node in nodes)
            {
                float weight = baseWeight;

                weight *= GetNodeWeightModifier(node, actor);

                pool.Add(new WeightedNode
                {
                    Node = node,
                    Weight = weight
                });
            }
        }

        private static float GetNodeWeightModifier(AbilityNode node, IActor actor)
        {
            if (node.ability == null)
                return 1f;

            float weight = 1f;

            // 🔹 Rarity
            weight *= node.ability.Rarity switch
            {
                AbilityRarity.Common => 1.0f,
                AbilityRarity.Rare => 0.5f,
                AbilityRarity.Legendary => 0.2f,
                _ => 1f
            };

            // 🔹 Offensive bias
            if (node.ability.IsOffensive)
                weight *= 1.2f;

            // 🔹 Stamina efficiency
            if (node.ability.StaminaCost <= 2)
                weight *= 1.1f;

            // 🔥 Synergy
            var synergy = actor.GetPrimeBonus(node.ability);
            if (synergy > 0)
                weight *= (1f + synergy);

            return weight;
        }

        private static int GetHighestUnlockedTier(IActor actor, AbilityTree tree)
        {
            if (actor?.AbilityComponent == null || tree == null)
                return 0;

            int highest = 0;

            for (int i = 0; i < tree.Tiers.Count; i++)
            {
                var tier = tree.Tiers[i];

                if (tier == null)
                    continue;

                // 🔥 If no requirements → unlocked by default
                if (tier.requirements == null || tier.requirements.Count == 0)
                {
                    highest = i;
                    continue;
                }

                bool allMet = true;

                foreach (var req in tier.requirements)
                {
                    if (!req.IsMet(actor, null)) // tier-level context
                    {
                        allMet = false;
                        break;
                    }
                }

                if (allMet)
                {
                    highest = i;
                }
                else
                {
                    // 🔥 Stop here — tiers should be sequential
                    break;
                }
            }

            return highest;
        }

        private static List<AbilityNode> WeightedPickUnique(
            List<WeightedNode> pool,
            int count)
        {
            var result = new List<AbilityNode>();

            var workingPool = new List<WeightedNode>(pool);

            while (result.Count < count && workingPool.Count > 0)
            {
                float totalWeight = workingPool.Sum(n => n.Weight);
                float roll = UnityEngine.Random.value * totalWeight;

                float cumulative = 0f;

                for (int i = 0; i < workingPool.Count; i++)
                {
                    cumulative += workingPool[i].Weight;

                    if (roll <= cumulative)
                    {
                        var chosen = workingPool[i].Node;

                        result.Add(chosen);

                        // remove all duplicates of that node
                        workingPool.RemoveAll(n => n.Node == chosen);

                        break;
                    }
                }
            }

            return result;
        }

        private static IEnumerable<AbilityNode> Repeat(List<AbilityNode> source, int times)
        {
            for (int i = 0; i < times; i++)
            {
                foreach (var item in source)
                {
                    yield return item;
                }
            }
        }
    }
}