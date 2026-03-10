using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class TollPaidEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Initiator => (IReceiver)Sender;
        public IReceiver Target { get; }
        public int Amount { get; }

        public TollPaidEvent(
            IReceiver initiator,
            IReceiver target,
            int amount,
            int frame)
            : base(initiator, frame)
        {
            this.Target = target;
            this.Amount = amount;
        } 

        public override string ToString()
        {
            return $"{nameof(TollPaidEvent)}::Initiator:{Initiator.SourceName}" +
                $"::Target:{Target.SourceName}::Amount:{Amount} @ Frame {Frame}";
        }
    }
}