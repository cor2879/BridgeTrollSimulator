using UnityEngine;
using OldSchoolGAmes.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Abilities
{
    public abstract class AbilityExecutionStrategy : ScriptableObject
    {
        public virtual bool CanExecute(EntityController initiator, Ability ability)
        {
            return true;
        }

        public abstract void Execute(
            EntityController initiator,
            EntityController target,
            Ability ability);

        public virtual float Evaluate(
            EntityController initiator,
            EntityController target,
            Ability ability)
        {
            return 0f;
        }

        public virtual bool TryExecuteSpecial(
            EntityController initiator,
            EntityController target,
            Ability ability)
        {
            return false;
        }
    }
}