using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Rewards;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Rewards.Events
{
    public class RewardEvent : GameEvent, ITargetedEvent
    {
        public IReceiver Target { get; }
        public RewardBundle Reward { get; }

        public RewardEvent(
            IEventSource sender,
            IReceiver target,
            RewardBundle reward,
            int frame)
            : base(sender, frame)
        {
            Target = target;
            Reward = reward;
        }

        public override string ToString()
        {
            return $"{nameof(RewardEvent)}::Sender:{Sender.SourceName}::Target:{Target.SourceName}::" +
                $"Reward:{Reward} @ Frame {Frame}";
        }
    }
}