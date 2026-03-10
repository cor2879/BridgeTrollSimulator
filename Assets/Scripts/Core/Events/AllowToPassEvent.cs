using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public class AllowToPassEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Initiator => (IReceiver)Sender;
        public IReceiver Target { get; }

        public AllowToPassEvent(
            IReceiver initiator,
            IReceiver target,
            int frame)
            : base(initiator, frame)
        {
            this.Target = target;
        } 

        public override string ToString()
        {
            return $"{nameof(AllowToPassEvent)}: Initiator: {Initiator?.SourceName} Target: {Target?.SourceName} @ Frame {Frame}";
        }
        
    }
}