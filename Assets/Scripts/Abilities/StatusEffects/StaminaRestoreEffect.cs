using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects
{
    public class StaminaRestoreEffect : StatusEffect
    {
        public StaminaRestoreEffect(int magnitude, int duration)
            : base(magnitude, duration)
        { }

        public override void OnTurnStart(IActor actor)
        {
            actor.RestoreStamina(Magnitude);
        }
    }
}