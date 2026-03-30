using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Strategies
{
    public abstract class AbilityExecutionStrategy 
    {
        public virtual void ApplyAdditionalEffects(
            IActor initiator,
            IActor target,
            Ability ability)
        { }
        public virtual bool CanExecute(IActor initiator, Ability ability)
        {
            return true;
        }

        public virtual float Evaluate(
            IActor initiator,
            IActor target,
            Ability ability)
        {
            return 0f;
        }

        public virtual bool TryExecuteSpecial(
            IActor initiator,
            IActor target,
            Ability ability)
        {
            return false;
        }
    }
}