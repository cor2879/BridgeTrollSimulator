using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class CombatLogEvent : GameEvent
    {
        public string Message { get; }

        public CombatLogEvent(
            string message,
            IEventSource sender,
            int frame)
            : base (sender, frame)
        {
            Message = message;
        }
    }
}