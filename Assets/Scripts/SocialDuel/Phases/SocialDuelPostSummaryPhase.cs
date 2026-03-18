using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Phases
{
    public class SocialDuelPostSummaryPhase : SocialDuelPhaseBase
    {
        public override void Enter(SocialDuelSystem system, SocialDuelContext context)
        {
            base.Enter(system, context);

            system.ResolutionUI.Show(context.Resolution);
        }
    }
}