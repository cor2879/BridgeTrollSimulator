using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class AllowToPassEvent : GameEvent, ITargetedEvent
    {
        public EntityController Initiator => (EntityController)Sender;
        public EntityController Target { get; }

        public AllowToPassEvent(
            EntityController initiator,
            EntityController target,
            int frame)
            : base(initiator, frame)
        {
            this.Target = target;
        } 

        public override string ToString()
        {
            return $"{nameof(AllowToPassEvent)}: Initiator: {Initiator?.Name} Target: {Target?.Name} @ Frame {Frame}";
        }
        
    }
}