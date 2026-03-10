using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class TollRefusedEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Initiator => (IReceiver)Sender;
        public IReceiver Target { get; }
        public int Amount { get; }

        public TollRefusedEvent(
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
            return $"{nameof(TollRefusedEvent)}::Initiator:{Initiator.SourceName}" +
                $"::Target:{Target.SourceName}::Amount:{Amount} @ Frame {Frame}";
        }
    }
}