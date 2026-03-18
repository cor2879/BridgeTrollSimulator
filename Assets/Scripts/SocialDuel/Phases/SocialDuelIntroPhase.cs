using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Phases
{
    public class SocialDuelIntroPhase : SocialDuelPhaseBase
    {
        public override void Enter(SocialDuelSystem system, SocialDuelContext context)
        {
            base.Enter(system, context);

            system.UI.ShowIntro(
                context.Player,
                context.Npc);
        }
    }
}