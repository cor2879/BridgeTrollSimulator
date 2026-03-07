using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Audio
{
    [CreateAssetMenu(menuName = "BridgeTroll/Audio/Audio Library")]
    public class AudioLibrary : ScriptableObject
    {
        [Header("Music")]
        public AudioClip[] overworldThemes;
        public AudioClip[] combatThemes;
        public AudioClip[] bossFightThemes;
        public AudioClip[] defeatThemes;
        public AudioClip[] victoryThemes;
        public AudioClip[] moodThemes;
        public AudioClip[] whimsicalThemes;

        [Header("Combat SFX")]
        public AudioClip attack;
        public AudioClip armorBreak;
        public AudioClip defend;
        public AudioClip crit;

        [Header("World Sounds")]
        public AudioClip coins;

        [Header("UI")]
        public AudioClip buttonClick;
    }
}