using System;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces
{
    public interface IGameEvent
    {
        IEventSource Sender { get; }
        int Frame { get; }
        DateTime Timestamp { get; }
    }
}