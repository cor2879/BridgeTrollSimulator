using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Feedback.Events
{
    public class ResolveBrokenEvent : GameEvent
    {
        public ResolveBrokenEvent(
            IEventSource sender)
            : base(sender, Time.frameCount)
        {
        }

        public override string ToString()
        {
            return $"{nameof(ResolveBrokenEvent)}::Sender:{Sender.SourceName} @ Frame {Frame}";
        }
    }
}