using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class ResumeRequestEvent : GameEvent
    {
        public ResumeRequestEvent(IEventSource sender, int frame) 
            : base(sender, frame) { }
    }
}