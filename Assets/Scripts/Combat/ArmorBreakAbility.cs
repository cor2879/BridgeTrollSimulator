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
        }

        public override void ApplySecondaryEffects(EntityController initiator, EntityController target)
        {
            target.ApplyDefenseDebuff(defenseDebuffAmount, duration);
        }
    }
}