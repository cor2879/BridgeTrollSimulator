using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Events
{
    public class DemandResolvedEvent : GameEvent, ITargetedEvent
    {
        public IResolver Resolver => (IResolver)Sender;
        public IReceiver Target { get; }
        public IDemand Demand { get; }

        public DemandResolvedEvent(
            IResolver resolver,
            IReceiver target,
            IDemand demand,
            int frame)
            : base(resolver, frame)
        {
            this.Target = target;
            this.Demand = demand;
        } 

        public override string ToString()
        {
            return $"{nameof(DemandResolvedEvent)}::Resolver:{Resolver.SourceName}" +
                $"::Target:{Target.SourceName}::Demand:{Demand} @ Frame {Frame}";
        }
    }
}