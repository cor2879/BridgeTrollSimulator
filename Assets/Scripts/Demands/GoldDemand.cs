using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Demands
{
    public class GoldDemand : IDemand
    {
        public IReceiver Source { get; }

        public string Description { get; }

        public int Amount { get; }

        public GoldDemand(IReceiver source, int amount, string description = "")
        {
            Source = source;
            Amount = amount;
            Description = description;
        }

        public bool CanResolve(IResolver resolver)
        {
            return resolver.Gold >= Amount;
        }

        public void OnAccepted(IResolver resolver)
        {
            resolver.PayToll(Source, Amount);
        }

        public override string ToString()
        {
            return $"{nameof(GoldDemand)}::Source:{Source}::Description:\"{Description}\"::Amount:{Amount}";
        }
    }
}