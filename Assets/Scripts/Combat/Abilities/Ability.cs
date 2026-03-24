using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Abilities
{
    public abstract class Ability : ScriptableObject
    {
        [SerializeField]
        protected string abilityName;
        [SerializeField]
        protected int staminaCost;
        [SerializeField]
        protected float baseScore = 8f;
        [SerializeField]
        protected float damageMultiplier = 1f;
        [SerializeField]
        protected AbilityExecutionStrategy executionStrategy;
        [SerializeField]
        private List<AbilitySynergy> followUpSynergies = new();
        [SerializeField, ReadOnly]
        protected bool isOffensive = false;

        public string Name => abilityName;
        public int StaminaCost => staminaCost;
        public float DamageMultiplier => damageMultiplier;
        public bool IsOffensive => isOffensive;
        public IReadOnlyList<AbilitySynergy> FollowUpSynergies => followUpSynergies;

        public virtual bool TryExecuteSpecial(
            EntityController initiator,
            EntityController target)
        {
            if (executionStrategy != null)
            {
                return executionStrategy.Evaluate(initiator, target, this);
            }

            return false;    
        }
        
        public virtual bool CanExecute(EntityController initiator)
        {
            if (executionStrategy != null)
            {
                return executionStrategy.Evaluate(initiator, target, this);
            }
            
            return initiator.CanExecute(this);
        }

        public virtual int GetBaseDamage(EntityController initiator, EntityController target)
        {
            return Mathf.Max(1, initiator.Attack - target.Defense);
        }

        public virtual void ApplySecondaryEffects(EntityController initiator, EntityController target)
        { }

        public virtual float Evaluate(EntityController initiator, EntityController target)
        {
            var score = initiator.GetPrimeBonus(this);
            return score;
        }

        public int GetImmediateStaminaCost()
        {
            return StaminaCost;
        }

        public int GetSingleComboCost(EntityController initiator)
        {
            var baseCost = StaminaCost;

            var bestComboCost = baseCost;

            foreach (var synergy in FollowUpSynergies)
            {
                if (synergy.ability == null ||
                    !initiator.Abilities.Contains(synergy.ability))
                {
                    continue;
                }

                return baseCost + synergy.ability.StaminaCost;
            }

            return baseCost;
        }
    }
}