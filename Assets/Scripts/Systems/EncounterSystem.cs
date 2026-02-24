using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameState;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class EncounterSystem 
        : MonoBehaviour
    {
        private void OnEnable()
        {
            GameEventBus.Subscribe<EntityEncounterEvent>(OnEncounter);
            GameEventBus.Subscribe<DialogEndedEvent>(OnDialogEnded);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<EntityEncounterEvent>(OnEncounter);
            GameEventBus.Unsubscribe<DialogEndedEvent>(OnDialogEnded);
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

        private void OnDialogEnded(DialogEndedEvent evt)
        {
            GameDatabase.Instance.Player.ResetControlMode();
        }
    }
}