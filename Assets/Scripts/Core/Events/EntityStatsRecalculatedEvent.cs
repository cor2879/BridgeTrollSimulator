using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class EntityStatsRecalculatedEvent : GameEvent
    {
        public EntityStatsRecalculatedEvent(
            IEventSource sender,
            int frame)
            : base(sender, frame)
        {
        }

        public override string ToString()
        {
            return $"{nameof(EntityStatsRecalculatedEvent)}::Sender:{Sender.SourceName}:: @ Frame {Frame}";
        }
    }
}