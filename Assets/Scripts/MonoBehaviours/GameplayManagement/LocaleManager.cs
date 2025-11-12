#pragma warning disable CS0649
/**************************************************
 *  LocaleManager.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours.GameplayManagement
{
    using System;
    using System.Collections;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Settings;
    using UnityEngine.UI;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Exceptions;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Interfaces;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Platform;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Rules;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;
    using UnityEngine.SocialPlatforms;

    public class LocaleManager : MonoBehaviour
    {
        private static LocaleManager instance;

        [SerializeField, ReadOnly]
        private string currentLocale;

        [SerializeField, ReadOnly]
        private string selectedLanguage;

        public string CurrentLocale
        {
            get => this.currentLocale;
            private set => this.currentLocale = value;
        }

        public string SelectedLanguage
        {
            get => this.selectedLanguage;
            private set => this.selectedLanguage = value;
        }

        public static LocaleManager Instance 
        { 
            get => instance; 
            private set => instance = value; 
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }

            if (LocalizationSettings.SelectedLocale == null || string.Equals(LocalizationSettings.SelectedLocale.LocaleName, "None", StringComparison.OrdinalIgnoreCase))
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)) ?? 
                    LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier("en"));
            }
        }

        private void Update()
        {
            this.CurrentLocale = LocalizationSettings.SelectedLocale.LocaleName;

            this.SelectedLanguage = Settings.SelectedLanguage.Name;

            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode));
        }
    }
}
