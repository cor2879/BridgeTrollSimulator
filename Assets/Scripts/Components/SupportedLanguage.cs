namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using System;
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine.Localization;

    using static LanguageCodes;

    public class SupportedLanguage : IEquatable<SupportedLanguage>
    {
        public SupportedLanguage(string cultureCode, string name)
        {
            this.CultureCode = cultureCode;
            this.Name = name;
        }

        public string CultureCode { get; private set; }

        public string Name { get; private set; }

        public static readonly Dictionary<string, SupportedLanguage> SupportedLanguages = new Dictionary<string, SupportedLanguage>
        {

            { English,  new SupportedLanguage(English, "English") },
            { French, new SupportedLanguage(French, "Français") },
            { German, new SupportedLanguage(German, "Deutsch") },
            { Korean, new SupportedLanguage(Korean, "???") },
            { Polish, new SupportedLanguage(Polish, "Polski") },
            { Portuguese, new SupportedLanguage(Portuguese, "Português") },
            { Russian, new SupportedLanguage(Russian, "???????") },
            { Spanish, new SupportedLanguage(Spanish, "Español") },
            { Vietnamese, new SupportedLanguage(Vietnamese, "Vi?t") }
        };

        public override bool Equals(object obj)
        {
            return this.Equals(obj as SupportedLanguage);
        }

        public bool Equals(SupportedLanguage other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return string.Equals(this.CultureCode, other.CultureCode, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return (this.CultureCode ?? string.Empty).GetHashCode();
        }

        public static implicit operator LocaleIdentifier(SupportedLanguage sl)
        {
            return new LocaleIdentifier(sl.CultureCode);
        }
    }
}
