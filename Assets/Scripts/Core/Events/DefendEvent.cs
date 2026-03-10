using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class DefendEvent : GameEvent
    {
        public DefendEvent(
            IEventSource initiator,
            int frame)
            : base(initiator, frame)
        { }

        public override string ToString()
        {
            return $"{nameof(DefendEvent)}::Initiator:{Sender.SourceName} @ Frame {Frame}";
        } 
    }
}