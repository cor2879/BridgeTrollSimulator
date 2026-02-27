using System.Collections;
using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Extensions;

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
        [SerializeField, ReadOnly]
        private bool respawnQueued = false;

        [SerializeField]
        private int maxNpcCount = 5;

        private void OnEnable()
        {
            GameEventBus.Subscribe<EntityDiedEvent>(OnEntityDied);
            GameEventBus.Subscribe<AllowToPassEvent>(OnAllowedToPass);            
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<EntityDiedEvent>(OnEntityDied);
            GameEventBus.Unsubscribe<AllowToPassEvent>(OnAllowedToPass);
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

                if (!respawnQueued)
                {
                    respawnQueued = true;
                    StartCoroutine(RespawnRoutine());
                }
            }
        }

        private void OnAllowedToPass(AllowToPassEvent evt)
        {
            if (evt.Target is NpcController)
            {
                activeNpcCount--;

                if (!respawnQueued)
                {
                    respawnQueued = true;
                    StartCoroutine(RespawnRoutine());
                }
            }
        }

        private IEnumerator RespawnRoutine()
        {
            var respawnDelay = Random.Range(respawnRange.x, respawnRange.y);

            yield return CoroutineExtensions.WaitForSecondsRespectingPause(respawnDelay);

            yield return CoroutineExtensions.WaitUntilGameplayActive();

            if (activeNpcCount < maxNpcCount)
            {
                SpawnNpc();
            }

            respawnQueued = false;
        }

        private void SpawnNpc()
        {
            Instantiate(npcPrefab, transform.position, Quaternion.identity);
            activeNpcCount++;
        }
    }
}