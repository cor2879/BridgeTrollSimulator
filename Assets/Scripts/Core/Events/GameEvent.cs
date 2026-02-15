using System;
using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public abstract class GameEvent : IGameEvent
    {
        public IEventSource Sender { get; }
        public int Frame { get; }
        public DateTime Timestamp { get; }

        protected GameEvent(IEventSource sender, int frame)
        {
            Sender = sender;
            Frame = frame;
            Timestamp = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"{GetType().Name} from {Sender?.SystemType}" +
                $"::{Sender?.SourceName}:: @ frame {Frame}";
        }
    }
}