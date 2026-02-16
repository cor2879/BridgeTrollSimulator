using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Actions;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Conditions
{
    public abstract class DialogCondition : ScriptableObject
    {
        public abstract bool IsMet(
            DialogAction action,
            EntityController initiator,
            EntityController target
        );
    }
}