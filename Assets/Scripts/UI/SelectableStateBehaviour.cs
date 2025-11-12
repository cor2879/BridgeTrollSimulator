/**************************************************
 *  SelectableStateBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System;

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    using BeautifulInterface = Interface.Elements.Scripts;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;

    /// <summary>
    /// Adds standard behaviour to selectable UI elements
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    /// <seealso cref="UnityEngine.EventSystems.ISelectHandler" />
    [RequireComponent(typeof(Selectable))]
    public class SelectableStateBehaviour : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        private const float ColorMultiplier = 1.0f;

        private const float FadeDuration = 0.1f;

        [SerializeField, ReadOnly]
        private bool isSelected;

        [SerializeField]
        private ButtonsPanelBehaviour buttonsPanel;

        [SerializeField]
        private Color normalColor;

        [SerializeField]
        private Color selectedColor;

        [SerializeField]
        private Color highlightedColor;

        [SerializeField]
        private Color pressedColor;

        [SerializeField]
        private Color disabledColor;

        [SerializeField, ReadOnly]
        private BeautifulInterface.ButtonUI button;

        private Selectable selectable;

        private ColorBlock ColorBlock { get; set; }

        public Color NormalColor { get => this.normalColor; private set => this.normalColor = value; }

        public Color SelectedColor { get => this.selectedColor; private set => this.selectedColor = value; }

        public Color HighlightedColor { get => this.highlightedColor; private set => this.highlightedColor = value; }

        public Color PressedColor { get => this.pressedColor; private set => this.pressedColor = value; }

        public Color DisabledColor { get => this.disabledColor; private set => this.disabledColor = value; }

        public BeautifulInterface.ButtonUI Button
        {
            get
            {
                if (this.button == null)
                {
                    this.button = this.GetComponent<BeautifulInterface.ButtonUI>();
                }

                return this.button;
            }
        }

        public ButtonsPanelBehaviour ButtonsPanel
        {
            get
            {
                return this.buttonsPanel;
            }

            set
            {
                this.buttonsPanel = value;
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

        public bool IsSelected
        {
            get
            {
                if (this.ButtonsPanel != null)
                {
                    this.IsSelected = (this.ButtonsPanel.SelectedButton == this);
                }

                return this.isSelected;
            }

            private set => this.isSelected = value;
        }

        public Action Selected { get; set; }

        public Action Deselected { get; set; }

        public void Start()
        {
            if (this.ButtonsPanel != null && !(this.ButtonsPanel.Buttons.Contains(this)))
            {
                this.ButtonsPanel.Buttons.Add(this);
            }
        }

        public void Update()
        {
            if (this.IsSelected && (this.ButtonsPanel != null && !this.ButtonsPanel?.SelectedButton == this))
            {
                this.ButtonsPanel.SelectedButton = this;
            }

            if (ButtonsPanel?.SelectedButton == this)
            {
                this.Button?.Highlight();
            }
            else
            {
                this.Button?.Normal();
            }
        }

        public void Select()
        {
            if (ButtonsPanel.SelectedButton != null && ButtonsPanel.SelectedButton != this)
            {
                ButtonsPanel.SelectedButton.Deselect();
            }

            if (this.ButtonsPanel != null)
            {
                this.ButtonsPanel.SelectedButton = this;
            }
            else
            {
                this.Selectable.Select();
            }

            this.IsSelected = true;
        }

        public void Deselect()
        {
            if (this.Button != null)
            {
                this.Button.Deselect();
            }

            this.OnDeselect(null);
        }

        /// <summary>
        /// Called when the element is selected.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        public void OnSelect(BaseEventData eventData)
        {
            if (this.ButtonsPanel != null)
            {
                this.ButtonsPanel.SelectedButton = this;
            }

            this.IsSelected = true;
            this.Selected?.Invoke();
        }

        /// <summary>
        /// Called when the element is deselected.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        public void OnDeselect(BaseEventData eventData)
        {
            this.IsSelected = false;

            if (this.ButtonsPanel != null && this.ButtonsPanel.SelectedButton == this)
            {
                this.ButtonsPanel.SelectedButton = null;
            }

            this.Deselected?.Invoke();
        }
    }
}
