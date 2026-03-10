using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions
{
    public class AttackReaction : Reaction
    {
        public override bool CanReact(IReactor actor, IReactor opponent, ITargetedEvent evt)
        {
            return true;
        }

        public override float GetWeight(IReactor actor, IReactor opponent, ITargetedEvent evt)
        {
            return actor.Aggression;
        }

        public override void Execute(IReceiver actor, IReceiver opponent, ITargetedEvent evt)
        {
            GameEventBus.Publish(new CombatStartedEvent(actor, opponent, Time.frameCount));
        }
    }
}