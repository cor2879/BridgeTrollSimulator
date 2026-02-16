using System.Collections.Generic;

using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Conditions;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Actions
{
    public abstract class DialogAction : ScriptableObject
    {
        [SerializeField]
        private List<DialogCondition> conditions = new();

        public List<DialogCondition> Conditions => conditions;

        public virtual bool CanExecute(EntityController initiator, EntityController target)
        {
            foreach (var condition in Conditions)
            {
                if (!condition.IsMet(this, initiator, target))
                {
                    Debug.Log($"{this.ToString()}: condition failed");
                    return false;
                }
            }

            Debug.Log($"{this.ToString()}: condition met.");
            return true;
        }

        public abstract void Execute(
            EntityController initiator,
            EntityController target);
    }
}