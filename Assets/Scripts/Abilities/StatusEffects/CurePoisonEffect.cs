using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects
{
    public class CurePoisonEffect : StatusEffect
    {
        public CurePoisonEffect(int magnitude, int duration, EffectDefinition definition)
            : base(magnitude, duration, definition)
        { }

        public override void OnApply(IActor entity)
        {
            entity.RemoveStatusEffect<PoisonEffect>();
        }
    }
}