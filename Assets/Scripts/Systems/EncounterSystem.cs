using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class EncounterSystem 
        : MonoBehaviour
    {
        private void OnEnable()
        {
            GameEventBus.Subscribe<EntityEncounterEvent>(OnEncounter);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<EntityEncounterEvent>(OnEncounter);
        }

        private void OnEncounter(EntityEncounterEvent evt)
        {
            if (evt.Initiator.CurrentControlMode == ControlMode.Encounter)
            {
                return;
            }

            evt.Initiator.HandleEncounter(evt.Target);
            evt.Target.HandleEncounter(evt.Initiator);

            Debug.Log($"Encounter started between {evt.Initiator.name} and {evt.Target.name}");
        }
    }
}