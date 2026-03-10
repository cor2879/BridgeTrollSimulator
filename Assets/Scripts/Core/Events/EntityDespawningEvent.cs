using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class EntityDespawningEvent : GameEvent
    {
        public EntityDespawningEvent(
            IEventSource entity,
            int frameCount)
            : base(entity, frameCount)
        {}

        public override string ToString()
        {
            return $"{nameof(EntityDespawningEvent)}::Entity:{Sender.SourceName} @ Frame {Frame}";
        }
    }
}