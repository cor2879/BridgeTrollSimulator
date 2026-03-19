using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Scenarios;

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

            var scenario = DemandTollScenario.Instance;

            var reaction = ReactionResolver.Resolve(
                scenario,
                npc,
                player,
                evt);
            
            reaction.Execute(npc, player, evt);
        }

        private void OnRefusedPassage(RefusePassageEvent evt)
        {
            var npc = evt.Target as IResolver;
            var player = evt.Initiator;

            var demand = new LeaveDemand(player);
            
            npc.DemandComponent.AddDemand(demand);

            var scenario = RefusePassageScenario.Instance;
            var reaction = ReactionResolver.Resolve(
                scenario,
                npc,
                player,
                evt);

            reaction.Execute(npc, player, evt);
        }
    }
}