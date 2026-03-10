using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class RefusePassageEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Initiator => (IReceiver)Sender;
        public IReceiver Target { get; }

        public RefusePassageEvent(
            IReceiver initiator,
            IReceiver target,
            int frame)
            : base(initiator, frame)
        {
            this.Target = target;
        } 

        public override string ToString()
        {
            return $"{nameof(RefusePassageEvent)}::Initiator:{Initiator.SourceName}" +
                $"::Target:{Target.SourceName} @ Frame {Frame}";
        }        
    }
}