using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Feedback.Events
{
    public class ResolveDamageTakenEvent : GameEvent
    {
        public int Amount { get; }
        public bool IsCrit { get; }

        public ResolveDamageTakenEvent(
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
            return $"{nameof(ResolveDamageTakenEvent)}::Initiator:{Sender.SourceName}" +
                $"::Amount:{Amount}::IsCrit:{IsCrit} @ Frame {Frame}";
        }
    }
}