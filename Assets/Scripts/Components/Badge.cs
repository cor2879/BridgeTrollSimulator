namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using UnityEngine;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Settings;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Exceptions;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Platform;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Rules;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    public class Badge
    {
        private bool? earned;
        private bool? enabled;
        private DateTimeOffset? earnedDate;

        private string earnedPlayerPrefsKey;
        private string earnedDatePlayerPrefsKey;
        private string enabledPlayerPrefsKey;
        private string displayName;
        private string description;
        private string bonusDescription;
        private string currentCulture;

        private Badge()
        {
#if DEBUG

#endif
        }

        private Badge(IEnumerable<Rule> rules) : this()
        {
            foreach (var rule in rules)
            {
                this.AddRule(rule);
            }
        }

        public void StartListening()
        {
            foreach (var rule in this.Rules)
            {
                rule.OnUpdate += this.OnRuleUpdated;
            }
        }

        public void StopListening()
        {
            foreach (var rule in this.Rules)
            {
                rule.OnUpdate -= this.OnRuleUpdated;
            }
        }

        private List<Rule> InnerRules { get; } = new List<Rule>();

        public Rule[] Rules { get { return this.InnerRules.ToArray(); } }

        public string Name { get; private set; }

        public Color BackgroundColor { get; private set; } = Color.black;

        public string DisplayName
        {
            get => LocalizationSettings.StringDatabase.GetLocalizedString(
                    StringContent.StringContentTable,
                    this.displayName,
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            set => this.displayName = value;
        }

        public string Description
        {
            get => LocalizationSettings.StringDatabase.GetLocalizedString(
                    StringContent.StringContentTable,
                    this.description,
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            set => this.description = value;
        }

        public IEnumerable<Pair<string, string>> BonusDescriptionParameters
        {
            get;
            set;
        }

        public string BonusDescription
        {
            get
            {
                if ((this.BonusDescriptionParameters != null && this.BonusDescriptionParameters.Any()) &&
                    (string.IsNullOrEmpty(this.bonusDescription) ||
                    !string.Equals(LocalizationSettings.SelectedLocale.LocaleName, this.currentCulture, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var fallbackBehavior = FallbackBehavior.UseProjectSettings;

                    this.currentCulture = LocalizationSettings.SelectedLocale.LocaleName;
                    var stringBuilder = new StringBuilder(
                        LocalizationSettings.StringDatabase.GetLocalizedString(
                            StringContent.StringContentTable,
                            this.BonusDescriptionParameters.First().First,
                            locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)),
                            fallbackBehavior,
                            this.BonusDescriptionParameters.First().Second));

                    foreach (var pair in this.BonusDescriptionParameters.Skip(1))
                    {
                        stringBuilder.Append(
                            $",{LocalizationSettings.StringDatabase.GetLocalizedString(StringContent.StringContentTable, pair.First, locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)), fallbackBehavior, pair.Second)}");
                    }

                    this.bonusDescription = stringBuilder.ToString();
                }

                return this.bonusDescription;
            }
        }

        public string TextureName { get; private set; }

        private string EarnedPlayerPrefsKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.earnedPlayerPrefsKey))
                {
                    this.earnedPlayerPrefsKey = this.BuildPlayerPrefsKey(nameof(Badge.Earned));

                    if (!PlayerPrefsManager.IsKeyRegistered(this.earnedPlayerPrefsKey, PlayerPrefsDataType.Bool))
                    {
                        PlayerPrefsManager.RegisterKey(this.earnedPlayerPrefsKey, PlayerPrefsDataType.Bool);
                    }
                }

                return this.earnedPlayerPrefsKey;
            }
        }

        private string EarnedDatePlayerPrefsKey
        {
            get
            {
                if (this.earnedDatePlayerPrefsKey == null)
                {
                    this.earnedDatePlayerPrefsKey = this.BuildPlayerPrefsKey(nameof(Badge.EarnedDate));

                    if (!PlayerPrefsManager.IsKeyRegistered(this.earnedDatePlayerPrefsKey, PlayerPrefsDataType.DateTimeOffset))
                    {
                        PlayerPrefsManager.RegisterKey(this.earnedDatePlayerPrefsKey, PlayerPrefsDataType.DateTimeOffset);
                    }
                }

                return this.earnedDatePlayerPrefsKey;
            }
        }

        private string EnabledPlayerPrefsKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.enabledPlayerPrefsKey))
                {
                    this.enabledPlayerPrefsKey = this.BuildPlayerPrefsKey(nameof(Badge.Enabled));

                    if (!PlayerPrefsManager.IsKeyRegistered(this.enabledPlayerPrefsKey, PlayerPrefsDataType.Bool))
                    {
                        PlayerPrefsManager.RegisterKey(this.enabledPlayerPrefsKey, PlayerPrefsDataType.Bool);
                    }
                }

                return this.enabledPlayerPrefsKey;
            }
        }

        public bool Earned
        {
            get
            {
                if (this.earned == null)
                {
                    this.earned = PlayerPrefsManager.GetBool(this.EarnedPlayerPrefsKey);
                }

                return this.earned.Value;
            }

            set
            {
                if (this.earned != value)
                {
                    PlayerPrefsManager.SetProperty(this.EarnedPlayerPrefsKey, value);
                }

                var oldValue = this.earned;
                this.earned = value;

                if (oldValue != null && !oldValue.Value && value)
                {
                    this.BadgeEarned();
                }
            }
        }

        public DateTimeOffset? EarnedDate
        {
            get
            {
                if (this.earnedDate == null)
                {
                    this.earnedDate = PlayerPrefsManager.GetDateTimeOffset(this.EarnedDatePlayerPrefsKey);
                }

                return this.earnedDate;
            }

            set
            {
                PlayerPrefsManager.SetProperty(this.EarnedDatePlayerPrefsKey, value ?? default(DateTimeOffset));
                this.earnedDate = value;
            }
        }

        public bool Enabled
        {
            get
            {
                if (!this.Earned)
                {
                    return false;
                }

                if (this.enabled == null)
                {
                    this.enabled = PlayerPrefsManager.GetBool(this.EnabledPlayerPrefsKey, defaultValue: true);
                }

                return this.enabled.Value;
            }

            set
            {
                var oldValue = this.enabled;

                if (this.Earned)
                {
                    if (this.enabled != value)
                    {
                        PlayerPrefsManager.SetProperty(this.EnabledPlayerPrefsKey, value);
                    }

                    this.enabled = value;
                }
                else
                {
                    this.enabled = false;
                }

                if (this.enabled != oldValue)
                {
                    if (this.enabled.Value)
                    {
                        this.OnBadgeEnabled();
                    }
                    else
                    {
                        this.OnBadgeDisabled();
                    }
                }
            }
        }

        public Action OnEnabled { get; set; }

        public Action OnDisabled { get; set; }

        private void BadgeEarned()
        {
            this.OnBadgeEarned?.Invoke(this);
            this.EarnedDate = DateTime.UtcNow;
            PlatformManager.Instance.EarnBadge(this);
        }

        public Action<Badge> OnBadgeEarned;

        private Action InnerApply;

        public void Apply()
        {
            this.InnerApply?.Invoke();
        }

        /// <summary>
        /// Evaluates this instance.
        /// </summary>
        /// <returns></returns>
        public bool Evaluate()
        {
            return !this.Rules.Any() || this.InnerRules.TrueForAll(rule =>
            {
                var earned = rule.Evaluate();
                return earned;
            });
        }

        private void OnRuleUpdated()
        {
            if (!this.Earned)
            {
                this.Earned = this.Evaluate();
            }
        }

        public void OnBadgeEnabled()
        {
            this.OnEnabled?.Invoke();
        }

        public void OnBadgeDisabled()
        {
            this.OnDisabled?.Invoke();
        }

        public void AddRule(Rule rule)
        {
            this.InnerRules.Add(rule);
        }

        private string BuildPlayerPrefsKey(string propertyName)
        {
            return $"{nameof(Badge)}_{this.Name}_{propertyName}";
        }

        public void DeleteBadge()
        {
            PlayerPrefsManager.DeleteKey(this.EarnedDatePlayerPrefsKey);
            PlayerPrefsManager.DeleteKey(this.EarnedPlayerPrefsKey);
            PlayerPrefsManager.DeleteKey(this.EnabledPlayerPrefsKey);
        }

        public static Badge[] GetStaticBadges()
        {
            return new Badge[]
            {

            };
        }
    }
}
