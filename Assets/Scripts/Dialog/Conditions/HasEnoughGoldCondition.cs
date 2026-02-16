using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Actions;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Conditions
{
    [CreateAssetMenu(menuName = "BridgeTroll/DialogConditions/Has Enough Gold")]
    public class HasEnoughGoldCondition : DialogCondition
    {
        public override bool IsMet(
            DialogAction action,
            EntityController initiator,
            EntityController target)
        {
            if (initiator == null)
            {
                Debug.Log($"{nameof(HasEnoughGoldCondition)} initiator is null");
                return false;
            }

            var goldAction = action as DeductGoldAction;

            if (goldAction != null)
            {
                Debug.Log($"{nameof(HasEnoughGoldCondition)}: Target Gold: {target.Gold} | Gold Action Amount: {goldAction.Amount}");
                return target.Gold >= goldAction.Amount;
            }

            Debug.Log($"{nameof(HasEnoughGoldCondition)} goldAction is null!");
            return false;
        }
    }
}