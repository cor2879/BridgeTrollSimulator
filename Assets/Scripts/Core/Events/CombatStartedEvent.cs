using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class CombatStartedEvent : GameEvent
    {
        public EntityController Initiator => (EntityController)Sender;
        public EntityController Target { get; }
        
        public CombatStartedEvent(
            EntityController initiator,
            EntityController target,
            int frame)
            : base(initiator, frame)
        {
            Target = target;
        }

        public override string ToString()
        {
            return $"{nameof(CombatStartedEvent)}: {Initiator?.name} vs {Target?.name} @ Frame {Frame}";
        }
    }
}