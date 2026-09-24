using System.Collections.Generic;

using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class AbilityTreeBootstrap : MonoBehaviour
    {
        [SerializeField] private List<AbilityTree> trees;

        private void OnEnable()
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

                AbilityTreeService.Register(tree);
                Debug.Log($"Registered AbilityTree {tree.Id}");
            }
        }
    }
}