using System.Collections;
using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Spawning
{
    public class NpcSpawnSystem : MonoBehaviour
    {
        [SerializeField]
        private GameObject npcPrefab;
        [SerializeField]
        private Transform spawnPoint;
        [SerializeField]
        private Vector2 respawnRange = new Vector2(2f, 5f);

        [SerializeField, ReadOnly]
        private int activeNpcCount = 0;

        private void OnEnable()
        {
            GameEventBus.Subscribe<EntityDiedEvent>(OnEntityDied);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<EntityDiedEvent>(OnEntityDied);
        }

        private void Start()
        {
            SpawnNpc();
        }

        private void OnEntityDied(EntityDiedEvent evt)
        {
            if (evt.Entity is NpcController)
            {
                activeNpcCount--;
                StartCoroutine(RespawnRoutine());
            }
        }

        private IEnumerator RespawnRoutine()
        {
            var respawnDelay = Random.Range(respawnRange.x, respawnRange.y);
            yield return new WaitForSeconds(respawnDelay);
            SpawnNpc();
        }

        private void SpawnNpc()
        {
            Instantiate(npcPrefab, transform.position, Quaternion.identity);
            activeNpcCount++;
        }
    }
}