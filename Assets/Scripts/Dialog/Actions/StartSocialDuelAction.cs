using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Actions
{
    [CreateAssetMenu(menuName = "BridgeTroll/DialogActions/Start Social Duel")]
    public class StartSocialDuelAction : DialogAction
    {
        public override void Execute(
            EntityController initiator,
            EntityController target)
        {
            GameEventBus.Publish(
                new SocialDuelStartedEvent(
                    initiator,
                    target,
                    Time.frameCount));
        }
    }
}