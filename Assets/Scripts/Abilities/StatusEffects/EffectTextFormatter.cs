using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects
{
    public static class EffectTextFormatter
    {
        public static string Format(string template, IActor target, int amount)
        {
            if (string.IsNullOrEmpty(template))
                return null;

            return template
                .Replace("{target}", target.Name)
                .Replace("{amount}", amount.ToString());
        }
    }
}