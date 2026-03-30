using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Trees;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Requirements
{
    [System.Serializable]
    public abstract class AbilityRequirement
    {
        public abstract bool IsMet(IActor actor, AbilityNode node);
    }
}