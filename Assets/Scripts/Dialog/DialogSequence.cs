using System.Collections.Generic;
using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog
{
    [CreateAssetMenu(menuName = "BridgeTroll/Dialog Sequence")]
    public class DialogSequence : ScriptableObject, IEventSource
    {
        public string SourceName => "DialogSequence";
        public GameSystemType SystemType => GameSystemType.Dialog;
        public List<DialogLine> Lines = new();
    }
}