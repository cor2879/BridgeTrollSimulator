using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class DialogSystem : MonoBehaviour
    {
        [SerializeField]
        private DialogNode defaultEncounterNode;

        [SerializeField]
        private DialogUIController dialogPanel;

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
                    defaultEncounterNode,
                    evt.Initiator,
                    evt.Target,
                    Time.frameCount));
        }
    }
}