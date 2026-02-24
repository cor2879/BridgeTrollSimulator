using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class TollDemandedEvent : GameEvent, ITargetedEvent
    {
        public EntityController Initiator => (EntityController)Sender;
        public EntityController Target { get; }
        public int Amount { get; }

        public TollDemandedEvent(
            EntityController initiator,
            EntityController target,
            int amount,
            int frame)
            : base(initiator, frame)
        {
            this.Target = target;
            this.Amount = amount;
        } 

        public override string ToString()
        {
            return $"{nameof(TollDemandedEvent)}: Initiator: {Initiator?.Name} Target: {Target?.Name} Amount: {Amount} @ Frame {Frame}";
        }
        
    }
}