using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events
{
    public class SocialDuelStartedEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Initiator => (IReceiver)Sender;
        public IReceiver Target { get; }

        public SocialDuelStartedEvent(
            IReceiver initiator,
            IReceiver target,
            int frame)
            : base(initiator, frame)
        {
            Target = target;
        }

        public override string ToString()
        {
            return $"{nameof(SocialDuelStartedEvent)}::Initiator:{Initiator.SourceName}::Target:{Target.SourceName} @ Frame {Frame}";
        }
    }
}