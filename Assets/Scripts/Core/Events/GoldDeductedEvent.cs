using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class GoldDeductedEvent : GameEvent
    {
        public EntityController Initiator => (EntityController)Sender;
        public EntityController Target { get; }
        public int Amount { get; }

        public GoldDeductedEvent(
            EntityController initiator,
            EntityController target,
            int amount,
            int frame)
            : base(initiator, frame)
        {
            Target = target;
            Amount = amount;

            Initiator.AddGold(Target.DeductGold(Amount));
        }

        public override string ToString()
        {
            return $"{nameof(GoldDeductedEvent)}: {Initiator?.name} vs {Target?.name} Amount: {Amount} @ Frame {Frame}";
        }
    }
}