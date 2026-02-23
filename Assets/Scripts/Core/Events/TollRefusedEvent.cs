using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class TollRefusedEvent : GameEvent, ITargetedEvent
    {
        public EntityController Initiator => (EntityController)Sender;
        public EntityController Target { get; }
        public int Amount { get; }

        public TollRefusedEvent(
            EntityController initiator,
            EntityController target,
            int amount,
            int frame)
            : base(initiator, frame)
        {
            this.Target = target;
            this.Amount = amount;
        } 
    }
}