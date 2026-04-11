namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects
{
    public class ArmorBreakEffect : StatusEffect
    {
        public ArmorBreakEffect(int magnitude, int duration, EffectDefinition definition)
            : base(magnitude, duration, definition)
        { }

        public override int ModifyDefense(int baseValue)
        {
            return baseValue - Magnitude;
        }
    }
}