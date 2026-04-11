using System.Linq;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.InputHandling;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;
using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities
{
    public class TrollController : EntityController
    {
        public override ControlMode DefaultControlMode => ControlMode.FreeRoam;

        #region Initialization

        protected virtual void OnEnable()
        {
            GameEventBus.Subscribe<LeaveEvent>(OnLeave);
        } 

        protected virtual void OnDisable()
        {
            GameEventBus.Unsubscribe<LeaveEvent>(OnLeave);
        }

        protected override void Awake()
        {
            base.Awake();
            this.inputSource = new KeyboardInputSource();
            this.entityType = EntityType.Troll;
        }

        #endregion

        #region EventHandlers

        private void OnLeave(LeaveEvent evt)
        {
            if (CurrentControlMode != ControlMode.Encounter)
            {
                return;
            }

            if (evt.Sender is NpcController npc)
            {
                SetControlMode(DefaultControlMode);
            }
        }

        public override void OnCombatVictory(CombatResolutionData data)
        {
            ClearStatusEffects();
        }

        public override void OnCombatDefeat(CombatResolutionData data)
        {
            ClearStatusEffects();
            AllowToPass(data.EnemySide.First());
        }

        public override void OnSocialDuelVictory(SocialDuelResolutionData data)
        {
            ClearStatusEffects();
        }

        public override void OnSocialDuelLoss(SocialDuelResolutionData data)
        {
            ClearStatusEffects();
            AllowToPass(data.Npc);
        }

        #endregion

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
                controlMode == ControlMode.Dead ||
                controlMode == ControlMode.Combat)
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
                this.Die();
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

        public override void Receive<TEvent>(TEvent evt) 
        {
            base.Receive(evt);

            if (evt is TollRefusedEvent refused && refused.Target as EntityController == this)
            {
                HandleTollRefusal(refused);
            }

            if (evt is TollPaidEvent paid && paid.Target as EntityController == this)
            {
                HandleTollPaidEvent(paid);
            }

            if (evt is LevelUpEvent levelUp && IsPlayerControlled)
            {
                HandleLevelUp(levelUp);
            }
        }

        private void HandleTollRefusal(TollRefusedEvent evt)
        {
            /*
            GameEventBus.Publish(
                new CombatStartedEvent(this, evt.Initiator, Time.frameCount));
            */
        }

        private void HandleTollPaidEvent(TollPaidEvent evt)
        {
            AddGold(evt.Amount);

            RestoreResolve(maxResolve / 10);
            var npc = evt.Initiator as NpcController;

            if (npc.DemandComponent.HasDemands)
            {
                npc.DemandComponent.ResolveNextDemand(npc);
                return;
            }
            else
            {
                AllowToPass(npc);
            }
        }

        private void HandleLevelUp(LevelUpEvent levelUp)
        {
            Debug.Log($"Level Up received: {levelUp}");
        }

        public void AllowToPass(IReceiver npc)
        {
            GameEventBus.Publish(new AllowToPassEvent(
                this,
                npc,
                Time.frameCount));
            SetControlMode(DefaultControlMode);
        }

        #region IReactor

        public override void AcceptSurrender(IReactor opponent, ITargetedEvent evt)
        {}

        public override void DenySurrender(IReactor opponent, ITargetedEvent evt)
        {}

        #endregion
    }
}