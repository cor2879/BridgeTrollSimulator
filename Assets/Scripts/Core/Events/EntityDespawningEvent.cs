using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class EntityDespawningEvent : GameEvent
    {
        public EntityController Entity => (EntityController)Sender;

        public EntityDespawningEvent(
            EntityController entity,
            int frameCount)
            : base(entity, frameCount)
        {}
    }
}