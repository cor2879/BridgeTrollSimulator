using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Abilities
{
    public abstract class CombatExecutionStrategy : AbilityExecutionStrategy
    {
        protected int CalculateDamage(
            EntityController initiator,
            EntityController target,
            Ability ability)
        {
            return Mathf.RoundToInt(
                ability.DamageMultiplier *
                Mathf.Max(1, initiator.Attack - target.Defense)
            );
        }
    }
}