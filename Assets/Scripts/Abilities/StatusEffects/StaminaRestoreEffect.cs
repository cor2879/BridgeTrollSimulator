using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects
{
    public class StaminaRestoreEffect : StatusEffect
    {
        public StaminaRestoreEffect(int magnitude, int duration, EffectDefinition definition)
            : base(magnitude, duration, definition)
        { }

        public override void OnTurnEnd(IActor actor)
        {
            actor.RestoreStamina(Magnitude);
        }
    }
}