using System.Collections.Generic;

using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    [DefaultExecutionOrder(-10000)]
    public class AbilityTreeBootstrap : MonoBehaviour
    {
        [SerializeField] private List<AbilityTree> trees;

        private void Awake()
        {
            RegisterTrees();
        }

        private void OnEnable()
        {
            RegisterTrees();
        }

        private void RegisterTrees()
        {
            if (trees == null)
            {
                return;
            }

            foreach (var tree in trees)
            {
                if (tree == null)
                {
                    Debug.LogWarning("AbilityTreeBootstrap contains a null AbilityTree reference.");
                    continue;
                }

                if (AbilityTreeService.GetTree(tree.Id) == tree)
                {
                    continue;
                }

                AbilityTreeService.Register(tree);
                Debug.Log($"Registered AbilityTree {tree.Id}");
            }
        }
    }
}