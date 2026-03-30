namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects
{
    public class ArmorBreakEffect : StatusEffect
    {
        public ArmorBreakEffect(int magnitude, int duration)
            : base(magnitude, duration)
        { }

        public override int ModifyDefense(int baseValue)
        {
            return baseValue - Magnitude;
        }
    }
}