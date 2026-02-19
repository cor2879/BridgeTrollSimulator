using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.StatusEffects
{
    public abstract class StatusEffect
    {
        public int RemainingTurns { get; protected set; }

        protected StatusEffect(int duration)
        {
            RemainingTurns = duration;
        }

        public virtual void OnApply(EntityController entity) { }

        public virtual void OnTurnStart(EntityController entity) { }

        public virtual void OnTurnEnd(EntityController entity) { }

        public virtual void OnExpire(EntityController entity) { }

        public virtual int ModifyAttack(int baseValue) => baseValue;
        public virtual int ModifyDefense(int baseValue) => baseValue;

        public void Tick(EntityController entity)
        {
            RemainingTurns--;

            if (RemainingTurns <= 0)
            {
                OnExpire(entity);
            }
        }

        public bool IsExpired => RemainingTurns <= 0;
    }
}