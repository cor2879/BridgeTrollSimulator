using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Demands
{
    public class LeaveDemand : IDemand
    {
        public IReceiver Source { get; }

        public string Description { get; }

        public LeaveDemand(IReceiver source, string description = "")
        {
            Source = source;
            Description = description;
        }

        public bool CanResolve(IResolver resolver)
        {
            return true;
        }

        public void OnAccepted(IResolver resolver)
        {
            resolver.Leave();
        }

        public override string ToString()
        {
            return $"{nameof(LeaveDemand)}::Source:{Source}::Description:\"{Description}\"";
        }
    }
}