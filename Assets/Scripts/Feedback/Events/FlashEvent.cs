using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Feedback.Events
{
    public class FlashEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Target { get; }
        public Color Color { get; }
        public float Duration { get; }

        public FlashEvent(
            IEventSource sender,
            IReceiver target,
            Color color,
            float duration)
            : base(sender, Time.frameCount)
        {
            this.Target = target;
            this.Color = color;
            this.Duration = duration;
        } 

        public override string ToString()
        {
            return $"{nameof(FlashEvent)}::Initiator:{Sender.SourceName}::Target:{Target.SourceName}" +
                $"::Color:{Color}::Duration:{Duration} @ Frame {Frame}";
        }
    }
}