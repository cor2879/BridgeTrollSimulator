using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterSkills;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Abilities
{
    [CreateAssetMenu(menuName = "BridgeTroll/Abilities/LegacySocialAbility")]
    public class LegacySocialAbility : ScriptableObject
    {
        [SerializeField] private string abilityName;
        [SerializeField] private Sprite icon;
        [SerializeField] private SkillType governingSkill;
        [SerializeField] private StatType offensiveStat;
        [SerializeField] private StatType defensiveStat;
        [SerializeField] private int basePower = 5;
        [SerializeField] private string[] weakSuccessLines;
        [SerializeField] private string[] strongSuccessLines;
        [SerializeField] private string[] criticalSuccessLines;
        [SerializeField] private string[] weakFailureLines;
        [SerializeField] private string[] strongFailureLines;
        [SerializeField] private string[] criticalFailureLines;

        public string AbilityName => abilityName;
        public Sprite Icon => icon;
        public SkillType GoverningSkill => governingSkill;
        public StatType OffensiveStat => offensiveStat;

        public virtual bool TryExecuteSpecial(
            EntityController initiator,
            EntityController target)
        {
            return false;
        }

        public SocialExchangeOutcome ResolveExchange(
            EntityController attacker,
            EntityController defender)
        {
            int attackRoll = Dice.RollD20();
            int defenseRoll = Dice.RollD20();

            int attackTotal =
                attackRoll +
                attacker.BaseSkills.Get(governingSkill) +
                attacker.BaseStats.GetModifier(offensiveStat);

            int defenseTotal =
                defenseRoll +
                defender.BaseStats.GetModifier(defensiveStat);

            int margin = attackTotal - defenseTotal;

            // 🔥 APPLY MOMENTUM HERE
            margin += attacker.Momentum - defender.Momentum;

            bool isCritical = attackRoll == 20 || attackRoll == 1;

            SocialExchangeResult result;
            bool damageSelf;
            int resolveAmount;

            if (attackRoll == 20)
            {
                result = SocialExchangeResult.StrongSuccess;
                damageSelf = false;
                resolveAmount = basePower + Mathf.Max(5, margin);
            }
            else if (attackRoll == 1)
            {
                result = SocialExchangeResult.StrongFailure;
                damageSelf = true;
                resolveAmount = Mathf.Max(1, Mathf.Abs(margin) + basePower);
            }
            else
            {
                if (margin >= 10)
                {
                    result = SocialExchangeResult.StrongSuccess;
                    damageSelf = false;
                    resolveAmount = basePower + margin;
                }
                else if (margin >= 0)
                {
                    result = SocialExchangeResult.WeakSuccess;
                    damageSelf = false;
                    resolveAmount = Mathf.Max(1, basePower + margin / 2);
                }
                else if (margin <= -10)
                {
                    result = SocialExchangeResult.StrongFailure;
                    damageSelf = true;
                    resolveAmount = Mathf.Max(1, Mathf.Abs(margin));
                }
                else
                {
                    result = SocialExchangeResult.WeakFailure;
                    damageSelf = true;
                    resolveAmount = Mathf.Max(1, Mathf.Abs(margin) / 2);
                }
            }

            return new SocialExchangeOutcome(
                governingSkill,
                result,
                resolveAmount,
                damageSelf,
                isCritical,
                margin);
        }

        public string GetPlayerLine(SocialExchangeOutcome outcome)
        {
            if (outcome.IsCritical)
            {
                if (outcome.Result == SocialExchangeResult.StrongSuccess &&
                    criticalSuccessLines.Length > 0)
                {
                    return GetRandom(criticalSuccessLines);
                }

                if (outcome.Result == SocialExchangeResult.StrongFailure &&
                    criticalFailureLines.Length > 0)
                {
                    return GetRandom(criticalFailureLines);
                }
            }

            return outcome.Result switch
            {
                SocialExchangeResult.StrongSuccess => GetRandom(strongSuccessLines),
                SocialExchangeResult.WeakSuccess => GetRandom(weakSuccessLines),
                SocialExchangeResult.WeakFailure => GetRandom(weakFailureLines),
                SocialExchangeResult.StrongFailure => GetRandom(strongFailureLines),
                _ => string.Empty
            };
        }

        private string GetRandom(string[] lines)
        {
            if (lines == null || lines.Length == 0)
            {
                return string.Empty;
            }

            return lines[Random.Range(0, lines.Length)];
        }
    }
}