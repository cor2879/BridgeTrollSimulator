using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class GoldAddedEvent : GameEvent
    {
        public int Amount { get; }

        public GoldAddedEvent(
            IEventSource initiator,
            int amount,
            int frame)
            : base(initiator, frame)
        {
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{nameof(GoldAddedEvent)}::Sender:{Sender.SourceName} Amount: {Amount} @ Frame {Frame}";
        }
    }
}