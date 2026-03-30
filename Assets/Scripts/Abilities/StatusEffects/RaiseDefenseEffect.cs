namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects
{
    public class RaiseDefenseEffect : StatusEffect
    {
        public RaiseDefenseEffect(int magnitude, int duration)
            : base(magnitude, duration)
        { }

        public override int ModifyDefense(int baseValue)
        {
            return baseValue + Magnitude;
        }
    }
}