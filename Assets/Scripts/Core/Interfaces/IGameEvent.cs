using System;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces
{
    public interface IGameEvent
    {
        IEventSource Sender { get; }
        int Frame { get; }
        DateTime Timestamp { get; }
    }

    public interface ITargetedEvent : IGameEvent
    {
        EntityController Target { get; }
    }
}