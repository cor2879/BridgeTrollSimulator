using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects
{
    public abstract class StatusEffect
    {
        public int RemainingTurns { get; protected set; }
        public int Magnitude { get; protected set; }
        public virtual string EffectId => GetType().FullName;

        public StatusEffect(int magnitude, int duration)
        {
            RemainingTurns = duration;
            Magnitude = magnitude;
        }

        public virtual void OnApply(IActor entity) { }

        public virtual void OnTurnStart(IActor entity) { }

        public virtual void OnTurnEnd(IActor entity) { }

        public virtual void OnExpire(IActor entity) { }

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

            if (RemainingTurns <= 0)
            {
                OnExpire(entity);
            }
        }

        public bool IsExpired => RemainingTurns <= 0;
    }
}