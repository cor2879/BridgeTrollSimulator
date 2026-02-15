using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class DialogSystem : MonoBehaviour
    {
        [SerializeField]
        private DialogSequence defaultEncounterDialog;

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
            GameEventBus.Publish(
                new DialogStartedEvent(
                    defaultEncounterDialog,
                    evt.Initiator,
                    evt.Target,
                    Time.frameCount));
        }
    }
}