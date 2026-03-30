using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects
{
    public class CurePoisonEffect : StatusEffect
    {
        public CurePoisonEffect(int magnitude, int duration)
            : base(magnitude, duration) {}

        public override void OnApply(IActor entity)
        {
            entity.RemoveStatusEffect<PoisonEffect>();
        }
    }
}