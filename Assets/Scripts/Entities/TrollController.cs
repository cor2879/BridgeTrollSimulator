using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.InputHandling;
using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities
{
    public class TrollController : EntityController
    {
        protected override void Awake()
        {
            base.Awake();
            this.inputSource = new KeyboardInputSource();
            this.entityType = EntityType.Troll;
        }

        protected override void ProcessInput()
        {
            base.ProcessInput();

            if (!HasInput)
            {
                return;
            }

            if (controlMode == ControlMode.Disabled ||
                controlMode == ControlMode.CutScene ||
                controlMode == ControlMode.Encounter ||
                controlMode == ControlMode.Dead)
            {
                return;
            }

            if (inputSource.AttackPressed())
            {
                TriggerAction(Constants.Triggers.Attack);
            }

            if (inputSource.ThreatenPressed())
            {
                TriggerAction(Constants.Triggers.Threaten);
            }

            if (inputSource.ItchPressed())
            {
                TriggerAction(Constants.Triggers.Itch);
            }

            if (inputSource.JumpPressed() &&
                this.CurrentControlMode == ControlMode.FreeRoam)
            {
                TriggerAction(Constants.Triggers.Jump);
            }

            if (inputSource.DiePressed())
            {
                SetControlMode(ControlMode.Dead);
                TriggerAction(Constants.Triggers.Die);
            }
        }

        public override void HandleEncounter(IEncounterable other)
        {
            Debug.Log($"{entityType}_{instanceId}::{nameof(HandleEncounter)}");
            if (other.GameObject.CompareTag("NPC"))
            {
                SetControlMode(ControlMode.Encounter);
                animatorSpeed = 0f;

                Debug.Log("Troll encountered NPC!");
            }
        }
    }
}