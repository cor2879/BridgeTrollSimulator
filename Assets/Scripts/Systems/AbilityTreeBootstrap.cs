using System.Collections.Generic;

using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class AbilityTreeBootstrap : MonoBehaviour
    {
        [SerializeField] private List<AbilityTree> trees;

        private void Awake()
        {
            foreach (var tree in trees)
            {
                AbilityTreeService.Register(tree);
                Debug.Log($"Registered AbilityTree {tree.Id}");
            }
        }
    }
}