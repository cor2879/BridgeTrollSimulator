using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Rewards;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Rewards.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel
{
    public class SocialDuelResolutionData : IRewardData
    {
        public EntityController Player { get; }
        public EntityController Npc { get; }
        public SocialDuelOutcome Outcome { get; }
        public RewardBundle Reward { get; }

        public SocialDuelResolutionData(
            EntityController player,
            EntityController npc,
            SocialDuelOutcome outcome,
            RewardBundle reward)
        {
            Player = player;
            Npc = npc;
            Outcome = outcome;
            Reward = reward;     
        }

        public override string ToString()
        {
            return $"{nameof(SocialDuelResolutionData)}::Player:{Player.SourceName}" +
                $"::Npc:{Npc.SourceName}::Outcome:{Outcome}::Reward:{Reward}";
        }
    }
}