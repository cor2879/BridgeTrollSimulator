using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects
{
    public class RegenEffect : StatusEffect
    {
        public RegenEffect(int magnitude, int duration, EffectDefinition definition)
            : base(magnitude, duration, definition)
        { }

        public override void OnTurnEnd(IActor entity)
        {
            entity.RestoreHealth(Magnitude);
        }
    }
}