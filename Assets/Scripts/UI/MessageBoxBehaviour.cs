#pragma warning disable CS0649
/**************************************************
 *  MessageBoxBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System;
    using System.Collections;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    /// <summary>
    /// Defines the behaviours for a UI MessageBox
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(ButtonsPanelBehaviour))]
    public class MessageBoxBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The text label behaviour
        /// </summary>
        [SerializeField]
        private TextLabelBehaviour textLabelBehaviour;

        /// <summary>
        /// The yes button
        /// </summary>
        [SerializeField]
        private Button yesButton;

        /// <summary>
        /// The no button
        /// </summary>
        [SerializeField]
        private Button noButton;

        [SerializeField, ReadOnly]
        private UnityAction onNoClicked;

        private ButtonsPanelBehaviour buttonsPanel;

        public bool Enabled { get; private set; } = false;

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

        /// <summary>
        /// Shows the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="onYesClicked">The on yes clicked.</param>
        /// <param name="onNoClicked">The on no clicked.</param>
        public void Show(string text, UnityAction onYesClicked, UnityAction onNoClicked)
        {
            this.Show(text, StringContent.Yes, StringContent.No, onYesClicked, onNoClicked);
        }

        /// <summary>
        /// Shows the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="onYesClicked">The on yes clicked.</param>
        /// <param name="onNoClicked">The on no clicked.</param>
        public void Show(string text, string yesButtonText, string noButtonText, UnityAction onYesClicked, UnityAction onNoClicked)
        {
            this.textLabelBehaviour.Text = text;
            //this.yesButton.GetComponent<TextButtonBehaviour>().Text = yesButtonText;
            //this.noButton.GetComponent<TextButtonBehaviour>().Text = noButtonText;
            this.onNoClicked = onNoClicked;

            this.yesButton.onClick.AddListener(() =>
            {
                if (InputExtension.IsSubmitPressed())
                {
                    StartCoroutine(nameof(this.WaitForSubmitUpThenInvokeOnYesClicked), onYesClicked);
                }
                else
                {
                    onYesClicked?.Invoke();
                    this.Hide(true);
                }
            });

            this.noButton.onClick.AddListener(() =>
            {
                StartCoroutine(nameof(this.WaitForCancelUpThenHide), onNoClicked);
            });

            this.gameObject.SetActive(true);
            // GameManager.Instance.PauseAction = true;
            this.ButtonsPanel.DefaultButton.Select();
        }

        /// <summary>
        /// Hides the message box.
        /// </summary>
        /// <param name="pauseAction">if set to <c>true</c> the game will be paused when the message box is hidden, otherwise gameplay will resume/continue.</param>
        public void Hide(bool pauseAction)
        {
            if (InputExtension.IsSubmitPressed())
            {
                StartCoroutine(nameof(this.WaitForSubmitUpThenHide), pauseAction);
                return;
            }

            this.HideInternal(pauseAction);
        }

        /// <summary>
        /// Hides the message box.
        /// </summary>
        /// <param name="pauseAction">if set to <c>true</c> the game will be paused when the message box is hidden, otherwise gameplay will resume/continue.</param>
        private void HideInternal(bool pauseAction)
        {
            this.textLabelBehaviour.Text = string.Empty;

            this.yesButton.onClick.RemoveAllListeners();
            this.noButton.onClick.RemoveAllListeners();

            this.gameObject.SetActive(false);
            // GameManager.Instance.PauseAction = pauseAction;
        }

        /// <summary>
        /// Waits for submit the submit button to be released, then invokes OnYesClicked.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        private IEnumerator WaitForSubmitUpThenInvokeOnYesClicked(object action)
        {
            var onYesClicked = action as UnityAction;

            while (InputExtension.IsSubmitPressed())
            {
                yield return new WaitForFixedUpdate();
            }

            onYesClicked?.Invoke();
        }

        /// <summary>
        /// Waits for the submit button to be released, then hides.
        /// </summary>
        /// <param name="pauseActionObj">The pause action object.</param>
        /// <returns></returns>
        private IEnumerator WaitForSubmitUpThenHide(object pauseActionObj)
        {
            var pauseAction = false;

            if (pauseActionObj != null)
            {
                try
                {
                    pauseAction = (bool)pauseActionObj;
                }
                catch (Exception)
                { }
            }
            while (InputExtension.IsSubmitPressed())
            {
                yield return new WaitForFixedUpdate();
            }

            this.HideInternal(pauseAction);
        }

        /// <summary>
        /// Waits for the cancel button to be released, then hides.
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitForCancelUpThenHide(UnityAction onNoClicked)
        {
            while (InputExtension.IsGamepadPresent() ? InputExtension.IsUseStairsPressed() : (InputExtension.IsCancelPressed() || InputExtension.IsSubmitPressed()))
            {
                yield return new WaitForFixedUpdate();
            }

            (onNoClicked ?? this.onNoClicked).Invoke();
            this.HideInternal(false);
        }

        /// <summary>
        /// Updates this instance when the Unity Engine updates each frame.
        /// </summary>
        private void Update()
        {
            if (InputExtension.IsGamepadPresent() ? InputExtension.IsUseStairsPressed() : InputExtension.IsCancelPressed())
            {
                StartCoroutine(nameof(this.WaitForCancelUpThenHide), this.onNoClicked);
            }
        }

        /// <summary>
        /// Called when this instance is enabled.
        /// </summary>
        private void OnEnable()
        {
            this.noButton.Select();
            this.Enabled = true;
        }

        /// <summary>
        /// Called when this instance is disabled.
        /// </summary>
        private void OnDisable()
        {
            this.yesButton.Select();
            this.Enabled = false;
        }
    }
}
