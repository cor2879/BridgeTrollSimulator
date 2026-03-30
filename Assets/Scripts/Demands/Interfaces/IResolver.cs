using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Interfaces
{
    public interface IResolver : IReceiver
    {
        int Gold { get; }
        int Fame { get; }
        int Respect { get; }
        DemandComponent DemandComponent { get; }
        ControlMode CurrentControlMode { get; }

        void PayToll(IReceiver receiver, int amount);
        void Leave();
    }
}