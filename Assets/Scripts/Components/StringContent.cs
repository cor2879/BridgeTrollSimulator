/*****************************************************************
 *  StringContent.cs
 *  
 *  copyright (c) 2024 Old Skool Games
 *****************************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Settings;

    public static class StringContent
    {
        public const string StringContentTable = "LocalizedStringConstants";

        public static string No
        {
            get => LocalizationSettings.StringDatabase.GetLocalizedString(
                    StringContentTable,
                    nameof(No),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "No";
        }

        public static string Yes
        {
            get => LocalizationSettings.StringDatabase.GetLocalizedString(
                    StringContentTable,
                    nameof(Yes),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "Yes";
        }

        public static string ExitDungeonConfirmation
        {
            get => LocalizationSettings.StringDatabase.GetLocalizedString(
                    StringContentTable,
                    nameof(ExitDungeonConfirmation),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "Really Exit Dungeon?"
        }
    }
}