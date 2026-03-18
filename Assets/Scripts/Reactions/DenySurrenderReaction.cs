using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions
{
    public class DenySurrenderReaction : Reaction
    {
        public override bool CanReact(IReactor actor, IReactor opponent, ITargetedEvent evt)
        {
            return evt is ConcedeEvent;
        }

        public override float GetWeight(IReactor actor, IReactor opponent, ITargetedEvent evt)
        {
            if (evt is not ConcedeEvent)
                return 0f;

            float weight = 3f;

            if (evt is ConcedeCombatEvent)
            {
                if (actor.CurrentHealth > actor.MaxHealth * 0.7f)
                    weight += 4f;

                if (opponent.CurrentHealth < opponent.MaxHealth * 0.2f)
                    weight += 3f;
            }
            else
            {
                if (actor.Resolve > (actor.MaxResolve * 0.7f))
                    weight += 4f;

                if (opponent.Resolve < (opponent.MaxResolve * 0.2f))
                    weight += 4f;
            }

            return weight;
        }

        public override void Execute(IReceiver actor, IReceiver opponent, ITargetedEvent evt)
        {
            // Combat simply continues
            actor.DenySurrender(opponent, evt as ConcedeEvent);
        }
    }
}