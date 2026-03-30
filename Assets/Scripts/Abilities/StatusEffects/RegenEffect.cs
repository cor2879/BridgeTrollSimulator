using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects
{
    public class RegenEffect : StatusEffect
    {
        public RegenEffect(int magnitude, int duration)
            : base(magnitude, duration) {}

        public override void OnTurnStart(IActor entity)
        {
            entity.RestoreHealth(Magnitude);
        }
    }
}