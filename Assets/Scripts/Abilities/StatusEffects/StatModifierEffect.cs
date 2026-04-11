using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects
{
    public class StatModifierEffect : StatusEffect
    {
        private readonly DerivedStatType stat;
        private readonly int flatAmount;
        private readonly float percentAmount;

        public override string EffectId => $"{stat}_{GetType().FullName}";

        public StatModifierEffect(
            DerivedStatType stat,
            int flatAmount,
            float percentAmount,
            int duration,
            EffectDefinition definition)
            : base(0, duration, definition) // magnitude unused here
        {
            this.stat = stat;
            this.flatAmount = flatAmount;
            this.percentAmount = percentAmount;
        }

        private int Apply(int value)
        {
            value += flatAmount;

            if (percentAmount != 0f)
            {
                value = UnityEngine.Mathf.RoundToInt(value * (1f + percentAmount));
            }

            return UnityEngine.Mathf.Max(0, value);
        }

        public override int ModifyAttack(int baseValue)
        {
            return stat == DerivedStatType.Attack
                ? Apply(baseValue)
                : baseValue;
        }

        public override int ModifyDefense(int baseValue)
        {
            return stat == DerivedStatType.Defense
                ? Apply(baseValue)
                : baseValue;
        }
    }
}