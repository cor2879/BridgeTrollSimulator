using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Requirements;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Strategies;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities
{
    [CreateAssetMenu(menuName = "BridgeTroll/Abilities/Ability")]
    public class Ability : ScriptableObject
    {
        [SerializeField, ExecutionStrategyDropdown]
        private string executionStrategyTypeName;

        private AbilityExecutionStrategy _cachedStrategy;

        [SerializeField]
        protected string abilityName;
        [SerializeField, TextArea(2, 5)]
        protected string description;
        [SerializeField]
        protected bool canExecuteExhausted = false;
        [SerializeField]
        protected AbilityRarity rarity = AbilityRarity.Common;
        [SerializeField]
        protected int staminaCost;
        [SerializeField]
        protected int staminaRestore;
        [SerializeField]
        protected float baseScore = 8f;
        [SerializeField]
        protected float damageMultiplier = 1f;
        [SerializeField, Range(0f, 1f)]
        private float defenseIgnorePercent;
        [SerializeReference, RequirementDropdown]
        private List<AbilityRequirement> requirements;
        [SerializeField]
        private List<EffectDefinition> effects;
        [SerializeField]
        private List<AbilitySynergy> followUpSynergies = new();
        [SerializeField]
        protected bool isOffensive = false;

        public float BaseScore => baseScore;
        public bool CanExecuteExhausted => canExecuteExhausted;
        public string Name => abilityName;
        public AbilityRarity Rarity => rarity;
        public string DialogueId => Name.Trim().ToLowerInvariant();
        public int StaminaCost => staminaCost;
        public int StaminaRestore => staminaRestore;
        public float DamageMultiplier => damageMultiplier;
        public float DefenseIgnorePercent => defenseIgnorePercent;
        public bool IsOffensive => isOffensive;
        public IReadOnlyList<AbilitySynergy> FollowUpSynergies => followUpSynergies;
        public string Description => description;

        public AbilityExecutionStrategy ExecutionStrategy
        {
            get
            {
                if (_cachedStrategy != null)
                {
                    return _cachedStrategy;
                }

                if (string.IsNullOrEmpty(executionStrategyTypeName))
                {
                    return null;
                }

                var type = System.AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.FullName == executionStrategyTypeName);

                if (type != null)
                {
                    _cachedStrategy = (AbilityExecutionStrategy)System.Activator.CreateInstance(type);
                }

                return _cachedStrategy;
            }
        }

        public virtual bool TryExecuteSpecial(
            IActor initiator,
            IActor target)
        {
            if (ExecutionStrategy != null)
            {
                return ExecutionStrategy.TryExecuteSpecial(initiator, target, this);
            }

            return false;    
        }
        
        public virtual bool CanExecute(IActor initiator)
        {
            if (ExecutionStrategy != null)
            {
                return ExecutionStrategy.CanExecute(initiator, this);
            }
            
            return initiator.CanExecute(this);
        }

        public virtual int GetBaseDamage(IActor initiator, IActor target)
        {
            return Mathf.Max(1, initiator.Attack - target.Defense);
        }

        public virtual void ApplySecondaryEffects(IActor initiator, IActor target)
        { 
            ExecutionStrategy?.ApplyAdditionalEffects(initiator, target, this);

            foreach (var factory in effects)
            {
                var statusEffect = factory.Create();

                var recipient = factory.Target == EffectTarget.Self
                    ? initiator
                    : target;

                recipient.AddStatusEffect(statusEffect, factory.StackingType);

                GameEventBus.Publish(
                    new StatusEffectAppliedEvent(
                        initiator,
                        recipient,
                        factory));
            }            
        }

        public virtual float Evaluate(IActor initiator, IActor target)
        {
            try
            {
                return ExecutionStrategy.Evaluate(initiator, target, this);
            }
            catch (System.Exception)
            {
                Debug.Log($"{Name} ability has no Execution Strategy.");
                return 0f;
            }

            // var score = initiator.GetPrimeBonus(this);
            // eturn score;
        }

        public int GetImmediateStaminaCost()
        {
            return StaminaCost;
        }

        public int GetSingleComboCost(IActor initiator)
        {
            var baseCost = StaminaCost;

            var bestComboCost = baseCost;

            foreach (var synergy in FollowUpSynergies)
            {
                if (synergy.ability == null ||
                    !initiator.ActiveAbilities.Contains(synergy.ability))
                {
                    continue;
                }

                return baseCost + synergy.ability.StaminaCost;
            }

            return baseCost;
        }
    }
}