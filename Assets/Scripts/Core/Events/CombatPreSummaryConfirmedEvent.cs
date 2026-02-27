using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class CombatPreSummaryConfirmedEvent : GameEvent, ITargetedEvent
    {
        public EntityController Initiator => (EntityController)Sender;
        public EntityController Target { get; }
        
        public CombatPreSummaryConfirmedEvent(
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