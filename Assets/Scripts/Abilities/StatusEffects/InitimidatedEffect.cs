namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects
{
    public class IntimidatedEffect : StatusEffect
    {
        public IntimidatedEffect(int magnitude, int duration, EffectDefinition definition)
            : base(magnitude, duration, definition)
        { }
        
        public override int ModifyAttack(int baseValue)
        {
            return baseValue - Magnitude;
        }
    }
}