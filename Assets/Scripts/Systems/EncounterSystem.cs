using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class EncounterSystem 
        : MonoBehaviour
    {
#region Initialization

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

#endregion

#region Event Handling

        private void OnEncounter(EntityEncounterEvent evt)
        {
            var initiator = (EntityController)evt.Initiator;
            var target = (EntityController)evt.Target;

            if (initiator.CurrentControlMode == ControlMode.Encounter)
            {
                return;
            }

            initiator.HandleEncounter(target);
            target.HandleEncounter(initiator);

            Debug.Log($"Encounter started between {evt.Initiator.SourceName} and {evt.Target.SourceName}");
        }

        private void OnDialogEnded(DialogEndedEvent evt)
        {
            GameDatabase.Instance.Player.ResetControlMode();
        }

#endregion
    }
}