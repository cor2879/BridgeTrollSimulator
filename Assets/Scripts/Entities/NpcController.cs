using System;
using System.Collections.Generic;

using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameState;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities
{
    public class NpcController : EntityController
    {
        [Header("NPC Movement")]
        [SerializeField]
        private float walkSpeed = 1.5f;

        [Header("Sound Effects")]
        protected AudioClip epicDeathSfx;

        public override ControlMode DefaultControlMode => ControlMode.Npc;
        
        protected override void Awake()
        {
            base.Awake();
            SetControlMode(ControlMode.Npc);
            entityType = EntityType.Npc;
        }

        protected override void ProcessInput()
        { 
            if (CurrentControlMode != ControlMode.Npc)
            {
                base.ProcessInput();
            }
        }

        protected override void UpdateAnimator()
        {
            if (CurrentControlMode != ControlMode.Npc)
            {
                // base.UpdateAnimator();
                // return;
            }

            float direction = rb.linearVelocity.x <= 0 ? -1f : 1f;
            float speed = Mathf.Abs(rb.linearVelocity.x) > 0.01f ? 0.5f : 0f;

            animator.SetFloat(Constants.AnimatorParams.Speed, speed);
            animator.SetFloat(Constants.AnimatorParams.xDirection, direction);
        }

        protected override void ApplyMovement()
        {
            if (CurrentControlMode != ControlMode.Npc)
            {
                base.ApplyMovement();
                return;
            }

            rb.linearVelocity = new Vector2(-walkSpeed, rb.linearVelocity.y);
        }

        public override void HandleEncounter(IEncounterable other)
        {
            if (other.GameObject.CompareTag("Troll"))
            {
                SetControlMode(ControlMode.Encounter);
                rb.linearVelocity = Vector2.zero;

                Debug.Log("NPC encountered Troll!");
            }
        }

        public override void Receive<TEvent>(TEvent evt)
        {
            base.Receive(evt);

            if (evt is AllowToPassEvent allow && allow.Target == this)
            {
                BeginPassing();
            }
        }

        protected void BeginPassing()
        {
            SetControlMode(ControlMode.Passing);

            var player = GameDatabase.Instance.Player;

            Physics2D.IgnoreCollision(
                this.Collider,
                player.Collider,
                true);
        }

        public override void Die()
        {
            if (UnityEngine.Random.value < 0.25)
            {
                SetControlMode(ControlMode.Dead);
                TriggerAction(Constants.Triggers.Explode);
                GameEventBus.Publish(new EntityDiedEvent(this, Time.frameCount));
                GameEventBus.Publish(new SoundEffectEvent(
                    this, 
                    epicDeathSfx != null ? epicDeathSfx : deathSfx,
                    Time.frameCount));
            }
            else
            {
                base.Die();
            }
        }
    }
}