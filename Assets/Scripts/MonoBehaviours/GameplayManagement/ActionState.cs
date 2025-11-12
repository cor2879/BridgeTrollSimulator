#pragma warning disable CS0649
/**************************************************
 *  ActionState.cs
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

    public abstract class ActionStateBase
    {
        protected bool lockInput = false;

        private static ActionStateBase instance = GetInitialState();

        public static ActionStateBase Instance { get => instance; }

        protected GameplayMenuManagerBehaviour GameplayMenuManager { get => GameplayMenuManagerBehaviour.Instance; }

        protected PlayerBehaviour Player { get => PlayerBehaviour.Instance; }

        public abstract bool CanPauseGame { get; }

        protected ActionStateBase() { }

        public abstract void HandleInput();

        public abstract void ExecuteLeftAction();

        public abstract void ExecuteUpAction();

        public abstract void ExecuteRightAction();

        public abstract void ExecuteDownAction();

        protected virtual void Look(Direction direction)
        {
            // this.Player.LookAtRoom(direction);
            // GameplayMenuManagerBehaviour.Instance.MenuState.ChangeState(LookingAtRoomMenuState.Instance);
            this.lockInput = false;
        }

        protected virtual void Look(Direction direction, GameplayMenuStateBase newState)
        {
            // this.Player.LookAtRoom(direction);
            // GameplayMenuManagerBehaviour.Instance.MenuState.ChangeState(newState);
            this.lockInput = false;
        }

        protected virtual void Shoot(Direction direction)
        {
            this.lockInput = true;
            /*
            this.Player.ShootArrow(
                direction,
                () =>
                {
                    this.lockInput = false;
                });
            */
        }

        protected virtual void Walk(Position position)
        {
            /*
            if (direction != Direction.Idle)
            {
                var newRoom = this.Player?.CurrentRoom?.GetAdjacentRoom(direction);

                if (newRoom != null)
                {
                    // GameManager.Instance.ClearMainWindowText();
                    /* this.Player.MovementBehaviour.MoveToRoom(
                        newRoom,
                        () =>
                        {
                            // GameplayMenuManager.MenuState.ChangeState(MainGameplayMenuState.Instance);
                            GameplayMenuManager.MenuState.ChangeActionState(WalkingState.Instance);
                        },
                        null); 
                    this.Player.IncrementMoveCount();
                }
            }
            */
        }

        protected virtual void Walk(Position position, ActionStateBase newActionState)
        {
            // TODO: Implement walking!
            /* 
            if (direction != Direction.Idle)
            {
                var newRoom = this.Player?.CurrentRoom?.GetAdjacentRoom(direction);

                if (newRoom != null)
                {
                    GameManager.ClearMainWindowText();
                    /*
                    this.Player.MovementBehaviour.MoveToRoom(
                        newRoom,
                        () =>
                        {
                            GameplayMenuManager.MenuState.ChangeActionState(newActionState);
                        },
                        null);
                    
                    this.Player.IncrementMoveCount();
                }
            }
            */
        }

        public static ActionStateBase GetInitialState()
        {
            return WalkingState.Instance;
            /*
            switch (Settings.MenuStyle)
            {
                case MenuStyle.DragonQuest:
                    return WalkingState.Instance;
                case MenuStyle.Mobile:
                    return EverythingState.Instance;
                default:
                    return WalkingState.Instance;
            }
            */
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

    public class WalkingState : ActionStateBase
    {
        private static WalkingState instance = new WalkingState();

        public static new WalkingState Instance { get => instance; }

        public override bool CanPauseGame
        {
            get => !this.Player.IsWalking && this.Player.CanMove;
        }

        public override void ExecuteLeftAction()
        {
            // this.Walk(Direction.West);
        }

        public override void ExecuteUpAction()
        {
            // this.Walk(Direction.North);
        }

        public override void ExecuteRightAction()
        {
            // this.Walk(Direction.East);
        }

        public override void ExecuteDownAction()
        {
            // this.Walk(Direction.South);
        }

        public override void HandleInput()
        {
            if (this.Player == null || !this.Player.HasCameraFocus || GameManager.Instance.PauseAction
                || this.Player.IsFalling)
            {
                return;
            }

            if (!this.Player.IsWalking && this.Player.CanMove)
            {
                // this.Walk(InputExtension.GetMovementDirection());
            }
        }
    }

    public class ShootingState : ActionStateBase
    {
        private static ShootingState instance = new ShootingState();

        public static new ShootingState Instance { get => instance; }

        public override bool CanPauseGame { get => false; }

        public override void ExecuteLeftAction()
        {
            this.Shoot(Direction.West);
        }

        public override void ExecuteUpAction()
        {
            this.Shoot(Direction.North);
        }

        public override void ExecuteRightAction()
        {
            this.Shoot(Direction.East);
        }

        public override void ExecuteDownAction()
        {
            this.Shoot(Direction.South);
        }

        public override void HandleInput()
        {
            if (this.lockInput || this.Player == null) // || this.Player.CurrentRoom == null || 
                // !this.Player.HasCameraFocus || GameManager.Instance.PauseAction)
            {
                return;
            }

            var direction = InputExtension.GetMovementDirection();

            if (direction != Direction.Idle) // && this.Player.CurrentRoom.GetAdjacentRoom(direction) != null)
            {
                this.lockInput = true;

                GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                nameof(this.WaitForPredicateToBeFalseThenDoAction),
                new WaitAction(
                    () => InputExtension.GetMovementDirection() == direction,
                    () => this.Shoot(direction)));
            }
        }
    }

    public class LookingState : ActionStateBase
    {
        private static LookingState instance = new LookingState();

        public static new LookingState Instance { get => instance; }

        public override bool CanPauseGame { get => false; }

        public override void ExecuteLeftAction()
        {
            // this.Look(Direction.West);
        }

        public override void ExecuteUpAction()
        {
            // this.Look(Direction.North);
        }

        public override void ExecuteRightAction()
        {
            // this.Look(Direction.East);
        }

        public override void ExecuteDownAction()
        {
            // this.Look(Direction.South);
        }

        public override void HandleInput()
        {
            if (this.lockInput || this.Player == null) // || this.Player.CurrentRoom == null || !this.Player.HasCameraFocus || GameManager.Instance.PauseAction)
            {
                return;
            }

            var direction = InputExtension.GetMovementDirection();

            if (direction != Direction.Idle) // && this.Player.CurrentRoom.GetAdjacentRoom(direction) != null)
            {
                this.lockInput = true;

                GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                nameof(this.WaitForPredicateToBeFalseThenDoAction),
                new WaitAction(
                    () => InputExtension.GetMovementDirection() == direction,
                    () => this.Look(direction)));
            }
        }
    }

    public class EverythingState : ActionStateBase
    {
        private static EverythingState instance = new EverythingState();

        public static new EverythingState Instance => instance;

        public override bool CanPauseGame { get => true; }

        public override void ExecuteLeftAction()
        {
            this.Look(Direction.West);
        }

        public override void ExecuteUpAction()
        {
            this.Look(Direction.North);
        }

        public override void ExecuteRightAction()
        {
            this.Look(Direction.East);
        }

        public override void ExecuteDownAction()
        {
            this.Look(Direction.South);
        }

        private void UseStairs()
        {
            GameManager.Instance.MessageBoxBehaviour.Show(
                StringContent.ExitDungeonConfirmation,
                () =>
                {
                    GameManager.Instance.GameOver(GameOverCondition.Victory);
                    GameManager.Instance.MessageBoxBehaviour.Hide(true);
                },
                () =>
                {
                });
        }

        private void HandleGeneralInput()
        {
            if (this.Player != null && this.Player.IsIdle)
            {
                if (InputExtension.IsSelectArrowsPressed())
                {
                    this.lockInput = true;

                    GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                        nameof(this.WaitForPredicateToBeFalseThenDoAction),
                        new WaitAction(
                            () => InputExtension.IsSelectArrowsPressed(),
                            () =>
                            {
                                // this.Player.SelectArrows(ArrowType.Arrow);
                                this.lockInput = false;
                            }));
                }

                if (InputExtension.IsSelectFlashArrowsPressed())
                {
                    this.lockInput = true;

                    GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                        nameof(this.WaitForPredicateToBeFalseThenDoAction),
                        new WaitAction(
                            () => InputExtension.IsSelectFlashArrowsPressed(),
                            () =>
                            {
                                // this.Player.SelectArrows(ArrowType.FlashArrow);
                                this.lockInput = false;
                            }));
                }

                if (InputExtension.IsSelectNetArrowsPressed())
                {
                    this.lockInput = true;

                    GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                        nameof(this.WaitForPredicateToBeFalseThenDoAction),
                        new WaitAction(
                            () => InputExtension.IsSelectNetArrowsPressed(),
                            () =>
                            {
                                // this.Player.SelectArrows(ArrowType.NetArrow);
                                this.lockInput = false;
                            }));
                }

                if (InputExtension.IsUseStairsPressed()) // && this.Player.CurrentRoom.IsEntrance)
                {
                    this.lockInput = true;

                    GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                        nameof(this.WaitForPredicateToBeFalseThenDoAction),
                        new WaitAction(
                            () => InputExtension.IsUseStairsPressed(),
                            () =>
                            {
                                this.lockInput = false;
                                this.UseStairs();
                            }));
                }
            }
        }

        private void HandleMoveInput()
        {
            if (InputExtension.IsMoveNorthPressed()) // && this.Player.CurrentRoom.GetAdjacentRoom(Direction.North) != null)
            {
                this.lockInput = true;

                GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsMoveNorthPressed(),
                        () =>
                        {
                            // this.Walk(Direction.North, Instance);
                            this.lockInput = false;
                        }));
            }

            if (InputExtension.IsMoveWestPressed()) // && this.Player.CurrentRoom.GetAdjacentRoom(Direction.West) != null)
            {
                this.lockInput = true;

                GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsMoveWestPressed(),
                        () =>
                        {
                            // this.Walk(Direction.West, Instance);
                            this.lockInput = false;
                        }));
            }

            if (InputExtension.IsMoveSouthPressed()) // && this.Player.CurrentRoom.GetAdjacentRoom(Direction.South) != null)
            {
                this.lockInput = true;

                GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsMoveSouthPressed(),
                        () =>
                        {
                            // this.Walk(Direction.South, Instance);
                            this.lockInput = false;
                        }));
            }

            if (InputExtension.IsMoveEastPressed()) // && this.Player.CurrentRoom.GetAdjacentRoom(Direction.East) != null) 
            {
                this.lockInput = true;

                GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsMoveEastPressed(),
                        () =>
                        {
                            // this.Walk(Direction.East, Instance);
                            this.lockInput = false;
                        }));
            }
        }

        private void HandleShootInput()
        {
            if (InputExtension.IsShootUpPressed()) // && this.Player.CurrentRoom.GetAdjacentRoom(Direction.North) != null)
            {
                this.lockInput = true;

                GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsShootUpPressed(),
                        () => this.Shoot(Direction.North)));
            }

            if (InputExtension.IsShootLeftPressed()) // && this.Player.CurrentRoom.GetAdjacentRoom(Direction.West) != null)
            {
                this.lockInput = true;

                GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsShootLeftPressed(),
                        () => this.Shoot(Direction.West)));
            }

            if (InputExtension.IsShootDownPressed()) // && this.Player.CurrentRoom.GetAdjacentRoom(Direction.South) != null)
            {
                this.lockInput = true;

                GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsShootDownPressed(),
                        () => this.Shoot(Direction.South)));
            }

            if (InputExtension.IsShootRightPressed()) // && this.Player.CurrentRoom.GetAdjacentRoom(Direction.East) != null)
            {
                this.lockInput = true;

                GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsShootRightPressed(),
                        () => this.Shoot(Direction.East)));
            }
        }

        private void HandleLookInput()
        {
            if (InputExtension.IsLookUpPressed()) // && this.Player.CurrentRoom.GetAdjacentRoom(Direction.North) != null)
            {
                this.lockInput = true;

                GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsLookUpPressed(),
                        () => this.Look(Direction.North)));
            }

            if (InputExtension.IsLookLeftPressed()) // && this.Player.CurrentRoom.GetAdjacentRoom(Direction.West) != null)
            {
                this.lockInput = true;

                GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsLookLeftPressed(),
                        () => this.Look(Direction.West)));
            }

            if (InputExtension.IsLookDownPressed()) // && this.Player.CurrentRoom.GetAdjacentRoom(Direction.South) != null)
            {
                this.lockInput = true;

                GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsLookDownPressed(),
                        () => this.Look(Direction.South)));
            }

            if (InputExtension.IsLookRightPressed()) // && this.Player.CurrentRoom.GetAdjacentRoom(Direction.East) != null)
            {
                this.lockInput = true;

                GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsLookRightPressed(),
                        () => this.Look(Direction.East)));
            }
        }

        public override void HandleInput()
        {
            if (this.lockInput || this.Player == null) // || this.Player.CurrentRoom == null || !this.Player.HasCameraFocus || GameManager.Instance.PauseAction)
            {
                return;
            }

            this.HandleGeneralInput();
            this.HandleMoveInput();
            this.HandleShootInput();
            this.HandleLookInput();
        }
    }
}