#pragma warning disable CS0649
/**************************************************
 *  OptionSelectorBehaviour.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Settings;
    using UnityEngine.UI;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;

    using Interface.Elements.Scripts;

    [RequireComponent(typeof(Selectable))]
    [RequireComponent(typeof(SelectableStateBehaviour))]
    public class OptionSelectorBehaviour : UIHelperBehaviour, ISelectHandler
    {
        [SerializeField]
        private TextUI displayTextUI;

        [SerializeField]
        private ButtonUI nextItemButton;

        [SerializeField]
        private ButtonUI previousItemButton;

        [SerializeField, ReadOnly]
        private ButtonUI selectedButton;

        [SerializeField, ReadOnly]
        private bool selected;

        [SerializeField, ReadOnly]
        private Selectable selectable;

        [SerializeField, ReadOnly]
        private SelectableStateBehaviour selectableState;

        [SerializeField]
        private bool localizeText = true;

        private IList<string> options = new string[0];

        private int index = 0;

        [SerializeField, ReadOnly]
        private bool lockInput = false;

        public TextUI DisplayTextUI
        {
            get
            {
                this.ValidateUnityEditorParameter(this.displayTextUI, nameof(this.displayTextUI));

                return this.displayTextUI;
            }
        }

        public ButtonUI PreviousItemButton
        {
            get
            {
                this.ValidateUnityEditorParameter(this.previousItemButton, nameof(this.previousItemButton));

                return this.previousItemButton;
            }
        }

        public ButtonUI NextItemButton
        {
            get
            {
                this.ValidateUnityEditorParameter(this.nextItemButton, nameof(this.nextItemButton));

                return this.nextItemButton;
            }
        }

        public Selectable Selectable
        {
            get
            {
                if (this.selectable == null)
                {
                    this.selectable = this.GetComponent<Selectable>();
                }

                return this.selectable;
            }
        }

        public SelectableStateBehaviour SelectableState
        {
            get
            {
                if (this.selectableState == null)
                {
                    this.selectableState = this.GetComponent<SelectableStateBehaviour>();
                }

                return this.selectableState;
            }
        }

        public ButtonUI SelectedButton { get => this.selectedButton; private set => this.selectedButton = value; }

        public ButtonsPanelBehaviour ButtonsPanel { get => this.SelectableState.ButtonsPanel; set => this.SelectableState.ButtonsPanel = value; }

        public bool IsSelected { get => this.SelectableState.IsSelected; }

        public bool ChildButtonIsSelected { get => this.PreviousItemButton.IsSelected || this.NextItemButton.IsSelected; }

        public IList<string> Options
        {
            get => this.options;
            set
            {
                this.options = value ?? new string[0];
                this.UpdateDisplayText();
                //this.OptionChanged?.Invoke();
            }
        }

        public int SelectedIndex
        {
            get => this.index;
            set
            {
                this.index = value >= this.Options.Count ? 0 : value < 0 && this.Options.Any() ? this.Options.Count - 1 : value;
                this.UpdateDisplayText();
                this.OptionChanged?.Invoke();
            }
        }

        public bool LocalizeText { get => this.localizeText; set => this.localizeText = value; }

        public UnityEvent OptionChanged { get; } = new UnityEvent();

        public string SelectedValue { get => this.Options.Any() ? this.Options[this.SelectedIndex] : "No Data"; }

        protected virtual void Start()
        {
            this.PreviousItemButton.onClick.AddListener(this.OnPreviousItemButtonPressed);
            this.PreviousItemButton.MaintainHighlightWhenClicked = true;
            this.PreviousItemButton.ButtonsPanel = this.ButtonsPanel;
            this.PreviousItemButton.navigation = this.Selectable.navigation;
            this.NextItemButton.onClick.AddListener(this.OnNextItemButtonPressed);
            this.NextItemButton.MaintainHighlightWhenClicked = true;
            this.NextItemButton.ButtonsPanel = this.ButtonsPanel;
            this.NextItemButton.navigation = this.Selectable.navigation;
        }

        protected virtual void Update()
        {
            if (this.ButtonsPanel == null)
            {
                return;
            }

            if (!this.ButtonsPanel.Active || this.lockInput)
            {
                return;
            }
            else if (this.IsSelected || this.ChildButtonIsSelected)
            {
                // Handle Input
                if (InputExtension.IsRightPressed())
                {
                    this.lockInput = true;
                    StartCoroutine(
                        nameof(this.WaitForPredicateToBeFalseThenDoAction),
                        new WaitAction(
                            () => InputExtension.IsRightPressed(),
                            () =>
                            {
                                this.NextItemButton.SelectableState.Select();
                                this.SelectedButton = this.NextItemButton;
                                this.lockInput = false;
                            }));
                }

                if (InputExtension.IsLeftPressed())
                {
                    this.lockInput = true;
                    StartCoroutine(
                        nameof(this.WaitForPredicateToBeFalseThenDoAction),
                        new WaitAction(
                            () => InputExtension.IsLeftPressed(),
                            () =>
                            {
                                this.PreviousItemButton.SelectableState.Select();
                                this.SelectedButton = this.PreviousItemButton;
                                this.lockInput = false;
                            }));
                }
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            this.lockInput = true;
            this.SelectableState.Select();
            StartCoroutine(
                nameof(this.WaitForDurationThenDoAction),
                new WaitDuration(
                    0.15f,
                    () =>
                    {
                        this.PreviousItemButton.SelectableState.Select();
                        this.SelectedButton = this.PreviousItemButton;
                        this.lockInput = false;
                    }));

        }

        public virtual void Initialize() { }

        public void OnDeselect(BaseEventData eventData)
        {

        }

        private void UpdateDisplayText()
        {
            if (this.DisplayTextUI.IsActive())
            {
                try
                {
                    this.DisplayTextUI.text = this.LocalizeText ?
                        LocalizationSettings.StringDatabase.GetLocalizedString(
                            StringContent.StringContentTable,
                            this.SelectedValue,
                            locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))) :
                        this.SelectedValue;
                    this.DisplayTextUI.StartAnimation();
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex);
                }
            }
        }

        private void OnNextItemButtonPressed()
        {
            this.SelectedIndex++;
            this.SelectedIndex++;
        }

        private void OnPreviousItemButtonPressed()
        {
            this.SelectedIndex--;
        }

        public void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(OptionSelectorBehaviour));
        }
    }
}
