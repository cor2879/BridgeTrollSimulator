namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects
{
    public class IntimidatedEffect : StatusEffect
    {
        public IntimidatedEffect(int magnitude, int duration)
            : base(magnitude, duration)
        { }
        
        public override int ModifyAttack(int baseValue)
        {
            return baseValue - Magnitude;
        }
    }
}