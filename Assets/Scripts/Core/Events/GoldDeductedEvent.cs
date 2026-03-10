using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class GoldDeductedEvent : GameEvent
    {
        public int Amount { get; }

        public GoldDeductedEvent(
            IEventSource initiator,
            int amount,
            int frame)
            : base(initiator, frame)
        {
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{nameof(GoldDeductedEvent)}::Sender:{Sender.SourceName} Amount: {Amount} @ Frame {Frame}";
        }
    }
}