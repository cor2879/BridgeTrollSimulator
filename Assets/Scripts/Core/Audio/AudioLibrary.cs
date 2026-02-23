using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Audio
{
    [CreateAssetMenu(menuName = "BridgeTroll/Audio/Audio Library")]
    public class AudioLibrary : ScriptableObject
    {
        [Header("Music")]
        public AudioClip[] overworldThemes;
        public AudioClip[] combatThemes;

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