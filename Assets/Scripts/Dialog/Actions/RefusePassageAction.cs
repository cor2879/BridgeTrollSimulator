using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Actions
{
    [CreateAssetMenu(menuName = "BridgeTroll/DialogActions/Refuse Passage")]
    public class RefusePassageAction : DialogAction
    {
        public override void Execute(
            EntityController initiator,
            EntityController target)
        {
            GameEventBus.Publish(
                new RefusePassageEvent(
                    initiator,
                    target,
                    Time.frameCount));
        }
    }
}