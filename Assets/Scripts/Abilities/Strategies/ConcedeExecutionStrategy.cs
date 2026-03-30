using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Strategies
{
    public class ConcedeExecutionStrategy : AbilityExecutionStrategy
    {
        public override float Evaluate(IActor self, IActor target, Ability ability)
        {
            float hpRatio = (float)self.CurrentHealth / self.MaxHealth;
            float resolveRatio = (float)self.Resolve / self.MaxResolve;

            float desperation = 1f - Mathf.Min(hpRatio, resolveRatio);

            if (desperation < 0.5f)
                return 0f;

            return desperation * 100f; // 🔥 huge spike when desperate
        }
    }
}