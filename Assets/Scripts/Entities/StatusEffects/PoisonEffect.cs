using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.StatusEffects
{
    public class PoisonEffect : StatusEffect
    {
        private readonly int damagePerTurn;

        public PoisonEffect(int damage, int duration)
            : base(duration)
        {
            damagePerTurn = damage;
        }

        public override void OnTurnStart(EntityController entity)
        {
            entity.TakeDamage(damagePerTurn);
        }
    }
}