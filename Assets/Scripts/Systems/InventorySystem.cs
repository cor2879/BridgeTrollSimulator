using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Audio;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;

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
            GameEventBus.Publish(
                new SoundEffectEvent(
                    evt.Initiator,
                    AudioSystem.Library.coins,
                    Time.frameCount));

            var newAmount = evt.Initiator.Gold;
            int previous = newAmount - evt.Amount;

            GameEventBus.Publish(
                new GoldFeedbackEvent(
                    evt.Initiator,
                    previous,
                    newAmount,
                    evt.Amount,
                    Time.frameCount));
        }
    }
}