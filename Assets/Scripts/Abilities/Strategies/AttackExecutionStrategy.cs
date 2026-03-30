using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Strategies
{
    public class AttackExecutionStrategy : AbilityExecutionStrategy
    {
        public override bool CanExecute(IActor initiator, Ability ability)
        {
            return initiator.CanExecute(ability);
        }

        public override float Evaluate(IActor initiator, IActor target, Ability ability)
        {
            if (initiator.CurrentStamina < ability.StaminaCost)
                return 0f;

            var score = ability.BaseScore + initiator.GetPrimeBonus(ability);

            var damage = Mathf.RoundToInt(
                ability.DamageMultiplier *
                Mathf.Max(1, initiator.Attack - target.Defense));

            if (target.CurrentHealth < damage)
                score += 20f;

            // 🔥 NEW: Combo awareness
            var comboCost = ability.GetSingleComboCost(initiator);

            if (initiator.CurrentStamina >= comboCost)
            {
                score += 10f; // reward being able to chain
            }
            else
            {
                var gap = comboCost - initiator.CurrentStamina;
                score -= gap * 1.5f; // penalize if not ready
            }

            return score;
        }
    }
}