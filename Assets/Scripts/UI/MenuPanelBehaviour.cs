#pragma warning disable CS0649
/**************************************************
 *  MenuPanelBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours.GameplayManagement;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;


    [RequireComponent(typeof(ButtonsPanelBehaviour))]
    public class MenuPanelBehaviour : UIHelperBehaviour
    {
        private bool lockInput;

        #region private child references

        [SerializeField]
        private Button backButton;

        [SerializeField]
        private Button settingsButton;

        [SerializeField]
        private Button quitButton;

        #endregion

        private ButtonsPanelBehaviour buttonsPanel;

        public static GameplayMenuManagerBehaviour GameplayMenuManager { get => GameplayMenuManagerBehaviour.Instance; }

        #region public child accessors

        public Button BackButton
        {
            get
            {
                this.ValidateUnityEditorParameter(this.backButton, nameof(this.BackButton));

                return this.backButton;
            }
        }

        public Button SettingsButton
        {
            get
            {
                this.ValidateUnityEditorParameter(this.settingsButton, nameof(this.settingsButton));

                return this.settingsButton;
            }
        }

        public Button QuitButton
        {
            get
            {
                this.ValidateUnityEditorParameter(this.quitButton, nameof(this.quitButton));

                return this.quitButton;
            }
        }

        #endregion

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

        public bool Enabled { get => enabled; private set => enabled = value; }

        public override void Enable()
        {
            GameManager.Instance.PauseAction = true;
            this.ButtonsPanel.DefaultButton.Select();
            this.Enabled = true;
            base.Enable();
        }

        public override void Disable()
        {
            this.Enabled = false;
            GameManager.Instance.PauseAction = false;
            base.Disable();
        }

        private void OnBackButtonClicked()
        {
            this.StartCoroutine(
                nameof(this.WaitForPredicateToBeFalseThenDoAction),
                new WaitAction(InputExtension.IsShootPressed, this.Disable));
        }

        public void Start()
        {
            GameManager.Instance.SettingsPanel.OnDisabled.AddListener(
                () =>
                {
                    this.Enable();
                });

            this.BackButton.onClick.AddListener(this.OnBackButtonClicked);

            this.SettingsButton.onClick.AddListener(
                () =>
                {
                    this.Disable();
                    GameManager.Instance.SettingsPanel.Enable();
                });


            this.QuitButton.onClick.AddListener(
                () =>
                {
                this.Disable();
                    /*
                    GameManager.Instance.MessageBoxBehaviour.Show(
                        StringContent.ForfeitDungeonConfirmation,
                        () =>
                        {
                            GameManager.Instance.GameOver(GameOverCondition.Quit);
                            GameManager.Instance.MessageBoxBehaviour.Hide(true);
                        },
                    () =>
                    {
                            GameplayMenuManager.MenuState.ChangeState(MainGameplayMenuState.Instance);
                    });
                    */
                });
        }

        public void Update()
        {
            GameManager.Instance.PauseAction = true;

            if (InputExtension.IsGamepadPresent() && InputExtension.IsCancelPressed())
            {
                StartCoroutine(nameof(this.WaitForCancelUpThenHide));
            }

            if (GameManager.Instance.IsGameOver)
            {
                base.Disable();
            }
        }

        public void FixedUpdate()
        {
        }

        /// <summary>
        /// Waits for the cancel button to be released, then hides.
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitForCancelUpThenHide()
        {
            while (InputExtension.IsCancelPressed())
            {
                yield return new WaitForFixedUpdate();
            }

            this.Disable();
        }

        public void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(MenuPanelBehaviour));
        }
    }
}
