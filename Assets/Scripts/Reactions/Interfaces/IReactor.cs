using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces
{
    public interface IReactor : IEventSource
    {
        string Name { get; }
        int Resolve { get; }
        float Aggression { get; }
        int Charisma { get; }
    }
}