#pragma warning disable CS0649
/**************************************************
 *  ControlState.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours.GameplayManagement
{
    using System.Collections;

    using UnityEngine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Platform;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    public abstract class ControlStateBase
    {
        private static ControlStateBase instance = GetInitialState();

        public static ControlStateBase Instance { get => instance; }

        protected static GameplayMenuManagerBehaviour GameplayMenuManager { get => GameplayMenuManagerBehaviour.Instance; }

        protected PlayerBehaviour Player { get => PlayerBehaviour.Instance; }

        protected ControlStateBase()
        { }

        private static ControlStateBase GetInitialState()
        {
            return GetState();
        }

        private static ControlStateBase GetState()
        {
            if (true || PlatformManager.Platform.Equals(SupportedPlatform.iOS))
            {
                KeyboardControlState.Instance.DisableMenus();

                if (false && InputExtension.IsGamepadPresent())
                {
                    MobileTouchControlState.Instance.DisableMenus();
                    return GamepadControlState.Instance;
                }

                GamepadControlState.Instance.DisableMenus();
                return MobileTouchControlState.Instance;
            }
            else
            {
                MobileTouchControlState.Instance.DisableMenus();

                if (InputExtension.IsGamepadPresent())
                {
                    KeyboardControlState.Instance.DisableMenus();
                    return GamepadControlState.Instance;
                }
                else
                {
                    GamepadControlState.Instance.DisableMenus();
                }

                return KeyboardControlState.Instance;
            }
        }

        public void Update()
        {
            if (GameplayMenuManager.CurrentControlState != ControlStateBase.GetState())
            {
                ControlStateBase.ChangeState(ControlStateBase.GetState());
            }
        }

        private void DisableMenus()
        {
            // this.MainGameplayMenu.SetActive(false);
            //  this.DirectionalActionMenu.SetActive(false);
            // this.LookingAtRoomMenu.SetActive(false);
        }

        private static void SwapStates(ControlStateBase previousState, ControlStateBase newState)
        {
            // newState.MainGameplayMenu.SetActive(previousState.MainGameplayMenu.Active);
            // newState.DirectionalActionMenu.SetActive(previousState.DirectionalActionMenu.Active);
            // newState.LookingAtRoomMenu.SetActive(previousState.LookingAtRoomMenu.Active);

            previousState.DisableMenus();
        }

        public static void ChangeState(ControlStateBase newState)
        {
            if (newState == GameplayMenuManager.CurrentControlState)
            {
                Debug.Log("A redundant control state change was made.  Was this intentional?");
            }
            else
            {
                GameplayMenuManager.PreviousControlState = GameplayMenuManager.CurrentControlState;
            }

            GameplayMenuManager.CurrentControlState = newState;

            if (GameplayMenuManager.PreviousControlState != null)
            {
                ControlStateBase.SwapStates(
                    GameplayMenuManager.PreviousControlState, 
                    GameplayMenuManager.CurrentControlState);
            }
        }

        protected IEnumerator WaitForDurationThenDoAction(WaitDuration waitDuration)
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
        protected IEnumerator WaitForPredicateToBeFalseThenDoAction(WaitAction waitAction)
        {
            while (waitAction.Predicate())
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            waitAction.DoAction.Invoke();
        }
    }

    public class KeyboardControlState : ControlStateBase
    {
        private static KeyboardControlState instance = new KeyboardControlState();

        public new static KeyboardControlState Instance { get => instance; }
    }

    public class GamepadControlState : ControlStateBase
    {
        private static GamepadControlState instance = new GamepadControlState();

        public new static GamepadControlState Instance { get => instance; }
    }

    public class MobileTouchControlState : ControlStateBase
    {
        private static MobileTouchControlState instance = new MobileTouchControlState();

        public new static MobileTouchControlState Instance { get => instance; }
    }
}
