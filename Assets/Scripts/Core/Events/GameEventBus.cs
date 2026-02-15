using System;
using System.Collections.Generic;

using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events
{
    public static class GameEventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> subscribers = new();

        public static void Subscribe<T>(Action<T> callBack) where T : IGameEvent
        {
            var type = typeof(T);

            if (!subscribers.ContainsKey(type))
            {
                subscribers[type] = new List<Delegate>();
            }

            subscribers[type].Add(callBack);
        }

        public static void Unsubscribe<T>(Action<T> handler) where T : IGameEvent
        {
            var type = typeof(T);

            if (!subscribers.TryGetValue(type, out var existing))
            {
                return;
            }

            existing.Remove(handler);

            if (existing.Count == 0)
            {
                subscribers.Remove(type);
            }
        }

        public static void Publish<T>(T gameEvent) where T : IGameEvent
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[Event] {gameEvent}");
#endif

            var type = typeof(T);

            if (!subscribers.ContainsKey(type))
            {
                return;
            }

            foreach (var subscriber in subscribers[type].ToArray())
            {
                ((Action<T>)subscriber)?.Invoke(gameEvent);
            }
        }
    }
}