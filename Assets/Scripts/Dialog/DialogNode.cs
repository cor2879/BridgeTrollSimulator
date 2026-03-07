using System;
using System.Collections.Generic;

using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog
{
    [CreateAssetMenu(menuName = "BridgeTroll/Dialog Node")]
    public class DialogNode : ScriptableObject
    {
        public DialogSpeakerRole SpeakerRole;

        [TextArea(2, 5)]
        public string Text;

        public List<DialogChoice> Choices = new List<DialogChoice>();
    }
}