using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class LeaveEvent : GameEvent
    {
        public LeaveEvent(
            IEventSource sender,
            int frame)
            : base(sender, frame)
        {}

        public override string ToString()
        {
            return $"{nameof(LeaveEvent)}::Sender:{Sender.SourceName} @ Frame {Frame}";
        }
    }
}