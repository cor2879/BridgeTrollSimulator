using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Actions
{
    [CreateAssetMenu(menuName = "BridgeTroll/DialogActions/Deduct Gold")]
    public class DeductGoldAction : DialogAction
    {
        public int Amount;

        public override void Execute(EntityController initiator, EntityController target)
        {
            // GameEventBus.Publish(
            //    new GoldDeductedEvent(initiator, target, Amount, Time.frameCount));
        }
    }
}