using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Actions
{
    [CreateAssetMenu(menuName = "BridgeTroll/DialogActions/Start Combat")]
    public class StartCombatAction : DialogAction
    {
        public override void Execute(EntityController initiator, EntityController target)
        {
            GameEventBus.Publish(
                new CombatStartedEvent(initiator, target, Time.frameCount));
        }
    }
}