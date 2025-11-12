#pragma warning disable CS0649
/**************************************************
 *  ButtonsPanelBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Exceptions;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    public class ButtonsPanelBehaviour : UIHelperBehaviour
    {
        [SerializeField]
        private List<SelectableStateBehaviour> buttons;

        [SerializeField]
        private SelectableStateBehaviour defaultButton;

        [SerializeField]
        private bool active;

        [SerializeField, ReadOnly]
        private bool lockFixedUpdate = false;

        [SerializeField, ReadOnly]
        private SelectableStateBehaviour selectedButton;

        [SerializeField, ReadOnly]
        private GameObject eventStateSelectedObject;

        public List<SelectableStateBehaviour> Buttons { get => this.buttons; }

        public SelectableStateBehaviour SelectedButton { get => this.selectedButton; set => this.selectedButton = value; }

        public SelectableStateBehaviour DefaultButton { get => this.defaultButton; set => this.defaultButton = value; }

        private GameObject EventStateSelectedObject { get => this.eventStateSelectedObject; set => this.eventStateSelectedObject = value; }

        public bool Active
        {
            get => this.active;
            private set => this.active = value;
        }

        public void Awake()
        {
            this.OnEnabled.AddListener(this.OnEnableAction);
            this.Activate();

            foreach (var button in this.Buttons.Where((button) => button != null))
            {
                button.ButtonsPanel = this;
            }
        }

        public void FixedUpdate()
        {
            if (this.lockFixedUpdate)
            {
                return;
            }

            if (this.Active)
            {
                if (this.SelectedButton == null)
                {
                    this.lockFixedUpdate = true;
                    StartCoroutine(
                        nameof(WaitForDurationThenDoAction),
                        new WaitDuration(
                            Time.fixedDeltaTime * 2,
                            () =>
                            {
                                if (this.SelectedButton == null)
                                {
                                    this.SelectedButton = this.DefaultButton;
                                }
                                this.lockFixedUpdate = false;
                            }));
                }

                if (this.SelectedButton != null && !this.SelectedButton.IsSelected)
                {
                    this.SelectedButton.Select();
                }

                if (this.EventStateSelectedObject == null && this.SelectedButton != null)
                {
                    EventSystem.current.SetSelectedGameObject(this.SelectedButton.gameObject);
                }
                else if (this.SelectedButton != null && (EventSystem.current.currentSelectedGameObject != this.SelectedButton.gameObject))
                {
                    EventSystem.current.SetSelectedGameObject(this.SelectedButton.gameObject);

                    //this.lockFixedUpdate = true;
                    //StartCoroutine(
                    //    nameof(WaitForDurationThenDoAction),
                    //    new WaitDuration(
                    //        Time.fixedDeltaTime * 2,
                    //        () =>
                    //        {
                    //            if (EventSystem.current.currentSelectedGameObject != this.SelectedButton.gameObject)
                    //            {
                    //                EventSystem.current.SetSelectedGameObject(this.SelectedButton.gameObject);
                    //            }
                    //            this.lockFixedUpdate = false;
                    //        }));

                }
            }

            this.EventStateSelectedObject = EventSystem.current.currentSelectedGameObject;
        }

        public override void Enable()
        {
            base.Enable();
        }

        private void OnEnableAction()
        {
            this.Activate();
        }

        public void Activate()
        {
            this.Active = true;
        }

        public void Deactivate()
        {
            this.Active = false;

            if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject == this.SelectedButton?.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}
