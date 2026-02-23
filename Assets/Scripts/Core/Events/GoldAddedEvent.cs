using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class GoldAddedEvent : GameEvent
    {
        public EntityController Initiator => (EntityController)Sender;
        public EntityController Target { get; }
        public int Amount { get; }

        public GoldAddedEvent(
            EntityController initiator,
            int amount,
            int frame)
            : base(initiator, frame)
        {
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{nameof(GoldAddedEvent)}: {Initiator?.name} Amount: {Amount} @ Frame {Frame}";
        }
    }
}