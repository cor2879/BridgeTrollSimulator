using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Enums;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog
{
    [System.Serializable]
    public struct SpeechRequest
    {
        public string Text;
        public SpeechBubbleMode Mode;
        public float Duration;
    }
}