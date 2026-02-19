namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.StatusEffects
{
    public class IntimidatedEffect : StatusEffect
    {
        private readonly int attackPenalty;

        public IntimidatedEffect(int penalty, int duration)
            : base(duration)
        {
            attackPenalty = penalty;
        }

        public override int ModifyAttack(int baseValue)
        {
            return baseValue - attackPenalty;
        }
    }
}