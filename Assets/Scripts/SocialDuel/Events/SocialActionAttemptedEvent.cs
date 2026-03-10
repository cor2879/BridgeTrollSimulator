using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events
{
    public class SocialActionAttemptedEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Attacker => (IReceiver)Sender;
        public IReceiver Target { get; }
        public string ActionName { get; }
        public bool Success { get; }

        public SocialActionAttemptedEvent(
            IReceiver attacker,
            IReceiver target,
            string actionName,
            bool success,
            int frame)
            : base(attacker, frame)
        {
            Target = target;
            ActionName = actionName;
            Success = success;
        }

        public override string ToString()
        {
            return $"{nameof(SocialActionAttemptedEvent)}::Attacker:{Attacker.SourceName}" +
                $"::Target:{Target.SourceName}::Action:{ActionName}::Success:{Success} @ Frame {Frame}";
        }
    }
}