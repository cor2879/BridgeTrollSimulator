namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Rewards
{
    public class RewardBundle
    {
        public static readonly RewardBundle Empty = new RewardBundle(0, 0, 0, 0, 0, 0);

        public int Experience { get; }
        public int Gold { get; }
        public int FameDelta { get; }
        public int RespectDelta { get; }
        public int ReputationDelta { get; }
        public int Resolve { get; }

        public RewardBundle(
            int experience,
            int gold,
            int fameDelta,
            int respectDelta,
            int reputationDelta,
            int resolve)
        {
            Experience = experience;
            Gold = gold;
            FameDelta = fameDelta;
            RespectDelta = respectDelta;
            ReputationDelta = reputationDelta;
            Resolve = resolve;
        }

        public override string ToString()
        {
            return $"{nameof(RewardBundle)}::Experience:{Experience}::Gold:{Gold}" +
                $"::FameDelta:{FameDelta}::RespectDelta:{RespectDelta}" +
                $"::ReputationDelta:{ReputationDelta}::Resolve:{Resolve}";
        }
    }
}