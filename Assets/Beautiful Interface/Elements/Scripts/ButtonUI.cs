using System;
using System.Collections;
using System.Collections.Generic;
using ElRaccoone.Tweens;
using ElRaccoone.Tweens.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Extensions;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

namespace Interface.Elements.Scripts
{
    public class ButtonUI : Button
    {

        /// <summary>
        /// The original position of the Text (init. before animating)
        /// </summary>
        private Vector3 originalTextPos;

        private bool persistHighlight;

        /// <summary>
        /// The duration for each tween animation
        /// </summary>
        public float duration = 0.15f;

        #region Has Booleans

        public bool hasSlider;

        public bool hasHoverSound;
        public bool hasClickSound;

        #endregion


        #region Normal

        public List<ButtonState> normalStates;

        #endregion


        #region On Highlight

        public List<ButtonState> highlightStates;

        #endregion


        #region On Click

        public List<ButtonState> clickStates;

        #endregion


        #region Slider Effect

        public Slider slider;

        #endregion


        #region Sounds

        public AudioClip onHoverAudio;
        public AudioClip onClickAudio;

        #endregion


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            foreach (var state in normalStates)
            {
                var image = state.image;
                if (!image) continue;
                if (state.color == default)
                {
                    state.color = image.color;
                }
                
                image.color = state.color;
            }
            if (hasSlider) slider.value = 0;
            
            
            // Set highlight state to copy normal state
            int i;
            for (i = 0; i < highlightStates.Count; i++)
            {
                if (normalStates.Count > i && highlightStates[i].image == null)
                {
                    highlightStates[i].image = normalStates[i].image;
                    highlightStates[i].color = normalStates[i].color;
                }
            }

            if (i < normalStates.Count)
                for (; i < normalStates.Count; i++)
                    highlightStates.Add(new ButtonState());
            
            // Set click state to copy normal state
            for (i = 0; i < clickStates.Count; i++)
            {
                if (normalStates.Count > i && clickStates[i].image == null)
                {
                    clickStates[i].image = normalStates[i].image;
                    clickStates[i].color = normalStates[i].color;
                }
            }
            
            if (i < normalStates.Count)
                for (; i < normalStates.Count; i++)
                    clickStates.Add(new ButtonState());
        }
#endif

        public void Set(ButtonState state)
        {
            state.image.TweenGraphicColor(state.color, duration).SetEase(EaseType.ExpoOut);
        }


        public void Normal()
        {
            if (persistHighlight) return;

            foreach (var state in normalStates)
            {
                Set(state);
            }

            // Tween slider value
            if (hasSlider)
            {
                this.TweenValueFloat(0, 0.2f, f =>
                {
                    slider.value = f;
                }).SetEase(EaseType.ExpoOut).SetFrom(slider.value);
            }
        }

        public void Highlight()
        {
            foreach (var state in highlightStates)
            {
                Set(state);
            }

            // Tween slider value
            if (hasSlider)
            {
                this.TweenValueFloat(1, 0.2f, f =>
                {
                    slider.value = f;
                }).SetEase(EaseType.ExpoOut);
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if (!interactable) return;

            Highlight();

            if (hasHoverSound) AudioManager.Play(onHoverAudio);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (!interactable || persistHighlight) return;

            foreach (var state in clickStates)
            {
                Set(state);
            }

            if (hasClickSound) AudioManager.Play(onClickAudio);
            Invoke(nameof(Normal), duration);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            if (!interactable) return;

            Normal();
        }

        #region Modifications


        private RectTransform rectTransform = null;
        private SelectableStateBehaviour selectableStateBehaviour = null;

        public RectTransform RectTransform
        {
            get
            {
                if (this.rectTransform == null)
                {
                    this.rectTransform = this.GetComponent<RectTransform>();
                }

                return this.rectTransform;
            }
        }

        public SelectableStateBehaviour SelectableState
        {
            get
            {
                if (this.selectableStateBehaviour == null)
                {
                    this.selectableStateBehaviour = this.GetComponent<SelectableStateBehaviour>();
                }

                return this.selectableStateBehaviour;
            }
        }

        public ButtonsPanelBehaviour ButtonsPanel { get => this.SelectableState.ButtonsPanel; set => this.SelectableState.ButtonsPanel = value; }

        /// <summary>
        /// Stay highlighted
        /// </summary>
        public bool PersistHighlight
        {
            get => persistHighlight;
            set
            {
                persistHighlight = value;
                if (value)
                {
                    Highlight();
                }
                else
                {
                    Normal();
                }
            }
        }

        public void Click()
        {
            ExecuteEvents.Execute(this.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }

        public override void Select()
        {
            Debug.Log($"{this} Selected.  SelectableState: {this.SelectableState.IsSelected}");
            base.Select();
        }

        public void Deselect()
        {
            Debug.Log($"{this} Deselected SelectableState: {this.SelectableState.IsSelected}");
        }

        private void OnSelect()
        {
            this.Highlight();
        }

        private void OnDeselect()
        {
            this.Normal();
        }

        private void HighlightOrNormal()
        {
            Invoke((this.MaintainHighlightWhenClicked && this.RectTransform.IntersectsWith(InputExtension.MousePosition)) ? nameof(Highlight) : nameof(Normal), duration);
        }

        public IEnumerator WaitForDurationThenDoAction(WaitDuration waitDuration)
        {
            while (waitDuration.Duration >= float.Epsilon)
            {
                waitDuration.Duration -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            waitDuration.DoAction.Invoke();
        }

        public bool MaintainHighlightWhenClicked { get; set; } = false;

        public bool IsSelected
        {
            get => this.SelectableState.IsSelected;
        }

        #endregion
    }

    [Serializable]
    public class ButtonState
    {
        public Graphic image;
        public Color color;
    }
}