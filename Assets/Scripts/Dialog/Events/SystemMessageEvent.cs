using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Events
{
    public class SystemMessageEvent : GameEvent
    {
        public string Message { get; }

        public SystemMessageEvent(
            IEventSource sender,
            string message,
            int frame)
            : base(sender, frame)
        {
            Message = message;
        }

        public override string ToString()
        {
            return $"{nameof(SystemMessageEvent)}::Message:\"{Message}\" @ Frame {Frame}";
        }
    }
}