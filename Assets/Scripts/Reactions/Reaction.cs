using System.Collections.Generic;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions
{
    public abstract class Reaction
    {
        private static Dictionary<System.Type, Reaction> instances = new();

        protected Reaction() {}

        public abstract bool CanReact(IReactor actor, IReactor opponent, ITargetedEvent evt);
        public abstract float GetWeight(IReactor actor, IReactor opponent, ITargetedEvent evt);
        public abstract void Execute(IReceiver actor, IReceiver opponent, ITargetedEvent evt);

        public static TReaction GetInstance<TReaction>() where TReaction : Reaction, new()
        {
            var type = typeof(TReaction);

            if (instances.TryGetValue(type, out var existing))
            {
                return (TReaction)existing;
            }

            instances.Add(type, new TReaction());
            return (TReaction)instances[type];
        }
    }
}