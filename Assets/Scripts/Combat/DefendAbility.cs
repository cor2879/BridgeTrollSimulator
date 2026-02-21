using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Combat
{
    [CreateAssetMenu(menuName = "BridgeTroll/Combat/Abilities/Defend")]
    public class DefendAbility : Ability
    {
        [SerializeField] 
        private int staminaRestore = 2;

        public int StaminaRestoreAmount => staminaRestore;

        private void Awake()
        {
            baseScore = 5f;
        }

        public override bool CanExecute(EntityController initiator)
        {
            return initiator.CanDefend();
        }

        public override void ApplySecondaryEffects(EntityController initiator, EntityController target)
        {
            initiator.Defend();
            initiator.RestoreStamina(staminaRestore);
        }

        public override float Evaluate(EntityController initiator, EntityController target)
        {
            var score = baseScore + base.Evaluate(initiator, target);

            var currentStamina = initiator.CurrentStamina;
            var staminaAfterDefend = Mathf.Min(
                initiator.MaxStamina,
                currentStamina + StaminaRestoreAmount
            );

            var bestComboCost = 0;

            // Find highest combo cost available to this initiator
            foreach (var ability in initiator.Abilities)
            {
                if (ability == null || ability == this)
                    continue;

                var comboCost = ability.GetSingleComboCost(initiator);

                if (comboCost > bestComboCost)
                    bestComboCost = comboCost;
            }

            // If defending helps reach that combo threshold, reward it
            if (currentStamina < bestComboCost &&
                staminaAfterDefend > currentStamina)
            {
                var gapBefore = bestComboCost - currentStamina;
                var gapAfter = Mathf.Max(0, bestComboCost - staminaAfterDefend);

                var staminaProgress = gapBefore - gapAfter;

                // Reward actual forward progress toward combo
                if (staminaProgress > 0)
                    score += staminaProgress * 5f;
            }

            // Optional: small recovery incentive if stamina critically low
            if (currentStamina < 2)
                score += 10f;

            return score;
        }
    }
}