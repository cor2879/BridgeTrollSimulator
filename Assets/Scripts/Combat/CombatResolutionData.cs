using System.Collections.Generic;
using System.Linq;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Combat
{
    public class CombatResolutionData
    {
        public CombatOutcome Outcome { get; }
        public List<EntityController> PlayerSide { get; }
        public List<EntityController> EnemySide { get; }
        public CombatFaction WinningFaction { get; }
        public int Experience { get; }
        public int FameDelta { get; }
        public int RespectDelta {get; }
        public int ReputationDelta { get; }
        public int GoldReward { get; }

        public CombatResolutionData(
            CombatOutcome outcome,
            IEnumerable<EntityController> playerSide,
            IEnumerable<EntityController> enemySide,
            CombatFaction winningFaction,
            int experience,
            int fameDelta,
            int respectDelta,
            int reputationDelta,
            int gold)
        {
            Outcome = outcome;
            PlayerSide = playerSide.ToList();
            EnemySide = enemySide.ToList();
            WinningFaction = winningFaction;
            Experience = experience;
            FameDelta = fameDelta;
            RespectDelta = respectDelta;
            ReputationDelta = reputationDelta;
            GoldReward = gold;
        }

        public override string ToString()
        {
            return $"{nameof(CombatResolutionData)}::Outcome:{Outcome}::WinningFaction:{WinningFaction}" +
            $"::Experience:{Experience}::Fame:{FameDelta}::Respect:{RespectDelta}::Reputation:{ReputationDelta}" +
            $"::Gold:{GoldReward}";
        }
    }
}