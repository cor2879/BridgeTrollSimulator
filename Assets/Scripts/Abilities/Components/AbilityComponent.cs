using UnityEngine;
using System.Collections.Generic;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Components
{
    public class AbilityComponent : MonoBehaviour
    {
        [SerializeField]
        private List<EntityAbilityTree> trees = new();

        [SerializeField, ReadOnly]
        private HashSet<Ability> activeAbilities = new();

        private Dictionary<string, EntityAbilityTree> _lookup;

        public IReadOnlyCollection<Ability> ActiveAbilities => activeAbilities;

        private void Awake()
        {
            BuildLookup();
        }

        private void BuildLookup()
        {
            _lookup = new Dictionary<string, EntityAbilityTree>();

            foreach (var tree in trees)
            {
                if (tree == null || string.IsNullOrEmpty(tree.TreeId))
                    continue;

                _lookup[tree.TreeId] = tree;
            }
        }

        public EntityAbilityTree GetTree(string treeId)
        {
            if (_lookup == null)
            {
                BuildLookup();
            }

            _lookup.TryGetValue(treeId, out var tree);
            return tree;
        }

        public void AddTree(EntityAbilityTree tree)
        {
            if (tree == null || string.IsNullOrEmpty(tree.TreeId))
                return;

            trees.Add(tree);

            if (_lookup != null)
            {
                _lookup[tree.TreeId] = tree;
            }
        }

        public bool HasAbility(AbilityNode node)
        {
            var tree = GetTree(node.treeId);
            return tree != null && tree.HasAbility(node);
        }

        public void AddActiveAbility(Ability ability)
        {
            activeAbilities.Add(ability);
        }
    }
}