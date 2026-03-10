using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class EntityDiedEvent : GameEvent
    {
        public EntityDiedEvent(
            IEventSource entity,
            int frameCount)
            : base(entity, frameCount)
        {}

        public override string ToString()
        {
            return $"{nameof(EntityDiedEvent)}::Entity:{Sender.SourceName} @ Frame {Frame}";
        }
    }
}