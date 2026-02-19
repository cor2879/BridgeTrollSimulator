namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.StatusEffects
{
    public class ArmorBreakEffect : StatusEffect
    {
        private readonly int defenseReduction;

        public ArmorBreakEffect(int amount, int duration)
            : base(duration)
        {
            defenseReduction = amount;
        }

        public override int ModifyDefense(int baseValue)
        {
            return baseValue - defenseReduction;
        }
    }
}