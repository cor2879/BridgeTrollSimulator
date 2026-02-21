using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Combat
{
    [CreateAssetMenu(menuName = "BridgeTroll/Combat/Abilities/Armor Break")]
    public class ArmorBreakAbility : Ability
    {
        [SerializeField] private int defenseDebuffAmount = 2;
        [SerializeField] private int duration = 2;

        private void OnEnable()
        {
            isOffensive = true;
            damageMultiplier = 0.7f;
            baseScore = 15f;
        }

        public override void ApplySecondaryEffects(EntityController initiator, EntityController target)
        {
            target.ApplyDefenseDebuff(defenseDebuffAmount, duration);
        }

        public override float Evaluate(EntityController initiator, EntityController target)
        {
            if (initiator.CurrentStamina < StaminaCost)
            {
                return float.MinValue;
            }

            var score = baseScore + base.Evaluate(initiator, target);

            var currentStamina = initiator.CurrentStamina;
            var comboCost = GetSingleComboCost(initiator);

            // If full combo is affordable, strong bonus
            if (currentStamina >= comboCost)
            {
                score += 20f;
            }
            else
            {
                // If combo is not affordable, reduce Smash value slightly
                var staminaGap = comboCost - currentStamina;

                score -= staminaGap * 2f;
            }

            return score;
        }
    }
}