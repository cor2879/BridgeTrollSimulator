/**************************************************
 *  InputExtension.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/
namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using System.Linq;

    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.EnhancedTouch;
    using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours.GameplayManagement;

    /// <summary>
    /// Static class used for handling game input
    /// </summary>
    public static class InputExtension
    {
        public static CursorState CursorState { get; private set; }

        public static bool IsAnyKeyPressed()
        {
            return Keyboard.current.anyKey.wasPressedThisFrame;
        }

        public static bool IsAnyButtonPressed()
        {
            return Gamepad.current != null &&
                (Gamepad.current.aButton.wasPressedThisFrame ||
                 Gamepad.current.bButton.wasPressedThisFrame ||
                 Gamepad.current.xButton.wasPressedThisFrame ||
                 Gamepad.current.yButton.wasPressedThisFrame);
        }

        public static bool IsAnyTouchActive()
        {
            return Touch.activeTouches.Where(t => t.isTap).Any();
        }

        /// <summary>
        /// Determines whether [is move north pressed].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is move north pressed]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMoveNorthPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.UpAction);
        }

        /// <summary>
        /// Determines whether [is move east pressed].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is move east pressed]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMoveEastPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.RightAction);
        }

        public static bool IsRightPressed()
        {
            return IsMoveEastPressed();
        }

        /// <summary>
        /// Determines whether [is move south pressed].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is move south pressed]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMoveSouthPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.DownAction);
        }

        /// <summary>
        /// Determines whether [is move west pressed].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is move west pressed]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMoveWestPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.LeftAction);
        }

        public static bool IsLeftPressed()
        {
            return IsMoveWestPressed();
        }

        /// <summary>
        /// Gets the movement direction.
        /// </summary>
        /// <returns></returns>
        public static Direction GetMovementDirection()
        {
            return IsMoveNorthPressed() ? Direction.North : IsMoveEastPressed() ? Direction.East :
                IsMoveSouthPressed() ? Direction.South : IsMoveWestPressed() ? Direction.West : Direction.Idle;
        }

        /// <summary>
        /// Determines whether [is close window pressed].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is close window pressed]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCloseWindowPressed()
        {
            return InputExtension.IsCancelPressed();
        }

        /// <summary>
        /// Determines whether [is open minimap pressed].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is open minimap pressed]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOpenMinimapPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.UseMapAction);
        }

        /// <summary>
        /// Determines whether [is center map pressed].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is center map pressed]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCenterMapPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.CenterAction);
        }

        /// <summary>
        /// Determines whether [is cancel pressed].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is cancel pressed]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCancelPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.CancelAction);
        }

        /// <summary>
        /// Determines whether [is submit pressed].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is submit pressed]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSubmitPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.SubmitAction);
        }

        public static bool IsShootPressed()
        {
            return IsSubmitPressed();
        }

        public static bool IsCycleItemsPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.CycleInventoryAction);
        }

        public static bool IsCyleArrowsPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.CycleArrowsAction);
        }

        public static bool IsLookPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.LookAction);
        }

        public static bool IsPageForwardPressed()
        {
            return IsCycleItemsPressed();
        }

        public static bool IsPageBackPressed()
        {
            return IsCyleArrowsPressed();
        }

        public static bool IsMenuPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.PauseAction);
        }

        public static bool IsUseItemPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.UseItemAction);
        }

        public static bool IsUseStairsPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.UseStairsAction);
        }

        public static bool IsMovePressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.MoveAction);
        }

        public static bool IsLeftTriggerPressed()
        {
            return Gamepad.current.leftTrigger.wasPressedThisFrame;
        }

        public static bool IsLookUpPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.LookUpAction);
        }

        public static bool IsLookLeftPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.LookLeftAction);
        }

        public static bool IsLookDownPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.LookDownAction);
        }

        public static bool IsLookRightPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.LookRightAction);
        }

        public static bool IsShootUpPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.ShootUpAction);
        }

        public static bool IsShootLeftPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.ShootLeftAction);
        }

        public static bool IsShootDownPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.ShootDownAction);
        }

        public static bool IsShootRightPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.ShootRightAction);
        }

        public static bool IsSelectArrowsPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.SelectArrowsAction);
        }

        public static bool IsSelectFlashArrowsPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.SelectFlashArrowsAction);
        }

        public static bool IsSelectNetArrowsPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.SelectNetArrowsAction);
        }

        public static bool IsUseEauDuMuglumpPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.UseEauDuMuglumpAction);
        }

        public static bool IsUseBearTrapPressed()
        {
            return InputManager.IsActionPressed(InputManager.Instance.UseBearTrapAction);
        }

        /// <summary>
        /// Gets the zoom delta.
        /// </summary>
        /// <returns></returns>
        public static float GetZoomDelta()
        {
            if (Mouse.current?.scroll.ReadValue().y != 0.0f)
            {
                return Mouse.current.scroll.ReadValue().y * 0.1f;
            }

            return GetVerticalScrollDelta();
        }

        public static float GetVerticalScrollDelta()
        {
            if (InputManager.IsActionPressed(InputManager.Instance.ZoomInAction))
            {
                return 1.0f;
            }

            if (InputManager.IsActionPressed(InputManager.Instance.ZoomOutAction))
            {
                return -1.0f;
            }

            return 0.0f;
        }

        public static float GetHorizontalScrollDelta()
        {
            if (InputManager.IsActionPressed(InputManager.Instance.ZoomInAction))
            {
                return 1.0f;
            }

            if (InputManager.IsActionPressed(InputManager.Instance.ZoomOutAction))
            {
                return -1.0f;
            }

            return 0.0f;
        }

        public static void HideMouseIfGamepadIsPresent()
        {
            if (IsGamepadPresent())
            {
                InputExtension.UnlockMouse();
                InputExtension.HideMouse();
                InputExtension.ClampMouse();
            }
            else
            {
                InputExtension.ShowMouse();
                InputExtension.UnClampMouse();
            }
        }

        public static bool IsGamepadPresent()
        {
            return false;  // !string.IsNullOrEmpty(Input.GetJoystickNames().FirstOrDefault());
        }

        public static void HideMouse()
        {
            if (!(InputExtension.CursorState == CursorState.Locked))
            {
                Cursor.visible = false;
            }
        }

        public static void LockMouse()
        {
            InputExtension.CursorState = CursorState.Locked;
        }

        public static void ClampMouse()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        public static void ShowMouse()
        {
            if (!(InputExtension.CursorState == CursorState.Locked))
            {
                Cursor.visible = true;
            }
        }

        public static void UnClampMouse()
        {
            if (!(InputExtension.CursorState == CursorState.Locked))
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        public static void UnlockMouse()
        {
            InputExtension.CursorState = CursorState.Default;
        }

        public static Vector3 MousePosition
        {
            get
            {
                return Mouse.current.position.ReadValue();
            }
        }
    }
}
