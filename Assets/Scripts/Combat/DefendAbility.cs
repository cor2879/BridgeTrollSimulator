using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Combat
{
    [CreateAssetMenu(menuName = "BridgeTroll/Combat/Abilities/Defend")]
    public class DefendAbility : Ability
    {
        [SerializeField] 
        private int staminaRestore = 2;

        public override bool CanExecute(EntityController initiator)
        {
            return initiator.CanDefend();
        }

        public override void ApplySecondaryEffects(EntityController initiator, EntityController target)
        {
            initiator.Defend();
            initiator.RestoreStamina(staminaRestore);
        }
    }
}