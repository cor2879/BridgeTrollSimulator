/**************************************************
 *  Settings.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using System;
    using UnityEngine;
    using UnityEngine.Localization.Settings;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;

    /// <summary>
    /// Defines the game settings.  Uses the <see cref="PlayerPrefs" /> class
    /// to persist the settings for multiple play sessions.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// The difficulty
        /// </summary>
        private static PlayerPrefsObjectProperty<Difficulty> difficulty = new PlayerPrefsObjectProperty<Difficulty>("difficulty", Difficulty.Default);

        /// <summary>
        /// The survival mode
        /// </summary>
        private static PlayerPrefsBoolProperty survivalMode = new PlayerPrefsBoolProperty("survivalMode");

        /// <summary>
        /// The play sound
        /// </summary>
        private static PlayerPrefsBoolProperty playSound = new PlayerPrefsBoolProperty("playSound", defaultValue: true);

        /// <summary>
        /// The enable vibration setting
        /// </summary>
        private static PlayerPrefsBoolProperty enableVibration = new PlayerPrefsBoolProperty("enableVibration", defaultValue: true);

        private static PlayerPrefsIntProperty musicVolume = new PlayerPrefsIntProperty("musicVolume", defaultValue: AudioManager.MaxVolume);

        private static PlayerPrefsIntProperty soundEffectVolume = new PlayerPrefsIntProperty("soundEffectVolume", defaultValue: AudioManager.MaxVolume);

        private static PlayerPrefsStringProperty selectedLanguage = new PlayerPrefsStringProperty("selectedLanguage", defaultValue: string.Empty);

        private static PlayerPrefsIntProperty menuStyle = new PlayerPrefsIntProperty("menuStyle", defaultValue: 1);

        /// <summary>
        /// Gets or sets the difficulty.
        /// </summary>
        /// <value>
        /// The difficulty.
        /// </value>
        public static Difficulty Difficulty
        {
            get
            {
                if (difficulty.Get() == null)
                {
                    Difficulty = Difficulty.Default;
                }

                return difficulty.Get();
            }

            set
            {
                difficulty.Set(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [survival mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [survival mode]; otherwise, <c>false</c>.
        /// </value>
        public static bool SurvivalMode
        {
            get
            {
                return survivalMode.Get();
            }

            set
            {
                survivalMode.Set(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [play sound].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [play sound]; otherwise, <c>false</c>.
        /// </value>
        public static bool PlaySound
        {
            get
            {
                return playSound.Get();
            }

            set
            {
                playSound.Set(value);

              /*  if (value && !GameManager.Instance.MusicManager.IsPlaying)
                {
                    GameManager.Instance.MusicManager.Play();
                }
                else if (!value && GameManager.Instance.MusicManager.IsPlaying)
                {
                    GameManager.Instance.MusicManager.Stop();
                } */
            }
        }

        public static int MusicVolume
        {
            get
            {
                return musicVolume.Get();
            }

            set
            {
                musicVolume.Set(value);

                // GameManager.Instance.MusicManager.Volume = value;
            }
        }

        public static int SoundEffectVolume
        {
            get
            {
                return soundEffectVolume.Get();
            }

            set
            {
                soundEffectVolume.Set(value);

                // GameManager.Instance.SoundEffectManager.Volume = value;
            }
        }

        public static bool EnableVibration
        {
            get
            {
                return false;
            }

            set
            {
                enableVibration.Set(value);
            }
        }

        public static SupportedLanguage SelectedLanguage
        {
            get
            {
                return SupportedLanguage.SupportedLanguages.TryGetValue(selectedLanguage.Get(), out var language) ? language :
                    SupportedLanguage.SupportedLanguages.TryGetValue(LocalizationSettings.SelectedLocale.LocaleName, out var currentLanguage) ? currentLanguage :
                        SupportedLanguage.SupportedLanguages["en"];
            }

            set => selectedLanguage.Set(value.CultureCode);
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public static void SaveSettings()
        {
            PlayerPrefsManager.SavePlayerPrefs();
        }
    }
}
