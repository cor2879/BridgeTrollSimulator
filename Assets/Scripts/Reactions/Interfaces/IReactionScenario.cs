using System.Collections.Generic;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces
{
    public interface IReactionScenario
    {
        IEnumerable<Reaction> GetAvailableReactions(IReactor actor, IReactor opponent, ITargetedEvent evt);
    }
}