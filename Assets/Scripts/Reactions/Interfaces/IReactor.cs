using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.Personalities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces
{
    public interface IReactor : IEventSource
    {
        string Name { get; }
        int Resolve { get; }
        int MaxResolve { get; }
        int CurrentHealth { get; }
        int MaxHealth { get; }
        float Aggression { get; }
        int Charisma { get; }
        bool IsPlayerControlled { get; }
        Personality Personality { get; }

        void AcceptSurrender(IReactor opponent, ITargetedEvent evt);
        void DenySurrender(IReactor oppoenent, ITargetedEvent evt);
        void ConcedeCombat(IReceiver opponent);
        void ConcedeSocialDuel(IReceiver opponent);
    }
}