using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions
{
    public static class ReactionResolver
    {
        public static Reaction Resolve(
            IReactionScenario scenario,
            IReactor actor,
            IReactor opponent,
            ITargetedEvent evt)
        {
            Reaction chosen = null;
            float totalWeight = 0.0f;
            float roll = 0.0f;

            var weightedReactions = scenario
                .GetAvailableReactions(actor, opponent, evt)
                .Select(r => (
                    reaction: r,
                    weight: r.GetWeight(actor, opponent, evt)
                ))
                .ToList();

            if (!weightedReactions.Any())
            {
                chosen = Reaction.GetInstance<LeaveReaction>();
            }
            else
            {
                totalWeight = weightedReactions.Sum(r => r.weight);
                roll = Random.value * totalWeight;

                float cumulative = 0f;

                foreach (var entry in weightedReactions)
                {
                    cumulative += entry.weight;

                    if (roll <= cumulative)
                    {
                        chosen = entry.reaction;
                        break;
                    }
                }
            }

            if (chosen == null)
            {
                chosen = weightedReactions.First().reaction;
            }

#if UNITY_EDITOR
            LogDecision(scenario, actor, opponent, weightedReactions, roll, chosen);
#endif
            return chosen;
        }

        private static void LogDecision(
            IReactionScenario scenario,
            IReactor actor,
            IReactor opponent,
            IEnumerable<(Reaction reaction, float weight)> reactions,
            float roll,
            Reaction chosen)
        {
            var sb = new StringBuilder();
            sb.AppendLine("----- Reaction Decision -----");
            sb.AppendLine($"Scenario: {scenario.GetType().Name}");
            sb.AppendLine($"Actor: {actor.Name}");
            sb.AppendLine($"Opponent: {opponent.Name}");

            foreach (var r in reactions)
            {
                sb.AppendLine($"{r.reaction.GetType().Name} weight: {r.weight:F2}");
            }

            sb.AppendLine($"Roll: {roll:F2}");
            sb.AppendLine($"Chosen: {chosen.GetType().Name}");

            Debug.Log(sb.ToString());
        }
    }
}