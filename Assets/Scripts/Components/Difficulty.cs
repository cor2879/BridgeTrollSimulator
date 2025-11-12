/**************************************************
 *  Difficulty.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine.Localization;
    using UnityEngine.Localization.Settings;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    /// <summary>
    /// Defines a range of values use for describing the possible difficulty settings.
    /// </summary>
    public enum DifficultySetting
    {
        /*
        /// <summary>
        /// The tutorial setting
        /// </summary>
        Tutorial = 0,
        */

        /// <summary>
        /// The beginner setting
        /// </summary>
        Beginner = 0,

        /// <summary>
        /// The easy setting
        /// </summary>
        Easy,

        /// <summary>
        /// The normal setting
        /// </summary>
        Normal,

        /// <summary>
        /// The hard setting
        /// </summary>
        Hard,

        /// <summary>
        /// The expert setting
        /// </summary>
        Expert,

        /// <summary>
        /// The Hunter and Prey setting
        /// </summary>
        HunterAndPrey,

        /// <summary>
        /// The custom setting
        /// </summary>
        Custom
#if DEBUG
        /// <summary>
        /// The empty test
        /// </summary>
        , Empty

#endif
    };

    /// <summary>
    /// Defines a structure for specifying the parameters used to build a dungeon
    /// </summary>
    public class Difficulty
    {
        public const string CustomModePlayerPrefsKey = "CustomMode";

        /// <summary>
        /// The pre-defined difficulty settings.
        /// </summary>
        private static Dictionary<DifficultySetting, Difficulty> difficulties = GetDifficultiesInternal();

        public static readonly Difficulty Default = difficulties[DifficultySetting.Beginner];

        private static Dictionary<DifficultySetting, Difficulty> GetDifficultiesInternal()
        {
            var difficulties = new Dictionary<DifficultySetting, Difficulty>()
            {
                {
                    DifficultySetting.Beginner,
                    new Difficulty()
                    {
                        Setting = DifficultySetting.Beginner,
                        DisplayName = "Beginner"
                    }
                },
                {
                    DifficultySetting.Easy,
                    new Difficulty()
                    {
                        Setting = DifficultySetting.Easy,
                        DisplayName = "Easy"
                    }
                },
                {
                    DifficultySetting.Normal,
                    new Difficulty()
                    {
                        Setting = DifficultySetting.Normal,
                        DisplayName = "Normal"
                    }
                },
                {
                    DifficultySetting.Hard,
                    new Difficulty()
                    {
                        Setting = DifficultySetting.Hard,
                        DisplayName = "Hard"
                    }
                },
                {
                    DifficultySetting.Expert,
                    new Difficulty()
                    {
                        Setting = DifficultySetting.Expert,
                        DisplayName = "Expert"
                    }
                },
                {
                    DifficultySetting.HunterAndPrey,
                    new Difficulty()
                    {
                        Setting = DifficultySetting.HunterAndPrey,
                        DisplayName = "HunterAndPrey"
                    }
                }
#if DEBUG
                ,
                {
                    DifficultySetting.Empty,
                    new Difficulty()
                    {
                        Setting = DifficultySetting.Empty,
                        DisplayName = "Empty"
                    }
                }
#endif
            };

            if (IsCustomModeEnabled())
            {
                difficulties.Add(DifficultySetting.Custom, GetCustomDifficulty());
            }

            return difficulties;
        }

        private string displayName;

        /// <summary>
        /// Prevents a default instance of the <see cref="Difficulty"/> class from being created.
        /// </summary>
        public Difficulty() { }

        /// <summary>
        /// Gets the setting.
        /// </summary>
        /// <value>
        /// The setting.
        /// </value>
        public DifficultySetting Setting { get; set; }

        public string LocalizedDisplayName
        {
            get => LocalizationSettings.StringDatabase.GetLocalizedString(
                    StringContent.StringContentTable,
                    this.displayName,
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
        }

        public string DisplayName
        {
            get => this.displayName;
            set => this.displayName = value;
        }

        public bool IsCustomMode()
        {
            return this.Setting == DifficultySetting.Custom;
        }

        /// <summary>
        /// Gets the difficulty.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns></returns>
        public static Difficulty GetDifficulty(DifficultySetting setting)
        {
            if (!difficulties.ContainsKey(setting))
            {
                return null;
            }

            return difficulties[setting];
        }

        public static void SetCustomDifficulty(Difficulty customDifficulty)
        {
            if (!difficulties.ContainsKey(DifficultySetting.Custom) || !(customDifficulty.Setting == DifficultySetting.Custom))
            {
                return;
            }

            difficulties[DifficultySetting.Custom] = customDifficulty;
        }

        /// <summary>
        /// Gets the difficulties.
        /// </summary>
        /// <returns></returns>
        public static Difficulty[] GetDifficulties()
        {
            if (IsCustomModeEnabled())
            {
                EnableCustomDifficulty();
            }

            return difficulties.Values.OrderBy(difficulty => difficulty.Setting).ToArray();
        }

        public static bool IsCustomModeEnabled()
        {
            var dungeonCrawlerBadge = Badge.GetStaticBadges().Where(badge => badge.Name == "DungeonCrawler").FirstOrDefault();

            return true || dungeonCrawlerBadge != null && dungeonCrawlerBadge.Enabled;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Setting.ToString();
        }

        private static Difficulty GetCustomDifficulty()
        {
            Difficulty customDifficulty = null;

            if (!PlayerPrefsManager.IsKeyRegistered(CustomModePlayerPrefsKey, PlayerPrefsDataType.Json))
            {
                PlayerPrefsManager.RegisterKey(CustomModePlayerPrefsKey, PlayerPrefsDataType.Json);
            }

            customDifficulty = PlayerPrefsManager.GetData<Difficulty>(CustomModePlayerPrefsKey);

            if (customDifficulty == null || customDifficulty.Setting != DifficultySetting.Custom)
            {
                customDifficulty = new Difficulty() { Setting = DifficultySetting.Custom };
            }

            customDifficulty.DisplayName = "Custom";

            return customDifficulty;
        }

        public static void SaveCustomDifficulty()
        {
            if (!difficulties.ContainsKey(DifficultySetting.Custom))
            {
                return;
            }

            SaveCustomDifficultyUnsafe();
        }

        private static void SaveCustomDifficultyUnsafe()
        {
            var customDifficulty = difficulties[DifficultySetting.Custom];

            PlayerPrefsManager.SetDataProperty(CustomModePlayerPrefsKey, customDifficulty);
        }

        public static void EnableCustomDifficulty()
        {
            if (!difficulties.ContainsKey(DifficultySetting.Custom))
            {
                difficulties.Add(DifficultySetting.Custom, GetCustomDifficulty());
            }
        }

        public static void DisableCustomDifficulty()
        {
            if (difficulties.ContainsKey(DifficultySetting.Custom))
            {
                SaveCustomDifficultyUnsafe();
                difficulties.Remove(DifficultySetting.Custom);
                Settings.Difficulty = Difficulty.Default;
            }
        }
    }

}
