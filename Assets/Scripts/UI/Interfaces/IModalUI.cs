using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI.Interfaces
{
    public interface IModalUI : IEventSource
    {
        bool IsBlockingUI { get; }
    }
}