#pragma warning disable CS0649
/**************************************************
 *  SettingsPanelBehaviourBase.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using Uui = UnityEngine.UI;
    using UnityEngine.UIElements;

    using BeautifulInterface = Interface.Elements.Scripts;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Exceptions;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours.GameplayManagement;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Interfaces;
    using Interface.Demo.Scripts;


    /// <summary>
    /// Defines the behaviours for the settings panel
    /// </summary>
    /// <seealso cref="OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours.UIHelperBehaviour" />
    [RequireComponent(typeof(PanelBehaviour))]
    public class SettingsPanelBehaviourBase : UIHelperBehaviour
    {
        private bool lockInput;

        private bool initialized = false;

        /// <summary>
        /// The play sound toggle
        /// </summary>
        [SerializeField]
        protected Uui.Toggle playSoundToggle;

        /// <summary>
        /// The save button
        /// </summary>
        [SerializeField]
        protected Uui.Button saveButton;

        /// <summary>
        /// The cancel button
        /// </summary>
        [SerializeField]
        protected Uui.Button cancelButton;

        [SerializeField]
        protected UISliderPanelBehaviour musicVolumeSliderPanel;

        [SerializeField]
        protected UISliderPanelBehaviour soundEffectVolumeSliderPanel;

        [SerializeField]
        protected LanguageSelectorBehaviour languageSelector;

        [SerializeField]
        protected MusicSelectorBehaviour musicSelector;

        [SerializeField, ReadOnly]
        private GameObject selectedGameObject;

        protected ButtonsPanelBehaviour buttonsPanel;

        protected PanelBehaviour panel;

        protected UISliderPanelBehaviour MusicVolumeSliderPanel
        {
            get
            {
                if (this.musicVolumeSliderPanel == null)
                {
                    throw new UIException($"The parameter {nameof(this.musicVolumeSliderPanel)} needs to be set in the Unity Editor.");
                }

                return this.musicVolumeSliderPanel;
            }
        }

        protected UISliderPanelBehaviour SoundEffectVolumeSliderPanel
        {
            get
            {
                if (this.soundEffectVolumeSliderPanel == null)
                {
                    throw new UIException($"The parameter {nameof(this.soundEffectVolumeSliderPanel)} needs to be set in the Unity Editor.");
                }

                return this.soundEffectVolumeSliderPanel;
            }
        }

        protected LanguageSelectorBehaviour LanguageSelector
        {
            get
            {
                if (this.languageSelector == null)
                {
                    throw new UIException($"The parameter {nameof(this.languageSelector)} needs to be set in the Unity Editor.");
                }

                return this.languageSelector;
            }
        }

        protected MusicSelectorBehaviour MusicSelector
        {
            get
            {
                if (this.musicSelector == null)
                {
                    throw new UIException($"The parameter {nameof(this.musicSelector)} needs to be set in the Unity Editor.");
                }

                return this.musicSelector;
            }
        }

        protected ButtonsPanelBehaviour ButtonsPanel
        {
            get
            {
                if (this.buttonsPanel == null)
                {
                    this.buttonsPanel = this.GetComponent<ButtonsPanelBehaviour>();
                }

                return this.buttonsPanel;
            }
        }

        protected Uui.Toggle PlaySoundToggle
        {
            get
            {
                this.ValidateUnityEditorParameter(this.playSoundToggle, nameof(this.playSoundToggle));

                return this.playSoundToggle;
            }
        }

        protected PanelBehaviour Panel
        {
            get
            {
                if (this.panel == null)
                {
                    this.panel = this.GetComponent<PanelBehaviour>();
                }

                return this.panel;
            }
        }

        private GameObject SelectedGameObject
        {
            get => this.selectedGameObject; set => this.selectedGameObject = value;
        }

        protected virtual void Save()
        {
            Settings.EnableVibration = false;
            Settings.PlaySound = this.PlaySoundToggle.isOn;
            Settings.MusicVolume = this.MusicVolumeSliderPanel.Value;
            Settings.SoundEffectVolume = this.SoundEffectVolumeSliderPanel.Value;
            Settings.SelectedLanguage = this.LanguageSelector.GetSelectedLanguage();
            SoundClips.CurrentBGM = SoundClips.Playlist[MusicSelector.SelectedIndex];
            Settings.SaveSettings();

            if (GameplayMenuManagerBehaviour.Instance != null)
            {
                GameplayMenuManagerBehaviour.ClearMainWindowText();
                // PlayerBehaviour.Instance.CurrentRoom.GetWarnings();
            }

            this.Close();
        }

        protected virtual void Cancel()
        {
            this.ResetVolume();

            var musicManager = this.GetMusicManager();

            if (!musicManager.IsPlayingAudioClip(SoundClips.CurrentBGM))
            {
                musicManager.SetBackgroundMusic(SoundClips.CurrentBGM);
            }

            this.Close();
        }

        public void Close()
        {
            /*
            if (TitleScreenBehaviour.Instance != null)
            {
                TitleScreenBehaviour.Instance.TitlePanel.Enable();
                TitleScreenBehaviour.Instance.MoreButtonsPanel.Show();
            }
            */
            this.Disable();
        }

        public void Awake()
        {
            this.OnEnabled.AddListener(this.Enabled);
        }

        /// <summary>
        /// Executes during the Start event of the GameObject life cycle
        /// </summary>
        public virtual void Start()
        {
            this.PopulatePlaySoundToggle();
            this.PopulateMusicVolumeSliderPanel();
            this.PopulateSoundEffectVolumeSliderPanel();

            this.saveButton.onClick.AddListener(this.Save);
            this.cancelButton.onClick.AddListener(this.Cancel);
            this.MusicVolumeSliderPanel.OnValueChanged.AddListener(this.OnMusicVolumeSliderValueChanged);
            this.SoundEffectVolumeSliderPanel.OnValueChanged.AddListener(this.OnSoundEffectVolumeSliderValueChanged);
            this.MusicSelector.OptionChanged.AddListener(this.OnMusicSelectionChanged);
        }

        private void Update()
        {
            this.SelectedGameObject = EventSystem.current.currentSelectedGameObject;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.PauseAction = true;
            }

            if (!this.lockInput)
            {
                if (InputExtension.IsCancelPressed())
                {
                    StartCoroutine(
                        nameof(this.WaitForPredicateToBeFalseThenDoAction),
                        new WaitAction(
                            () => InputExtension.IsCancelPressed(),
                            () =>
                            {
                                this.Cancel();
                            }));
                }
            }
        }

        private void Initialize()
        {
            if (!this.initialized)
            {
                this.LanguageSelector.Initialize();
                this.MusicSelector.Initialize();
                this.initialized = true;
            }
        }

        protected virtual void Enabled()
        {
            if (this.ButtonsPanel != null)
            {
                this.ButtonsPanel.DefaultButton.Select();
            }
        }

        private void ResetVolume()
        {
            this.PopulateMusicVolumeSliderPanel();
            this.PopulateSoundEffectVolumeSliderPanel();
        }

        private void OnMusicVolumeSliderValueChanged(float value)
        {
            this.GetMusicManager().Volume = (int)value;
        }

        private void OnSoundEffectVolumeSliderValueChanged(float value)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SoundEffectManager.Volume = (int)value;
            }
        }

        private void OnMusicSelectionChanged()
        {
            var musicManager = this.GetMusicManager();
            
            if (!musicManager.IsPlayingAudioClip(SoundClips.Playlist[this.MusicSelector.SelectedIndex]))
            {
                musicManager.SetBackgroundMusic(SoundClips.Playlist[this.MusicSelector.SelectedIndex]);
            }
        }

        private void PopulateMusicVolumeSliderPanel()
        {
            this.MusicVolumeSliderPanel.Value = Settings.MusicVolume;
        }

        private void PopulateSoundEffectVolumeSliderPanel()
        {
            this.SoundEffectVolumeSliderPanel.Value = Settings.SoundEffectVolume;
        }

        /// <summary>
        /// Populates the play sound toggle.
        /// </summary>
        private void PopulatePlaySoundToggle()
        {
            this.PlaySoundToggle.isOn = Settings.PlaySound;
        }

        /// <summary>
        /// Enables this instance.
        /// </summary>
        public override void Enable()
        {
            if (this.OnEnabled != null)
            {
                this.OnEnabled.Invoke();
            }

            base.Enable();
            this.Initialize();
            this.MusicSelector.SelectedIndex = Array.IndexOf(SoundClips.PlaylistFriendlyNames, SoundClips.CurrentBGM.Split('/').Last());
            this.Panel.Show();
        }

        /// <summary>
        /// Disables this instance.
        /// </summary>
        public override void Disable()
        {
            if (this.OnDisabled != null)
            {
                this.OnDisabled.Invoke();
            }

            base.Disable();
        }

        private AudioManager GetMusicManager()
        {
            return GameManager.Instance.MusicManager;
        }

        public void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(SettingsPanelBehaviourBase));
        }
    }
}
