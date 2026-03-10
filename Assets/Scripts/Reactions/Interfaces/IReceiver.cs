using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces
{
    public interface IReceiver : IReactor
    {
        void Receive<TEvent>(TEvent evt) where TEvent : ITargetedEvent;
    }
}