using System.Collections.Generic;

using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Actions;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Conditions;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog
{
    [System.Serializable]
    public class DialogChoice
    {
        public string ChoiceText;
        public DialogNode NextNode;

        [SerializeField]
        private List<DialogAction> actions = new List<DialogAction>();

        public List<DialogAction> Actions => actions;
    }
}