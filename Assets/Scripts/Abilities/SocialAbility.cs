using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Strategies;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Libraries;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterSkills;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities
{
    [CreateAssetMenu(menuName = "BridgeTroll/Abilities/Social/SocialAbility")]
    public class SocialAbility : Ability
    {
        [Header("Social")]
        [SerializeField] private SkillType governingSkill;
        [SerializeField] private StatType offensiveStat;
        [SerializeField] private StatType defensiveStat;
        [SerializeField] private int basePower = 5;

        public SkillType GoverningSkill => governingSkill;
        public StatType OffensiveStat => offensiveStat;
        public StatType DefensiveStat => defensiveStat;
        public int BasePower => basePower;

        public SocialExecutionStrategy SocialExecutionStrategy
        {
            get => this.ExecutionStrategy as SocialExecutionStrategy;
        }

        public SocialExchangeOutcome ResolveExchange(IActor attacker, IActor defender)
        {
            return this.SocialExecutionStrategy.ResolveExchange(attacker, defender, this);
        }

        public string GetPlayerLine(SocialExchangeOutcome outcome)
        {
            return SocialDialogueLibrary.GetRandom(this.DialogueId, outcome);
        }
    }
}