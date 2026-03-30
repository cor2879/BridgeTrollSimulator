using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Spawning
{
    [RequireComponent(typeof(Collider2D))]
    public class DespawnZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("DespawnZone Encounter");
            
            if (!other.TryGetComponent<EntityController>(out var entity))
            {
                return;
            }

            if (entity.CurrentControlMode == ControlMode.Passing ||
                entity.CurrentControlMode == ControlMode.Leaving)
            {
                entity.BeginDespawn();   
            }
        }
    }
}