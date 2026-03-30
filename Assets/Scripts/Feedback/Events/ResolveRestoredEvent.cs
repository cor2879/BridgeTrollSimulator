using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Feedback.Events
{
    public class ResolveRestoredEvent : GameEvent
    {
        public int Amount { get; }
        public bool IsCrit { get; }

        public ResolveRestoredEvent(
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
            return $"{nameof(ResolveRestoredEvent)}::Initiator:{Sender.SourceName}" +
                $"::Amount:{Amount}::IsCrit:{IsCrit} @ Frame {Frame}";
        }
    }
}