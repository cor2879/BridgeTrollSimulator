using System.Collections.Generic;
using System.Linq;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Rewards;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Rewards.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Combat
{
    public class CombatResolutionData : IRewardData
    {
        public CombatOutcome Outcome { get; }
        public List<EntityController> PlayerSide { get; }
        public List<EntityController> EnemySide { get; }
        public CombatFaction WinningFaction { get; }
        public RewardBundle Reward { get; }

        public CombatResolutionData(
            CombatOutcome outcome,
            IEnumerable<EntityController> playerSide,
            IEnumerable<EntityController> enemySide,
            CombatFaction winningFaction,
            RewardBundle rewards)
        {
            Outcome = outcome;
            PlayerSide = playerSide.ToList();
            EnemySide = enemySide.ToList();
            WinningFaction = winningFaction;
            Reward = rewards;
        }

        public override string ToString()
        {
            return $"{nameof(CombatResolutionData)}::Outcome:{Outcome}::WinningFaction:{WinningFaction}" +
                $"::{Reward}";
        }
    }
}