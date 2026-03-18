using System.Collections.Generic;
using System.Linq;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Scenarios
{
    public class SurrenderScenario : IReactionScenario
    {
        public static readonly SurrenderScenario Instance = new SurrenderScenario();

        private static readonly List<Reaction> reactions = new()
        {
            Reaction.GetInstance<AcceptSurrenderReaction>(),
            Reaction.GetInstance<DenySurrenderReaction>()
        };

        protected SurrenderScenario() { }

        public IEnumerable<Reaction> GetAvailableReactions(
            IReactor actor,
            IReactor opponent,
            ITargetedEvent evt)
        {
            return reactions.Where(r => r.CanReact(actor, opponent, evt));
        }
    }
}