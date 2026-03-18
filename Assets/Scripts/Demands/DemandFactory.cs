using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Demands
{
    public static class DemandFactory
    {
        public static IDemand CreateSurrenderDemand(IResolver npc, IReceiver player)
        {
            if (npc.DemandComponent.Demands.Any())
            {
                return npc.DemandComponent.Demands.First();
            }

            if (npc.Gold > 0)
            {
                int amount = Mathf.Max(1, Mathf.RoundToInt(npc.Gold * Random.Range(0.3f, 0.7f)));
          
                return new GoldDemand(
                    player,
                    amount,
                    $"{npc.Name} offers {amount} gold to surrender.");
            }

            return new LeaveDemand(
                player,
                $"{npc.Name} offers to leave peacefully");
        }
    }
}