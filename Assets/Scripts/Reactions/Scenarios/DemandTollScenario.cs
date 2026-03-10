using System.Collections.Generic;
using System.Linq;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Scenarios
{
    public class DemandTollScenario : IReactionScenario
    {
        public static readonly DemandTollScenario Instance = new DemandTollScenario();

        private static readonly List<Reaction> reactions = new()
        {
            Reaction.GetInstance<AttackReaction>(),
            Reaction.GetInstance<PayTollReaction>(),
            Reaction.GetInstance<SocialDuelReaction>(),
            // Not implmented yet // Reaction.GetInstance<BegReaction>(),
            Reaction.GetInstance<LeaveReaction>()
        };

        protected DemandTollScenario() { }

        public IEnumerable<Reaction> GetAvailableReactions(IReactor actor, IReactor opponent, ITargetedEvent evt)
        {
            return reactions.Where(r => r.CanReact(actor, opponent, evt));
        }
    }
}