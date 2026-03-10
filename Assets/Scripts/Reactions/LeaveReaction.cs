using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions
{
    public class LeaveReaction : Reaction
    {
        public override bool CanReact(IReactor actor, IReactor opponent, ITargetedEvent evt)
        {
            return true;
        }

        public override float GetWeight(IReactor actor, IReactor opponent, ITargetedEvent evt)
        {
            return Mathf.Max(1, 100 - actor.Resolve);
        }

        public override void Execute(IReceiver actor, IReceiver opponent, ITargetedEvent evt)
        {
            if (actor is NpcController npc)
            {
                npc.Leave();
            }
        }
    }
}