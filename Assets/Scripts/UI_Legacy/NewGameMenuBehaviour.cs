#pragma warning disable CS0649
/**************************************************
 *  NewGameMenuBehaviour.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;

    using Interface.Elements.Scripts;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;

    public class NewGameMenuBehaviour : BasePanel
    {
        [SerializeField]
        private ButtonUI backButton;

        [SerializeField]
        private OptionSelectorBehaviour dungeonTypeSelector;

        [SerializeField]
        private OptionSelectorBehaviour survivalModeSelector;

        [SerializeField]
        private ButtonUI nextButton;

        [SerializeField, ReadOnly]
        private ButtonsPanelBehaviour buttonsPanel;

        public ButtonUI BackButton
        {
            get
            {
                this.ValidateUnityEditorParameter(this.backButton, nameof(this.backButton));

                return this.backButton;
            }
        }

        public OptionSelectorBehaviour DungeonTypeSelector
        {
            get
            {
                this.ValidateUnityEditorParameter(this.dungeonTypeSelector, nameof(this.dungeonTypeSelector));

                return this.dungeonTypeSelector;
            }
        }

        public OptionSelectorBehaviour SurvivalModeSelector
        {
            get
            {
                this.ValidateUnityEditorParameter(this.survivalModeSelector, nameof(this.survivalModeSelector));

                return this.survivalModeSelector;
            }
        }

        public ButtonUI NextButton
        {
            get
            {
                this.ValidateUnityEditorParameter(this.nextButton, nameof(this.nextButton));

                return this.nextButton;
            }
        }

        public ButtonsPanelBehaviour ButtonsPanel
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

        private IList<Difficulty> Difficulties { get; set; }

        public void Exit()
        {
            Debug.Log("Exit");
            // TitleScreenBehaviour.Instance.MainButtonsPanel.Show();
            // TitleScreenBehaviour.Instance.MainButtonsPanel.GetComponent<ButtonsPanelBehaviour>().Activate();
            this.Hide();
        }

        public void Next()
        {
            Settings.SurvivalMode = SurvivalModeSelector.SelectedIndex != 0;
            Settings.Difficulty = Difficulty.GetDifficulty((DifficultySetting)this.DungeonTypeSelector.SelectedIndex);
            this.Hide();
            // TitleScreenBehaviour.Instance.StartGame();
        }

        private void Initialize()
        {
            if (this.DungeonTypeSelector.Options == null || !this.DungeonTypeSelector.Options.Any())
            {
                this.Difficulties = Difficulty.GetDifficulties();
                this.DungeonTypeSelector.Options = this.Difficulties.Select(d => d.DisplayName).ToArray();
                this.DungeonTypeSelector.SelectedIndex = (int)Settings.Difficulty.Setting;
            }

            if (this.SurvivalModeSelector.Options == null || !this.SurvivalModeSelector.Options.Any())
            {
                this.SurvivalModeSelector.Options = Constants.YesNoOptions;
                this.SurvivalModeSelector.SelectedIndex = Settings.SurvivalMode ? 1 : 0;
            }
        }

        private void Start()
        {
            this.BackButton.onClick.AddListener(this.Exit);
            this.NextButton.onClick.AddListener(this.Next);
        }

        public void Show()
        {
            this.Initialize();
            this.BackButton.SelectableState.Select();
            this.ButtonsPanel.Activate();
            base.Show(CanvasSide.Centre);
        }

        public void Hide()
        {
            this.ButtonsPanel.Deactivate();
            base.Hide(CanvasSide.Centre);
        }

        public void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(NewGameMenuBehaviour));
        }
    }
}
