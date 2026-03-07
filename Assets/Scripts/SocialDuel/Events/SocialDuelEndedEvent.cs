using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events
{
    public class SocialDuelEndedEvent : GameEvent
    {
        public SocialDuelOutcome Outcome { get; }

        public SocialDuelEndedEvent(
            IEventSource sender,
            SocialDuelOutcome outcome,
            int frame)
            : base(sender, frame)
        {
            Outcome = outcome;
        }
    }
}