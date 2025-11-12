#pragma warning disable CS0649
/**************************************************
 *  InputManager.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours.GameplayManagement
{
    using System;

    using UnityEngine;
    using UnityEngine.InputSystem;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    public class InputManager : MonoBehaviour
    {
        private static InputManager instance;

        #region private child references

        [SerializeField]
        private InputAction submitAction;

        [SerializeField]
        private InputAction cancelAction;

        [SerializeField]
        private InputAction pauseAction;

        [SerializeField]
        private InputAction useMapAction;

        [SerializeField]
        private InputAction cycleArrowsAction;

        [SerializeField]
        private InputAction cycleInventoryAction;

        [SerializeField]
        private InputAction useItemAction;

        [SerializeField]
        private InputAction lookAction;

        [SerializeField]
        private InputAction upAction;

        [SerializeField]
        private InputAction leftAction;

        [SerializeField]
        private InputAction downAction;

        [SerializeField]
        private InputAction rightAction;

        [SerializeField]
        private InputAction centerAction;

        [SerializeField]
        private InputAction zoomInAction;

        [SerializeField]
        private InputAction zoomOutAction;

        [SerializeField]
        private InputAction useStairsAction;

        [SerializeField]
        private InputAction moveAction;

        [SerializeField]
        private InputAction lookUpAction;

        [SerializeField]
        private InputAction lookLeftAction;

        [SerializeField]
        private InputAction lookDownAction;

        [SerializeField]
        private InputAction lookRightAction;

        [SerializeField]
        private InputAction shootUpAction;

        [SerializeField]
        private InputAction shootLeftAction;

        [SerializeField]
        private InputAction shootDownAction;

        [SerializeField]
        private InputAction shootRightAction;

        [SerializeField]
        private InputAction selectArrowsAction;

        [SerializeField]
        private InputAction selectFlashArrowsAction;

        [SerializeField]
        private InputAction selectNetArrowsAction;

        [SerializeField]
        private InputAction useEauDuMuglumpAction;

        [SerializeField]
        private InputAction useBearTrapAction;

        #endregion

        public static InputManager Instance { get => instance; private set => instance = value; }

        #region public child accessors

        public InputAction SubmitAction { get => this.submitAction; }

        public InputAction CancelAction { get => this.cancelAction; }

        public InputAction PauseAction { get => this.pauseAction; }

        public InputAction UseMapAction { get => this.useMapAction; }

        public InputAction CycleArrowsAction { get => this.cycleArrowsAction; }

        public InputAction CycleInventoryAction { get => this.cycleInventoryAction; }

        public InputAction UseItemAction { get => this.useItemAction; }

        public InputAction LookAction { get => this.lookAction; }

        public InputAction UpAction { get => this.upAction; }

        public InputAction LeftAction { get => this.leftAction; }

        public InputAction DownAction { get => this.downAction; }

        public InputAction RightAction { get => this.rightAction; }

        public InputAction CenterAction { get => this.centerAction; }

        public InputAction ZoomInAction { get => this.zoomInAction; }

        public InputAction ZoomOutAction { get => this.zoomOutAction; }

        public InputAction UseStairsAction { get => this.useStairsAction; }

        public InputAction MoveAction { get => this.moveAction; }

        public InputAction LookUpAction { get => this.lookUpAction; }

        public InputAction LookLeftAction { get => this.lookLeftAction; }

        public InputAction LookDownAction { get => this.lookDownAction; }

        public InputAction LookRightAction { get => this.lookRightAction; }

        public InputAction ShootUpAction { get => this.shootUpAction; }

        public InputAction ShootLeftAction { get => this.shootLeftAction; }

        public InputAction ShootDownAction { get => this.shootDownAction; }

        public InputAction ShootRightAction { get => this.shootRightAction; }

        public InputAction SelectArrowsAction { get => this.selectArrowsAction; }

        public InputAction SelectFlashArrowsAction { get => this.selectFlashArrowsAction; }

        public InputAction SelectNetArrowsAction { get => this.selectNetArrowsAction; }

        public InputAction UseEauDuMuglumpAction { get => this.useEauDuMuglumpAction; }

        public InputAction UseBearTrapAction { get => this.useBearTrapAction; }

        #endregion

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }

            this.SubmitAction.performed += this.SubmitActionPerformed;
            this.CancelAction.performed += this.CancelActionPerformed;
            this.PauseAction.performed += this.PauseActionPerformed;
            this.UseMapAction.performed += this.UseMapActionPerformed;
            this.CycleArrowsAction.performed += this.CycleArrowsActionPerformed;
            this.CycleInventoryAction.performed += this.CycleInventoryActionPerformed;
            this.UseItemAction.performed += this.UseItemActionPerformed;
            this.LookAction.performed += this.LookActionPerformed;
            this.UpAction.performed += this.UpActionPerformed;
            this.LeftAction.performed += this.LeftActionPerformed;
            this.DownAction.performed += this.DownActionPerformed;
            this.RightAction.performed += this.RightActionPerformed;
            this.CenterAction.performed += this.CenterActionPerformed;
            this.ZoomInAction.performed += this.ZoomInActionPerformed;
            this.ZoomOutAction.performed += this.ZoomOutActionPerformed;
            this.UseStairsAction.performed += this.UseStairsActionPerformed;
            this.MoveAction.performed += this.MoveActionPerformed;
            this.LookUpAction.performed += this.LookUpActionPerformed; 
            this.LookLeftAction.performed += this.LookLeftActionPerformed;
            this.LookDownAction.performed += this.LookDownActionPerformed;
            this.LookRightAction.performed += this.LookRightActionPerformed;
            this.ShootUpAction.performed += this.ShootUpActionPerformed;
            this.ShootLeftAction.performed += this.ShootLeftActionPerformed;
            this.ShootDownAction.performed += this.ShootDownActionPerformed;
            this.ShootRightAction.performed += this.ShootRightActionPerformed;

            this.SelectArrowsAction.performed += this.SelectArrowsActionPerformed;
            this.SelectFlashArrowsAction.performed += this.SelectFlashArrowsActionPerformed;
            this.SelectNetArrowsAction.performed += this.SelectNetArrowsActionPerformed;

            this.UseEauDuMuglumpAction.performed += this.UseEauDuMuglumpActionPerformed;
            this.UseBearTrapAction.performed += this.UseBearTrapActionPerformed;
        }

        private void OnEnable()
        {
            this.SubmitAction.Enable();
            this.CancelAction.Enable();
            this.PauseAction.Enable();
            this.UseMapAction.Enable();
            this.CycleArrowsAction.Enable();
            this.CycleInventoryAction.Enable();
            this.UseItemAction.Enable();
            this.LookAction.Enable();
            this.UpAction.Enable();
            this.LeftAction.Enable();
            this.DownAction.Enable();
            this.RightAction.Enable();
            this.CenterAction.Enable();
            this.ZoomInAction.Enable();
            this.ZoomOutAction.Enable();
            this.UseStairsAction.Enable();
            this.MoveAction.Enable();
            this.LookUpAction.Enable();
            this.LookLeftAction.Enable();
            this.LookDownAction.Enable();
            this.LookRightAction.Enable();
            this.ShootUpAction.Enable();
            this.ShootLeftAction.Enable();
            this.ShootDownAction.Enable();
            this.ShootRightAction.Enable();
            this.SelectArrowsAction.Enable();
            this.SelectFlashArrowsAction.Enable();
            this.SelectNetArrowsAction.Enable();
            this.UseEauDuMuglumpAction.Enable();
            this.UseBearTrapAction.Enable();
        }

        private void OnDisable()
        {
            this.SubmitAction.Disable();
            this.CancelAction.Disable();
            this.PauseAction.Disable();
            this.UseMapAction.Disable();
            this.CycleArrowsAction.Disable();
            this.CycleInventoryAction.Disable();
            this.UseItemAction.Disable();
            this.LookAction.Disable();
            this.UpAction.Disable();
            this.LeftAction.Disable();
            this.DownAction.Disable();
            this.RightAction.Disable();
            this.CenterAction.Disable();
            this.ZoomInAction.Disable();
            this.ZoomOutAction.Disable();
            this.UseStairsAction.Disable();
            this.MoveAction.Disable();
            this.LookUpAction.Disable();
            this.LookLeftAction.Disable();
            this.LookDownAction.Disable();
            this.LookRightAction.Disable();
            this.ShootUpAction.Disable();
            this.ShootLeftAction.Disable();
            this.ShootDownAction.Disable();
            this.ShootRightAction.Disable();
            this.SelectArrowsAction.Disable();
            this.SelectFlashArrowsAction.Disable();
            this.SelectNetArrowsAction.Disable();
            this.UseEauDuMuglumpAction.Disable();
            this.UseBearTrapAction.Disable();
        }

        private void HandleActionPerformed(InputAction.CallbackContext callbackContext, Action onDown, Action onPressed, Action onUp)
        {
            if (callbackContext.action.WasPressedThisFrame())
            {
                onDown?.Invoke();
            }
            
            if (callbackContext.action.IsPressed())
            {
                onPressed?.Invoke();
            }
            
            if (callbackContext.action.WasReleasedThisFrame())
            {
                onUp?.Invoke();
            }
        }

        private void SubmitActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnSubmitActionDown, this.OnSubmitActionPressed, this.OnSubmitActionUp);
        }

        private void CancelActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnCancelActionDown, this.OnCancelActionPressed, this.OnCancelActionUp);
        }

        private void PauseActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnPauseActionDown, this.OnPauseActionPressed, this.OnPauseActionUp);
        }

        private void UseMapActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnUseMapActionDown, this.OnUseMapActionPressed, this.OnUseMapActionUp);
        }

        private void CycleArrowsActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnCycleArrowsActionDown, this.OnCycleArrowsActionPressed, this.OnCycleArrowsActionUp);
        }

        private void CycleInventoryActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnCycleInventoryActionDown, this.OnCycleInventoryActionPressed, this.OnCycleInventoryActionUp);
        }

        private void UseItemActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnUseItemActionDown, this.OnUseItemActionPressed, this.OnUseItemActionUp);
        }

        private void LookActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnLookActionDown, this.OnLookActionPressed, this.OnLookActionUp);
        }

        private void UpActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnUpActionDown, this.OnUpActionPressed, this.OnUpActionUp);
        }

        private void LeftActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnLeftActionDown, this.OnLeftActionPressed, this.OnLeftActionUp);
        }

        private void DownActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnDownActionDown, this.OnDownActionPressed, this.OnDownActionUp);
        }

        private void RightActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnRightActionDown, this.OnRightActionPressed, this.OnRightActionUp);
        }

        private void CenterActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnCenterActionDown, this.OnCenterActionPressed, this.OnCenterActionUp);
        }

        private void ZoomInActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnZoomInActionDown, this.OnZoomInActionPressed, this.OnZoomInActionUp);
        }

        private void ZoomOutActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnZoomOutActionDown, this.OnZoomOutActionPressed, this.OnZoomOutActionUp);
        }

        private void UseStairsActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnUseStairsActionDown, this.OnUseStairsActionPressed, this.OnUseStairsActionUp);
        }

        private void MoveActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnMoveActionDown, this.OnMoveActionPressed, this.OnMoveActionUp);
        }

        private void LookUpActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnLookUpActionDown, this.OnLookUpActionPressed, this.OnLookUpActionUp);
        }
        private void LookLeftActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnLookLeftActionDown, this.OnLookLeftActionPressed, this.OnLookLeftActionUp);
        }
        private void LookDownActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnLookDownActionDown, this.OnLookDownActionPressed, this.OnLookDownActionUp);
        }
        private void LookRightActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnLookRightActionDown, this.OnLookRightActionPressed, this.OnLookRightActionUp);
        }

        private void ShootUpActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnShootUpActionDown, this.OnShootUpActionPressed, this.OnShootUpActionUp);
        }
        private void ShootLeftActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnShootLeftActionDown, this.OnShootLeftActionPressed, this.OnShootLeftActionUp);
        }
        private void ShootDownActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnShootDownActionDown, this.OnShootDownActionPressed, this.OnShootDownActionUp);
        }
        private void ShootRightActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnShootRightActionDown, this.OnShootRightActionPressed, this.OnShootRightActionUp);
        }

        private void SelectArrowsActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnSelectArrowsActionDown, this.OnSelectArrowsActionPressed, this.OnSelectArrowsActionUp);
        }

        private void SelectFlashArrowsActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnSelectFlashArrowsActionDown, this.OnSelectFlashArrowsActionPressed, this.OnSelectFlashArrowsActionUp);
        }

        private void SelectNetArrowsActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnSelectNetArrowsActionDown, this.OnSelectNetArrowsActionPressed, this.OnSelectNetArrowsActionUp);
        }

        private void UseEauDuMuglumpActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnUseEauDuMuglumpActionDown, this.OnUseEauDuMuglumpActionPressed, this.OnUseEauDuMuglumpActionUp);
        }

        private void UseBearTrapActionPerformed(InputAction.CallbackContext callbackContext)
        {
            this.HandleActionPerformed(callbackContext, this.OnUseBearTrapActionDown, this.OnUseBearTrapActionPressed, this.OnUseBearTrapActionUp);
        }


        public event Action OnSubmitActionDown;
        public event Action OnSubmitActionPressed;
        public event Action OnSubmitActionUp;

        public event Action OnCancelActionDown;
        public event Action OnCancelActionPressed;
        public event Action OnCancelActionUp;

        public event Action OnPauseActionDown;
        public event Action OnPauseActionPressed;
        public event Action OnPauseActionUp;

        public event Action OnUseMapActionDown;
        public event Action OnUseMapActionPressed;
        public event Action OnUseMapActionUp;

        public event Action OnCycleArrowsActionDown;
        public event Action OnCycleArrowsActionPressed;
        public event Action OnCycleArrowsActionUp;

        public event Action OnCycleInventoryActionDown;
        public event Action OnCycleInventoryActionPressed;
        public event Action OnCycleInventoryActionUp;

        public event Action OnUseItemActionDown;
        public event Action OnUseItemActionPressed;
        public event Action OnUseItemActionUp;

        public event Action OnLookActionDown;
        public event Action OnLookActionPressed;
        public event Action OnLookActionUp;

        public event Action OnUpActionDown;
        public event Action OnUpActionPressed;
        public event Action OnUpActionUp;

        public event Action OnLeftActionDown;
        public event Action OnLeftActionPressed;
        public event Action OnLeftActionUp;

        public event Action OnDownActionDown;
        public event Action OnDownActionPressed;
        public event Action OnDownActionUp;

        public event Action OnRightActionDown;
        public event Action OnRightActionPressed;
        public event Action OnRightActionUp;

        public event Action OnCenterActionDown;
        public event Action OnCenterActionPressed;
        public event Action OnCenterActionUp;

        public event Action OnZoomInActionDown;
        public event Action OnZoomInActionPressed;
        public event Action OnZoomInActionUp;

        public event Action OnZoomOutActionDown;
        public event Action OnZoomOutActionPressed;
        public event Action OnZoomOutActionUp;

        public event Action OnUseStairsActionDown;
        public event Action OnUseStairsActionPressed;
        public event Action OnUseStairsActionUp;

        public event Action OnMoveActionDown;
        public event Action OnMoveActionPressed;
        public event Action OnMoveActionUp;

        public event Action OnLookUpActionDown;
        public event Action OnLookUpActionPressed;
        public event Action OnLookUpActionUp;

        public event Action OnLookLeftActionDown;
        public event Action OnLookLeftActionPressed;
        public event Action OnLookLeftActionUp;

        public event Action OnLookDownActionDown;
        public event Action OnLookDownActionPressed;
        public event Action OnLookDownActionUp;

        public event Action OnLookRightActionDown;
        public event Action OnLookRightActionPressed;
        public event Action OnLookRightActionUp;

        public event Action OnShootUpActionDown;
        public event Action OnShootUpActionPressed;
        public event Action OnShootUpActionUp;

        public event Action OnShootLeftActionDown;
        public event Action OnShootLeftActionPressed;
        public event Action OnShootLeftActionUp;

        public event Action OnShootDownActionDown;
        public event Action OnShootDownActionPressed;
        public event Action OnShootDownActionUp;

        public event Action OnShootRightActionDown;
        public event Action OnShootRightActionPressed;
        public event Action OnShootRightActionUp;

        public event Action OnSelectArrowsActionDown;
        public event Action OnSelectArrowsActionPressed;
        public event Action OnSelectArrowsActionUp;

        public event Action OnSelectFlashArrowsActionDown;
        public event Action OnSelectFlashArrowsActionPressed;
        public event Action OnSelectFlashArrowsActionUp;

        public event Action OnSelectNetArrowsActionDown;
        public event Action OnSelectNetArrowsActionPressed;
        public event Action OnSelectNetArrowsActionUp;

        public event Action OnUseEauDuMuglumpActionDown;
        public event Action OnUseEauDuMuglumpActionPressed;
        public event Action OnUseEauDuMuglumpActionUp;

        public event Action OnUseBearTrapActionDown;
        public event Action OnUseBearTrapActionPressed;
        public event Action OnUseBearTrapActionUp;

        public static bool IsActionPressed(InputAction inputAction)
        {
            return inputAction.IsPressed();
        }
    }
}
