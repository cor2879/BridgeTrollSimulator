using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events
{
    public class SocialDuelResolutionCompletedEvent : GameEvent
    {
        public SocialDuelResolutionData Data { get; }

        public SocialDuelResolutionCompletedEvent(
            IEventSource subject,
            SocialDuelResolutionData data,
            int frame)
            : base(subject, frame)
        { 
            Data = data;
        }         

        public override string ToString()
        {
            return $"{nameof(SocialDuelResolutionCompletedEvent)}::Initiator:{Sender.SourceName}" +
                $"::Data:{Data} @ Frame {Frame}";
        }
    }
}