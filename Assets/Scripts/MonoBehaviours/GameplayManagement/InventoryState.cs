#pragma warning disable CS0649
/**************************************************
 *  InventoryState.cs
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

    public abstract class InventoryStateBase
    {
        private static InventoryStateBase instance = GetInitialState();

        public static InventoryStateBase Instance { get => instance; }

        protected bool LockInput { get; set; }

        protected PlayerBehaviour Player { get => PlayerBehaviour.Instance; }

        protected InventoryStateBase() { }

        public abstract void HandleInput();

        public static InventoryStateBase GetInitialState()
        {
            return InventoryAvailableState.Instance;
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

        public abstract void UseItem();

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

    public class InventoryAvailableState : InventoryStateBase
    {
        private static InventoryAvailableState instance = new InventoryAvailableState();

        public static new InventoryAvailableState Instance { get => instance; }

        public override void UseItem()
        {
            if (Player == null) /* || Player.CurrentRoom == null) */
            {
                return;
            }

            /*
            if (Player.Inventory.GetItemCount(Player.SelectedItemType) > 0)
            {
                if (Player.SelectedItemType == ItemType.EauDuMuglump)
                {
                    Player.UseCoverScent();
                }
                else if (Player.SelectedItemType == ItemType.BearTrap)
                {
                    Player.UseBearTrap();
                }

            }
            else
            {
                var message = string.Empty; // StringContent.ItemEmpty[Player.SelectedItemType]();

                if (!GameplayMenuManagerBehaviour.Instance.MainTextPanel.Text.Contains(message))
                {
                    GameplayMenuManagerBehaviour.AppendLineMainWindowText(message);
                }
            }
            */
        }

        public override void HandleInput()
        {
            if (this.LockInput)
            {
                return;
            }

            if (InputExtension.IsUseItemPressed())
            {
                this.LockInput = true;
                GameplayMenuManagerBehaviour.Instance.StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsUseItemPressed(),
                        () =>
                        {
                            this.LockInput = false;
                            this.UseItem();
                        }));
            }
        }
    }

    public class InventoryUnavailableState : InventoryStateBase
    {
        private static InventoryAvailableState instance = new InventoryAvailableState();

        public static new InventoryAvailableState Instance { get => instance; }

        public override void UseItem() { }

        public override void HandleInput() { }
    }
}
