using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Events;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions
{
    public class PayTollReaction : Reaction
    {
        public override bool CanReact(
            IReactor actor,
            IReactor opponent,
            ITargetedEvent evt)
        {
            if (evt is not TollDemandedEvent tollEvt)
                return false;

            if (actor is not EntityController npc)
                return false;

            return npc.Gold >= tollEvt.Amount;
        }

        public override float GetWeight(
            IReactor actor,
            IReactor opponent,
            ITargetedEvent evt)
        {
            if (evt is not TollDemandedEvent tollEvt)
                return 0;

            if (actor is not EntityController npc)
                return 0;

            float wealthFactor = npc.Gold / (float)(tollEvt.Amount + 1);

            return 1f + wealthFactor;
        }

        public override void Execute(
            IReceiver actor,
            IReceiver opponent,
            ITargetedEvent evt)
        {
            if (evt is not TollDemandedEvent tollEvt)
                return;

            if (actor is not IResolver npc)
            {
                return;
            }

            npc.DemandComponent.ResolveNextDemand(npc);
        }
    }
}