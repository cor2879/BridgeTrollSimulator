using System.Collections;
using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Audio
{
    public class AudioSystem : MonoBehaviour
    {
        public static AudioSystem Instance { get; private set; }

        [Header("Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Library")]
        [SerializeField] private AudioLibrary library;

        [SerializeField, ReadOnly]
        private AudioClip currentMusic;
        [SerializeField, ReadOnly]
        private int musicResumeSample;

        public static AudioLibrary Library => Instance.library;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            GameEventBus.Subscribe<DamageTakenEvent>(OnDamage);
            GameEventBus.Subscribe<DefendEvent>(OnDefend);
            GameEventBus.Subscribe<SoundEffectEvent>(OnSoundEffect);
        }

        private void OnDamage(DamageTakenEvent evt)
        {
            PlaySFX(evt.IsCrit ? library.crit : library.attack);
        }

        private void OnDefend(DefendEvent evt)
        {
            PlaySFX(library.defend);    
        }

        private void OnSoundEffect(SoundEffectEvent evt)
        {
            PlaySFX(evt.Clip);
        }

        #region Music

        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (clip == null)
                return;

            currentMusic = clip;
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.timeSamples = 0;
            musicSource.Play();
        }

        public void PauseMusic()
        {
            musicResumeSample = musicSource.timeSamples;
            musicSource.Pause();
        }

        public void ResumeMusic()
        {
            musicSource.clip = currentMusic;
            musicSource.timeSamples = musicResumeSample;
            musicSource.Play();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

        public int MusicVolume
        {
            get => Mathf.RoundToInt(musicSource.volume * 100f);
            set => musicSource.volume = Mathf.Clamp01(value / 100f);
        }

        public void PlayOverworldMusic()
        {
            PlayMusic(library.overworldThemes[Random.Range(0, library.overworldThemes.Length)]);
        }

        public void PlayCombatMusic()
        {
            PlayMusic(library.combatThemes[Random.Range(0, library.combatThemes.Length)]);
        }

        #endregion

        #region SFX

        public int SfxVolume
        {
            get => Mathf.RoundToInt(sfxSource.volume * 100f);
            set => sfxSource.volume = Mathf.Clamp01(value / 100f);
        }

        public void PlaySFX(AudioClip clip, float volume = 1f)
        {
            if (clip == null)
                return;

            sfxSource.PlayOneShot(clip, volume);
        }
        
        public void PlayDistinctSFX(AudioClip clip)
        {
            if (clip == null)
            {
                return;
            }

            StartCoroutine(PlayDistinctRoutine(clip));
        }

        private IEnumerator PlayDistinctRoutine(AudioClip clip)
        {
            PauseMusic();
            sfxSource.PlayOneShot(clip);

            yield return new WaitForSeconds(clip.length);

            ResumeMusic();
        }

        #endregion
    }
}