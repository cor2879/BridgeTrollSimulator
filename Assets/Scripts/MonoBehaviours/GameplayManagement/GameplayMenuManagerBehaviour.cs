#pragma warning disable CS0649
/**************************************************
 *  GameplayMenuManagerBehavour.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours.GameplayManagement
{
    using System;
    using System.Collections;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;


    using BeautifulInterface = Interface.Elements.Scripts;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Exceptions;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Interfaces;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Platform;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Rules;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    public class GameplayMenuManagerBehaviour : MonoBehaviour
    {
        private static GameplayMenuManagerBehaviour instance;
        private static bool[] validDirections = new bool[] { false, false, false, false };

        [SerializeField, ReadOnly]
        private GameplayMenuStateBase menuState;

        [SerializeField, ReadOnly]
        private GameplayMenuStateBase previousState;

        [SerializeField, ReadOnly]
        private ControlStateBase currentControlState;

        [SerializeField, ReadOnly]
        private ControlStateBase previousControlState;

        [SerializeField, ReadOnly]
        private bool isInputLocked;

        [SerializeField]
        private MainTextPanelBehaviour mainTextPanel;

        #region private reference fields


        #endregion

        #region public reference accessors

        #endregion

        #region public Component accessors


        #endregion

        private static PlayerBehaviour Player { get => PlayerBehaviour.Instance; }

        public GameplayMenuStateBase MenuState { get => this.menuState; set => this.menuState = value; }

        public GameplayMenuStateBase PreviousState { get => this.previousState; set => this.previousState = value; }

        public ControlStateBase CurrentControlState { get => this.currentControlState; set => this.currentControlState = value; }

        public ControlStateBase PreviousControlState { get => this.previousControlState; set => this.previousControlState = value; }

        public static GameplayMenuManagerBehaviour Instance { get => instance; }

        private static GameManager GameManager { get => GameManager.Instance; }

        public MainTextPanelBehaviour MainTextPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.mainTextPanel, nameof(this.mainTextPanel));

                return this.mainTextPanel;
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
        }

        private void Update()
        {
            // SetMenuValidDirections(GetValidDirections());
            this.MenuState.Update();
            this.CurrentControlState.Update();
            this.isInputLocked = this.MenuState.LockInput;
        }

        private void Start()
        {
            this.MenuState = GameplayMenuStateBase.Instance;
            this.CurrentControlState = ControlStateBase.Instance;
            this.MenuState.Start();
        }

        public static void AppendLineMainWindowText(string text)
        {
            // TODO
        }
        
        public static void SetMainGameplayMenuActive(bool activeState)
        {
            // Instance.CurrentControlState.MainGameplayMenu.SetActive(activeState);
        }

        public static void ClearMainWindowText()
        {

        }
        
        public static void SetMainTextWindowActive(bool activeState)
        {
            // TODO
        }

        public static void SetDirectionalActionSubMenuActiveState(bool activeState)
        {
            // Instance.CurrentControlState.DirectionalActionMenu.SetActive(activeState);
        }

        public static void SetLookingAtRoomMenuActiveState(bool activeState)
        {
            // Instance.CurrentControlState.LookingAtRoomMenu.SetActive(activeState);
        }

        public static void OpenMinimap()
        {
            GameManager.Minimap.Enable();
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

        /// <summary>
        /// Waits for predicate to be false then does the action.
        /// </summary>
        /// <param name="waitAction">The wait action.</param>
        /// <returns></returns>
        public IEnumerator WaitForPredicateToBeFalseThenDoAction(WaitAction waitAction)
        {
            while (waitAction.Predicate())
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            waitAction.DoAction.Invoke();
        }

        private void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(GameplayMenuManagerBehaviour));
        }
    }
}
