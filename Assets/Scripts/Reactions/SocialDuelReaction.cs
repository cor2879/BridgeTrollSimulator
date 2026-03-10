using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions
{
    public class SocialDuelReaction : Reaction
    {
        private const float MinAttackerResolve = 0.25f;

        public override bool CanReact(IReactor actor, IReactor opponent, ITargetedEvent evt)
        {
            return actor.Resolve > MinAttackerResolve &&
                opponent.Resolve > 0.0f;
        }

        public override float GetWeight(IReactor actor, IReactor opponent, ITargetedEvent evt)
        {
            return actor.Charisma;
        }

        public override void Execute(IReceiver actor, IReceiver opponent, ITargetedEvent evt)
        {
            GameEventBus.Publish(new SocialDuelStartedEvent(actor, opponent, Time.frameCount));
        }
    }
}