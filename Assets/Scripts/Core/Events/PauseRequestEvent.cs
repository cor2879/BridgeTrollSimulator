using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class PauseRequestEvent : GameEvent
    {
        public PauseRequestEvent(IEventSource sender, int frame) 
            : base(sender, frame) { }
    }
}