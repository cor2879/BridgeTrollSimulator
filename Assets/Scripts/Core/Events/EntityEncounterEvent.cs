using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class EntityEncounterEvent : GameEvent, ITargetedEvent
    {
        public EntityController Initiator { get; }
        public EntityController Target { get; }

        public EntityEncounterEvent(EntityController initiator, EntityController target, int frame)
            : base(initiator, frame)
        {
            Initiator = initiator;
            Target = target;
        }

        public override string ToString()
        {
            return $"Encounter: {Initiator.name} + {Target.name}";
        }
    }
}