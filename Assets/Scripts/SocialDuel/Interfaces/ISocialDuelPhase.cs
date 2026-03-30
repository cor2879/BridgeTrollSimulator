using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Interfaces
{
    public interface ISocialDuelPhase
    {
        void Enter(SocialDuelSystem system, SocialDuelContext context);
        void Exit();
        void OnAbilityChosen(SocialAbility ability);
        void OnAdvance();
    }
}