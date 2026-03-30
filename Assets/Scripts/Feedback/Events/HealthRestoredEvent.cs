using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Feedback.Events
{
    public class HealthRestoredEvent : GameEvent
    {
        public int Amount { get; }
        public bool IsCrit { get; }

        public HealthRestoredEvent(
            IEventSource sender,
            int amount,
            bool isCrit = false)
            : base(sender, Time.frameCount)
        {
            this.Amount = amount;
            this.IsCrit = isCrit;
        } 

        public override string ToString()
        {
            return $"{nameof(HealthRestoredEvent)}::Initiator:{Sender.SourceName}" +
                $"::Amount:{Amount}::IsCrit:{IsCrit} @ Frame {Frame}";
        }
    }
}