using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects
{
    public class PoisonEffect : StatusEffect
    {
        public PoisonEffect(int magnitude, int duration, EffectDefinition definition)
            : base(magnitude, duration, definition)
        { }

        public override void OnTurnEnd(IActor entity)
        {
            entity.TakeDamage(Magnitude);
        }
    }
}