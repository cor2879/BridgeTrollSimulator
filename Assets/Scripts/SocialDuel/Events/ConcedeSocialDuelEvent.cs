using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events
{
    public class ConcedeSocialDuelEvent : ConcedeEvent
    {
        public ConcedeSocialDuelEvent(
            IReceiver initiator,
            IReceiver target,
            int frame)
            : base(initiator, target, frame)
        {
        }

        public override string ToString()
        {
            return $"{nameof(ConcedeSocialDuelEvent)}::Initiator:{Initiator.SourceName}" +
                $"::Target:{Target.SourceName} @ Frame {Frame}";
        }
    }
}