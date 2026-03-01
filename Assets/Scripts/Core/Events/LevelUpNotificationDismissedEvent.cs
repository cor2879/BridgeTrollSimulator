using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class LevelUpNotificationDismissedEvent : GameEvent, ITargetedEvent
    {
        public EntityController Target { get; }

        public LevelUpNotificationDismissedEvent(
            IEventSource sender,
            EntityController target,
            int frame)
            : base(sender, frame)
        {
            Target = target;
        }

        public override string ToString()
        {
            return $"{nameof(LevelUpNotificationDismissedEvent)}::Target:{Target.Name}:: @ Frame {Frame}";
        }
    }
}