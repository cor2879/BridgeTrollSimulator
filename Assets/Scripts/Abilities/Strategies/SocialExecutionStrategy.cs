using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Strategies
{
    public class SocialExecutionStrategy : AbilityExecutionStrategy
    {
        public SocialExchangeOutcome ResolveExchange(
            IActor attacker,
            IActor defender,
            Ability ability)
        {
            if (ability is not SocialAbility social)
            {
                Debug.LogError($"Ability {ability.Name} is not a SocialAbility!");
                return default;
            }

            int attackRoll = Dice.RollD20();
            int defenseRoll = Dice.RollD20();

            int attackTotal =
                attackRoll +
                attacker.BaseSkills.Get(social.GoverningSkill) +
                attacker.BaseStats.GetModifier(social.OffensiveStat);

            int defenseTotal =
                defenseRoll +
                defender.BaseStats.GetModifier(social.DefensiveStat);

            int margin = attackTotal - defenseTotal;

            margin += attacker.Momentum - defender.Momentum;

            bool isCritical = attackRoll == 20 || attackRoll == 1;

            SocialExchangeResult result;
            bool damageSelf;
            int resolveAmount;

            if (attackRoll == 20)
            {
                result = SocialExchangeResult.StrongSuccess;
                damageSelf = false;
                resolveAmount = social.BasePower + Mathf.Max(5, margin);
            }
            else if (attackRoll == 1)
            {
                result = SocialExchangeResult.StrongFailure;
                damageSelf = true;
                resolveAmount = Mathf.Max(1, Mathf.Abs(margin) + social.BasePower);
            }
            else
            {
                if (margin >= 10)
                {
                    result = SocialExchangeResult.StrongSuccess;
                    damageSelf = false;
                    resolveAmount = social.BasePower + margin;
                }
                else if (margin >= 0)
                {
                    result = SocialExchangeResult.WeakSuccess;
                    damageSelf = false;
                    resolveAmount = Mathf.Max(1, social.BasePower + margin / 2);
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
                social.GoverningSkill,
                result,
                resolveAmount,
                damageSelf,
                isCritical,
                margin);
        }
    }
}