using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Combat
{
    public abstract class Ability : ScriptableObject
    {
        [SerializeField]
        protected string abilityName;
        [SerializeField]
        protected int staminaCost;
        [SerializeField]
        protected float damageMultiplier = 1f;
        [SerializeField, ReadOnly]
        protected bool isOffensive = false;


        public string Name => abilityName;
        public int StaminaCost => staminaCost;
        public float DamageMultiplier => damageMultiplier;
        public bool IsOffensive => isOffensive;

        public virtual bool CanExecute(EntityController initiator)
        {
            return initiator.CanExecute(this);
        }

        public virtual int GetBaseDamage(EntityController initiator, EntityController target)
        {
            return Mathf.Max(1, initiator.Attack - target.Defense);
        }

        public virtual void ApplySecondaryEffects(EntityController initiator, EntityController target)
        { }
    }
}