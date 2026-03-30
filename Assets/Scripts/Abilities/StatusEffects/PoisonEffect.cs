using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects
{
    public class PoisonEffect : StatusEffect
    {
        public PoisonEffect(int magnitude, int duration)
            : base(magnitude, duration)
        { }

        public override void OnTurnEnd(IActor entity)
        {
            entity.TakeDamage(Magnitude);
        }
    }
}