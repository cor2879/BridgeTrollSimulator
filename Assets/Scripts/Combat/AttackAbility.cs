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
    }
}