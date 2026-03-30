using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Strategies
{
    public class DefendExecutionStrategy : AbilityExecutionStrategy
    {
        public override void ApplyAdditionalEffects(
            IActor initiator,
            IActor target,
            Ability ability)
        {
            initiator.Defend();
            initiator.RestoreStamina(ability.StaminaRestore);
        }

        public override bool CanExecute(IActor initiator, Ability ability)
        {
            return initiator.CanExecute(ability);
        }

        public override float Evaluate(IActor initiator, IActor target, Ability ability)
        {
            var score = ability.BaseScore + initiator.GetPrimeBonus(ability);

            var currentStamina = initiator.CurrentStamina;
            var staminaAfter = Mathf.Min(
                initiator.MaxStamina,
                currentStamina + ability.StaminaRestore
            );

            int bestComboCost = 0;

            foreach (var a in initiator.ActiveAbilities)
            {
                if (a == null || a == ability)
                    continue;

                var comboCost = a.GetSingleComboCost(initiator);

                if (comboCost > bestComboCost)
                    bestComboCost = comboCost;
            }

            if (currentStamina < bestComboCost &&
                staminaAfter > currentStamina)
            {
                var gapBefore = bestComboCost - currentStamina;
                var gapAfter = Mathf.Max(0, bestComboCost - staminaAfter);

                var progress = gapBefore - gapAfter;

                if (progress > 0)
                    score += progress * 5f;
            }

            if (currentStamina < 2)
                score += 10f;

            return score;
        }

        public override bool TryExecuteSpecial(IActor initiator, IActor target, Ability ability)
        {
            // Not a "special", handled in ApplySecondaryEffects
            return false;
        }
    }
}