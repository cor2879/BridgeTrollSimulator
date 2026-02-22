using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class EntityDiedEvent : GameEvent
    {
        public EntityController Entity => (EntityController)Sender;

        public EntityDiedEvent(
            EntityController entity,
            int frameCount)
            : base(entity, frameCount)
        {}
    }
}