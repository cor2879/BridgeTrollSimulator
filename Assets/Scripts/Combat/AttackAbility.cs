using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Combat
{
    [CreateAssetMenu(menuName = "BridgeTroll/Combat/Abilities/Attack")]
    public class AttackAbility : Ability
    {
        private void OnEnable()
        {
            isOffensive = true;
            damageMultiplier = 1f;
        }

        public override bool CanExecute(EntityController initiator)
        {
            return initiator.CanAttack();
        }

        public override float Evaluate(EntityController initiator, EntityController target)
        {
            if (initiator.CurrentStamina < StaminaCost)
            {
                return 0f;
            }

            var score = baseScore + base.Evaluate(initiator, target);

            if (target.CurrentHealth < damageMultiplier * initiator.Attack - target.Defense)
            {
                score += 20f;
            }

            return score;
        }
    }
}