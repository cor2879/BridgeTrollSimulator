using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Feedback.Events
{
    public class FloatingTextEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Target { get; }
        public int Amount { get; }
        public bool IsCrit { get; }
        public Color Color { get; }

        public FloatingTextEvent(
            IEventSource sender,
            IReceiver target,
            int amount,
            Color color,
            bool isCrit = false)
            : base(sender, Time.frameCount)
        {
            Target = target;
            Amount = amount;
            Color = color;
            IsCrit = isCrit;
        }

        public override string ToString()
        {
            return $"{nameof(FloatingTextEvent)}::Initiator:{Sender.SourceName}" +
                $"::Target:{Target.SourceName}::Amount:{Amount}::IsCrit:{IsCrit}" +
                $"::Color:{Color} @ Frame {Frame}";
        }
    }
}