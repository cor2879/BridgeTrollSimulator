using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects
{
    public abstract class StatusEffect
    {
        public int RemainingTurns { get; protected set; }
        public int Magnitude { get; protected set; }
        protected EffectDefinition Definition { get; }
        public virtual string EffectId => GetType().FullName;

        public StatusEffect(int magnitude, int duration, EffectDefinition definition)
        {
            RemainingTurns = duration;
            Magnitude = magnitude;
            Definition = definition;
        }

        public virtual void OnApply(IActor entity) { }

        public virtual void OnTurnStart(IActor entity) { }

        public virtual void OnTurnEnd(IActor entity) { }

        public virtual void OnExpire(IActor entity)
        {
            GameEventBus.Publish(
                new StatusEffectExpiredEvent(
                    entity,
                    Definition,
                    Magnitude));
        }

        public virtual int ModifyAttack(int baseValue) => baseValue;
        public virtual int ModifyDefense(int baseValue) => baseValue;

        public virtual void Refresh(StatusEffect newEffect)
        {
            RemainingTurns = newEffect.RemainingTurns;
            
            if (newEffect.Magnitude > this.Magnitude)
            {
                Magnitude = newEffect.Magnitude;
            }
        }

        public void Tick(IActor entity)
        {
            if (entity.IsSurrendering)
            {
                return;
            }
            
            RemainingTurns--;

            GameEventBus.Publish(
                new StatusEffectTickEvent(entity, Definition, Magnitude));

            if (RemainingTurns <= 0)
            {
                OnExpire(entity);
            }
        }

        public bool IsExpired => RemainingTurns <= 0;
    }
}