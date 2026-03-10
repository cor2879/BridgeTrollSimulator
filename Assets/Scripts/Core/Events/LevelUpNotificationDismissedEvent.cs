using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class LevelUpNotificationDismissedEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Target { get; }

        public LevelUpNotificationDismissedEvent(
            IEventSource sender,
            IReceiver target,
            int frame)
            : base(sender, frame)
        {
            Target = target;
        }

        public override string ToString()
        {
            return $"{nameof(LevelUpNotificationDismissedEvent)}::Sender:{Sender.SourceName}" +
                $"::Target:{Target.SourceName} @ Frame {Frame}";
        }
    }
}