using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Audio;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class InventorySystem : MonoBehaviour
    {
        private void OnEnable()
        {
            GameEventBus.Subscribe<GoldAddedEvent>(OnGoldAdded);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<GoldAddedEvent>(OnGoldAdded);
        }

        private void OnGoldAdded(GoldAddedEvent evt)
        {
            Debug.Log($"Gold Added {evt.Amount}");

            var initiator = evt.Sender as EntityController;
            var newAmount = initiator.Gold;
            int previous = newAmount - evt.Amount;

            GameEventBus.Publish(
                new GoldFeedbackEvent(
                    initiator,
                    previous,
                    newAmount,
                    evt.Amount,
                    Time.frameCount));
        }
    }
}