#pragma warning disable CS0649
/**************************************************
 *  GameplayMenuState.cs
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
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    public abstract class GameplayMenuStateBase
    {
        private static GameplayMenuStateBase instance = GetInitialState();

        public static GameplayMenuStateBase Instance { get => instance; }

        protected PlayerBehaviour Player { get => PlayerBehaviour.Instance; }

        protected bool MainMenuActive { get; set; }

        protected bool DirectionalMenuActive { get; set; }

        protected bool LookingAtRoomMenuActive { get; set; }

        private static bool LOCK_INPUT { get; set; }

        public virtual bool LockInput { get => LOCK_INPUT; protected set => LOCK_INPUT = value; }

        public ActionStateBase PreviousActionState { get; set; } = null;

        public ActionStateBase ActionState { get; private set; } = ActionStateBase.GetInitialState();

        public InventoryStateBase InventoryState { get; set; } = InventoryStateBase.GetInitialState();

        public GameplayMenuManagerBehaviour GameplayMenuManager { get => GameplayMenuManagerBehaviour.Instance; }

        protected GameplayMenuStateBase() { }

        public virtual void Update()
        {
            GameplayMenuManagerBehaviour.SetMainGameplayMenuActive(this.MainMenuActive);
            GameplayMenuManagerBehaviour.SetDirectionalActionSubMenuActiveState(this.DirectionalMenuActive);
            GameplayMenuManagerBehaviour.SetLookingAtRoomMenuActiveState(this.LookingAtRoomMenuActive);

            if (!this.LockInput && this.Player != null && !GameManager.Instance.Minimap.IsActive())
            {
                this.HandleInput();
            }
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void Start()
        {
            /*
            switch (Settings.MenuStyle)
            {
                case MenuStyle.DragonQuest:
                    this.ChangeToWalkingState(false);
                    break;
                case MenuStyle.Mobile:
                    this.ChangeToEverythingState();
                    break;
                default:
                    ChangeToWalkingState(false);
                    break;
            }
            */
        }

        public abstract void HandleInput();

        protected virtual void ExitState() { }

        public void ChangeToWalkingState(bool showDirectionalMenu)
        {
            this.ChangeState(showDirectionalMenu ? 
                DirectionalActionSubMenuState.Instance as GameplayMenuStateBase : 
                MainGameplayMenuState.Instance as GameplayMenuStateBase);
            this.ChangeActionState(WalkingState.Instance);
        }

        public void ChangeToShootingState()
        {
            this.ChangeState(DirectionalActionSubMenuState.Instance);
            this.ChangeActionState(ShootingState.Instance);
        }

        public void ChangeToLookingState()
        {
            this.ChangeState(DirectionalActionSubMenuState.Instance);
            this.ChangeActionState(LookingState.Instance);
        }

        public void ChangeToEverythingState()
        {
            this.ChangeState(DirectionalActionSubMenuState.Instance);
            this.ChangeActionState(EverythingState.Instance);
        }

        public void TogglePauseGame()
        {
            if (!GameManager.Instance.MenuPanel.Enabled)
            {
                GameManager.Instance.MenuPanel.Enable();
            }
            else if (!InputExtension.IsGamepadPresent())
            {
                GameManager.Instance.MenuPanel.Disable();
            }
        }

        public virtual void GoBack()
        {
            this.ChangeToWalkingState(false);
        }

        public virtual void ExecuteLeftAction()
        {
            this.ActionState?.ExecuteLeftAction();
        }

        public virtual void ExecuteUpAction()
        {
            this.ActionState?.ExecuteUpAction();
        }

        public virtual void ExecuteRightAction()
        {
            this.ActionState?.ExecuteRightAction();
        }

        public virtual void ExecuteDownAction()
        {
            this.ActionState?.ExecuteDownAction();
        }

        public void ChangeState(GameplayMenuStateBase newState)
        {
            if (newState != this.GameplayMenuManager.MenuState)
            {
                this.GameplayMenuManager.PreviousState = this.GameplayMenuManager.MenuState;
            }
            else
            {
                Debug.Log("The Gameplay Menu State Manager was had a redundant state change.  Did you mean to do this?");
            }

            this.GameplayMenuManager.MenuState = newState;
            this.GameplayMenuManager.PreviousState?.ExitState();
        }

        public void ChangeActionState(ActionStateBase newState)
        {
            if (newState != this.GameplayMenuManager.MenuState.ActionState)
            {
                this.GameplayMenuManager.MenuState.PreviousActionState = this.GameplayMenuManager.MenuState.ActionState;
            }
            else
            {
                Debug.Log($"The ActionState for {this} had a redundant state change.  Did you mean to do this?");
            }

            this.GameplayMenuManager.MenuState.ActionState = newState;
        }

        protected static GameplayMenuStateBase GetInitialState()
        {
            return MainGameplayMenuState.Instance;
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

    public class MainGameplayMenuState : GameplayMenuStateBase
    {
        private static MainGameplayMenuState instance = new MainGameplayMenuState();

        public static new MainGameplayMenuState Instance { get => instance; }

        protected MainGameplayMenuState() { }

        private bool ShowThisMenu { get => this.Player != null && !this.Player.IsWalking && !this.Player.IsFalling && !GameManager.Instance.PauseAction; }          

        public override void Update()
        {
            this.MainMenuActive = this.ShowThisMenu;
            this.DirectionalMenuActive = false;
            this.LookingAtRoomMenuActive = false;
            GameplayMenuManagerBehaviour.SetMainTextWindowActive(this.ShowThisMenu);
            base.Update();
        }

        public override void HandleInput()
        {
            this.ActionState.HandleInput();
            this.InventoryState.HandleInput();

            if (!GameManager.Instance.PauseAction)
            {
                if (InputExtension.IsMovePressed())
                {
                    this.LockInput = true;
                    GameplayMenuManager.StartCoroutine(
                        nameof(this.WaitForPredicateToBeFalseThenDoAction),
                        new WaitAction(
                            () => InputExtension.IsMovePressed(),
                            () =>
                            {
                                this.LockInput = false;
                                this.ChangeToWalkingState(true);
                            }));
                }

                if (InputExtension.IsShootPressed())
                {
                    this.LockInput = true;
                    GameplayMenuManager.StartCoroutine(
                        nameof(this.WaitForPredicateToBeFalseThenDoAction),
                        new WaitAction(
                            () => InputExtension.IsShootPressed(),
                            () =>
                            {
                                this.LockInput = false;
                                this.ChangeToShootingState();
                            }));
                }

                if (InputExtension.IsLookPressed())
                {
                    this.LockInput = true;
                    GameplayMenuManager.StartCoroutine(
                        nameof(this.WaitForPredicateToBeFalseThenDoAction),
                        new WaitAction(
                            () => InputExtension.IsLookPressed(),
                            () =>
                            {
                                this.LockInput = false;
                                this.ChangeToLookingState();
                            }));
                }

                /*
                if (this.Player.CurrentRoom != null && this.Player.CurrentRoom.IsEntrance && !this.Player.IsWalking && !this.Player.IsFalling && InputExtension.IsUseStairsPressed())
                {
                    this.LockInput = true;
                    GameplayMenuManager.StartCoroutine(
                        nameof(this.WaitForPredicateToBeFalseThenDoAction),
                        new WaitAction(
                            () => InputExtension.IsUseStairsPressed(),
                            () =>
                            {
                                this.LockInput = false;
                                // his.UseStairs();
                            }));
                }
                */
            }
            if ((InputExtension.IsMenuPressed()) && !this.LockInput && !this.Player.IsFalling && !GameManager.Instance.IsGameOver)
            {
                this.LockInput = true;
                GameplayMenuManager.StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsMenuPressed(),
                        () =>
                        {
                            this.LockInput = false;
                            this.TogglePauseGame();
                        }));
            }
        }
    }

    public class DirectionalActionSubMenuState : GameplayMenuStateBase
    {
        private static DirectionalActionSubMenuState instance = new DirectionalActionSubMenuState();

        public static new DirectionalActionSubMenuState Instance { get => instance; }

        protected DirectionalActionSubMenuState () { }

        private bool ShowThisMenu
        {
            get
            {
                return this.Player != null &&
                   // this.Player.HasCameraFocus &&
                    this.Player.IsIdle &&
                    !GameManager.Instance.PauseAction;
            }
        }
        
        public override void Update()
        {
            this.MainMenuActive = false;

            if (!this.DirectionalMenuActive && this.ShowThisMenu && this.ActionState == ShootingState.Instance)
            {
                GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.BowStrings.GetNext());
            }

            if (!this.DirectionalMenuActive && this.ShowThisMenu)
            {
                GameplayMenuManagerBehaviour.ClearMainWindowText();
                // this.Player.CurrentRoom?.GetWarnings();
            }

            this.DirectionalMenuActive = this.ShowThisMenu;
            this.LookingAtRoomMenuActive = false;
            GameplayMenuManagerBehaviour.SetMainTextWindowActive(this.ShowThisMenu);
            this.Player?.SetAnimatorValue(Constants.IsAiming, this.ShowThisMenu && this.ActionState == ShootingState.Instance);
            base.Update();
        }

        public override void HandleInput()
        {
            this.ActionState.HandleInput();
            this.InventoryState.HandleInput();

            if (InputExtension.IsCancelPressed() && !this.LockInput)
            {
                this.LockInput = true;
                GameplayMenuManager.StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsCancelPressed(),
                        () =>
                        {
                            this.LockInput = false;
                            if (this.ActionState.CanPauseGame)
                            {
                                this.TogglePauseGame();
                            }
                            else
                            {
                                this.GoBack();
                            }
                        }));
            }
        }

        protected override void ExitState()
        {
            this.Player?.SetAnimatorValue(Constants.IsAiming, false);
        }
    }

    public class LookingAtRoomMenuState : GameplayMenuStateBase
    { 
        private static LookingAtRoomMenuState instance = new LookingAtRoomMenuState();

        public static new LookingAtRoomMenuState Instance { get => instance; }

        protected LookingAtRoomMenuState() { }

        private bool ShowThisMenu { get => this.Player != null && !this.Player.IsFalling && !GameManager.Instance.PauseAction; }

        public override void Update()
        {
            this.MainMenuActive = false;
            this.DirectionalMenuActive = false;
            this.LookingAtRoomMenuActive = this.ShowThisMenu;
            GameplayMenuManagerBehaviour.SetMainTextWindowActive(false);
            base.Update();
        }

        public override void GoBack()
        {
            CameraManager.TransitionTo(Player);
            this.ChangeState(DirectionalActionSubMenuState.Instance);
        }

        public override void HandleInput()
        {
            if (InputExtension.IsCancelPressed() && !this.LockInput)
            {
                this.LockInput = true;
                GameplayMenuManager.StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsCancelPressed(),
                        () =>
                        {
                            this.LockInput = false;
                            this.GoBack();
                        }));
            }
        }
    }

    public class NullMenuState : GameplayMenuStateBase
    {
        private static NullMenuState instance = new NullMenuState();

        public static new NullMenuState Instance { get => instance; }

        protected NullMenuState() { }

        public override void Update()
        {
            this.MainMenuActive = false;
            this.DirectionalMenuActive = false;
            this.LookingAtRoomMenuActive = false;
            GameplayMenuManagerBehaviour.SetMainTextWindowActive(false);
            base.Update();
        }

        public override void HandleInput() { }
    }
}
