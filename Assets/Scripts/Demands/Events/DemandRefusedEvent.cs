using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Events
{
    public class DemandRefusedEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Initiator => (IReceiver)Sender;
        public IReceiver Target { get; }
        public IDemand Demand { get; }

        public DemandRefusedEvent(
            IReceiver initiator,
            IReceiver target,
            IDemand demand,
            int frame)
            : base(initiator, frame)
        {
            this.Target = target;
            this.Demand = demand;
        } 

        public override string ToString()
        {
            return $"{nameof(DemandRefusedEvent)}::Initiator:{Initiator.SourceName}" +
                $"::Target:{Target.SourceName}::Demand:{Demand} @ Frame {Frame}";
        }
    }
}