using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Demands
{
    public class DemandSystem : MonoBehaviour
    {
        private void OnEnable()
        {
            GameEventBus.Subscribe<TollDemandedEvent>(OnTollDemanded);
            GameEventBus.Subscribe<RefusePassageEvent>(OnRefusedPassage);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<TollDemandedEvent>(OnTollDemanded);
            GameEventBus.Unsubscribe<RefusePassageEvent>(OnRefusedPassage);
        }

        private void OnTollDemanded(TollDemandedEvent evt)
        {
            var npc = evt.Target as IResolver;
            var player = evt.Initiator;

            var demand = new GoldDemand(player, evt.Amount);

            npc.DemandComponent.AddDemand(demand);
        }

        private void OnRefusedPassage(RefusePassageEvent evt)
        {
            var npc = evt.Target as IResolver;
            var player = evt.Initiator;

            var demand = new LeaveDemand(player);
            
            npc.DemandComponent.AddDemand(demand);
        }
    }
}