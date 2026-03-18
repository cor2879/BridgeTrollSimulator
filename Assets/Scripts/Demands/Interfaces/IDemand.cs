using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Interfaces
{
    public interface IDemand
    {
        IReceiver Source { get; }

        string Description { get; }

        bool CanResolve(IResolver resolver);

        void OnAccepted(IResolver resolver);
    }
}