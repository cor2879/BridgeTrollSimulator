using System;
using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core
{
    public class NpcSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject npcPrefab;

        [SerializeField]
        private float spawnInterval = 5f;

        private void Start()
        {
            InvokeRepeating(nameof(SpawnNpc), 2f, spawnInterval);
        }

        private void SpawnNpc()
        {
            Instantiate(npcPrefab, transform.position, Quaternion.identity);
        }
    }
}