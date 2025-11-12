/**************************************************
 *  AudioManager.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    using UnityEngine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    /// <summary>
    /// Singleton class which manages all audio features of a game.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public const int MaxVolume = 100;
        public const int MinVolume = 0;

        /// <summary>
        /// The current background music
        /// </summary>
        [SerializeField, ReadOnly]
        private AudioClip backgroundMusic;

        /// <summary>
        /// The audio clips
        /// </summary>
        private static Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

        /// <summary>
        /// The audio source
        /// </summary>
        private AudioSource audioSource;

        /// <summary>
        /// Gets the current background music.
        /// </summary>
        /// <value>
        /// The background music.
        /// </value>
        public AudioClip BackgroundMusic
        {
            get { return this.backgroundMusic; }
            private set { this.backgroundMusic = value; }
        }

        /// <summary>
        /// Gets the audio source.
        /// </summary>
        /// <value>
        /// The audio source.
        /// </value>
        private AudioSource AudioSource
        {
            get
            {
                if (this.audioSource == null)
                {
                    this.audioSource = this.GetComponent<AudioSource>();
                }

                return this.audioSource;
            }
        }

        public int Volume
        {
            get { return Mathf.FloorToInt(this.AudioSource.volume * 100); }
            set { this.AudioSource.volume = ((float)value) / 100; }
        }

        public bool IsPlaying
        {
            get { return this.AudioSource.isPlaying; }
        }

        /// <summary>
        /// Defines code that will execute during the Start event of the object's lifetime.
        /// </summary>
        public void Start()
        {
            this.LoadAudioClips();
        }

        /// <summary>
        /// Plays whatever sound clip is currently assigned as the default in the <see cref="AudioSource" />.
        /// </summary>
        public void Play()
        {
            if (!Settings.PlaySound)
            {
                return;
            }

            this.AudioSource.Play();
        }

        public bool IsPlayingAudioClip(string name)
        {
            return this.BackgroundMusic.Equals(this.GetAudioClip(name));
        }

        /// <summary>
        /// Stops the the <see cref="AudioSource" />.
        /// </summary>
        public void Stop()
        {
            this.AudioSource.Stop();
        }

        /// <summary>
        /// Gets the audio clip associated with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public AudioClip GetAudioClip(string name)
        {
            if (AudioManager.audioClips.ContainsKey(name))
            {
                return AudioManager.audioClips[name];
            }

            return null;
        }

        /// <summary>
        /// Preloads all of the known audio clips into memory so that they are ready for use.
        /// </summary>
        private void LoadAudioClips()
        {
            var soundClipsType = typeof(SoundClips);

            foreach (var field in soundClipsType.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                AddAudioClip(field.GetValue(null)?.ToString());
            }
        }

        /// <summary>
        /// Adds an <see cref="AudioClip" /> to the AudioClip collection.
        /// </summary>
        /// <param name="name">The name of the audio clip to add.</param>
        private void AddAudioClip(string name)
        {
            if (!AudioManager.audioClips.ContainsKey(name))
            {
                AudioManager.audioClips.Add(name, Resources.Load<AudioClip>(name));
            }
        }

        public void PlayAudioOnceAtPoint(string name, Vector3 position)
        {
            if (!Settings.PlaySound)
            {
                return;
            }

            if (!AudioManager.audioClips.ContainsKey(name))
            {
                this.AddAudioClip(name);
            }

            AudioSource.PlayClipAtPoint(AudioManager.audioClips[name], position);
        }

        /// <summary>
        /// Plays an <see cref="AudioClip" /> once.
        /// </summary>
        /// <param name="name">The name of the audio clip.</param>
        public void PlayAudioOnce(string name)
        {
            if (!Settings.PlaySound)
            {
                return;
            }

            if (!AudioManager.audioClips.ContainsKey(name))
            {
                this.AddAudioClip(name);
            }

            this.AudioSource.PlayOneShot(AudioManager.audioClips[name]);
        }

        public void PlayAudioOnce(AudioClip audioClip)
        {
            if (!Settings.PlaySound)
            {
                return;
            }

            this.AudioSource.PlayOneShot(audioClip);
        }

        public void PlayAudioOnceAtVolume(string name, float volume)
        {
            if (!Settings.PlaySound)
            {
                return;
            }

            if (!AudioManager.audioClips.ContainsKey(name))
            {
                this.AddAudioClip(name);
            }

            this.AudioSource.PlayOneShot(AudioManager.audioClips[name], volume);
        }

        /// <summary>
        /// Sets the background music.  Background music plays constantly and loops repeatedly until the audio source
        /// is stopped or paused.
        /// </summary>
        /// <param name="name">The name of the <see cref="AudioClip" /> to set as the background music..</param>
        public void SetBackgroundMusic(string name)
        {
            if (!AudioManager.audioClips.ContainsKey(name))
            {
                this.AddAudioClip(name);
            }

            this.BackgroundMusic = AudioManager.audioClips[name];
            this.AudioSource.loop = true;
            this.AudioSource.clip = this.BackgroundMusic;
            this.AudioSource.timeSamples = 0;

            this.Play();
        }

        /// <summary>
        /// Pauses any existing background music and plays the specified sound clip once.  After
        /// the sound clip has completed, the original background music is restored.
        /// </summary>
        /// <param name="name">The sound clip name.</param>
        public void PlayDistinctAudioOnce(string name)
        {
            if (!Settings.PlaySound)
            {
                return;
            }

            if (!AudioManager.audioClips.ContainsKey(name))
            {
                this.AddAudioClip(name);
            }

            this.AudioSource.Pause();

            var audioRestoreSettings = new AudioRestoreSettings()
            {
                Duration = AudioManager.audioClips[name].length * 0.8f,
                TimeSamples = this.AudioSource.timeSamples
            };

            this.AudioSource.clip = null;
            this.AudioSource.PlayOneShot(AudioManager.audioClips[name]);

            StartCoroutine(nameof(this.RestoreBackgroundMusicAfterWait), audioRestoreSettings);
        }

        /// <summary>
        /// Restores the background music after wait waiting for any other audio clips to finish playing.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        private IEnumerator RestoreBackgroundMusicAfterWait(AudioRestoreSettings settings)
        {
            while (this.AudioSource.isPlaying)
            {
                if (this.AudioSource.clip == this.BackgroundMusic)
                {
                    yield break;
                }

                yield return new WaitForFixedUpdate();
            }

            this.AudioSource.clip = this.BackgroundMusic;
            this.AudioSource.timeSamples = settings.TimeSamples;
            this.AudioSource.Play();
        }

        /// <summary>
        /// Defines settings for restoring the background music.
        /// </summary>
        private struct AudioRestoreSettings
        {
            /// <summary>
            /// The duration
            /// </summary>
            public float Duration;

            /// <summary>
            /// The time samples
            /// </summary>
            public int TimeSamples;
        }
    }
}
