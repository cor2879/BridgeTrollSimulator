using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces
{
    public interface IEventSource
    {
        string SourceName { get; }
        GameSystemType SystemType { get; }
    }
}