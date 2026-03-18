using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions
{
    public class AcceptSurrenderReaction : Reaction
    {
        public override bool CanReact(IReactor actor, IReactor opponent, ITargetedEvent evt)
        {
            return evt is ConcedeEvent;
        }

        public override float GetWeight(IReactor actor, IReactor opponent, ITargetedEvent evt)
        {
            if (evt is not ConcedeEvent)
                return 0f;

            float weight = 5f;

            if (actor.Resolve < (actor.MaxResolve * 0.3f))
                weight += 3f;

            if (actor.CurrentHealth < actor.MaxHealth * 0.3f)
                weight += 2f;

            return weight;
        }

        public override void Execute(IReceiver actor, IReceiver opponent, ITargetedEvent evt)
        {
            actor.AcceptSurrender(opponent, evt);
        }
    }
}