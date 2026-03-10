using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces
{
    public interface ITargetedEvent : IGameEvent
    {
        IReceiver Target { get; }
    }
}