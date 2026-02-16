using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog
{
    [System.Serializable]
    public class DialogLine
    {
        public string Speaker;
        [TextArea(2, 5)]
        public string Text;
    }
}