using System;
using System.Collections.Generic;

using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities
{
    public class NpcController : EntityController, IResolver
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
            if (CurrentControlMode != ControlMode.Npc &&
                CurrentControlMode != ControlMode.Passing)
            {
                // base.UpdateAnimator();
                // return;
            }

            animator.speed = 1f;
            float direction = rb.linearVelocity.x <= 0 ? -1f : 1f;
            float speed = Mathf.Abs(rb.linearVelocity.x) > 0.01f ? 0.5f : 0f;

            var velocityX = rb.linearVelocity.x;

            if (velocityX < -0.01f)
            {
                SetFacing(false);
            }
            else if (velocityX > 0.01f)
            {
                SetFacing(true);
            }

            animator.SetFloat(Constants.AnimatorParams.Speed, speed);
            animator.SetFloat(Constants.AnimatorParams.xDirection, direction);
        }

        protected override void ApplyMovement()
        {
            if (CurrentControlMode == ControlMode.Leaving)
            {
                rb.linearVelocity = new Vector2(walkSpeed, rb.linearVelocity.y);
                return;    
            }

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
        
        public override void OnCombatVictory(CombatResolutionData data)
        {
            ClearDemands();
            SpeechBubble.Show(Personality.GetSocialDuelVictoryDialog());
        }

        public override void OnCombatDefeat(CombatResolutionData data)
        {
            if (CurrentControlMode != ControlMode.Dead)
            {
                SpeechBubble.Show(Personality.GetSurrenderDialog());
                DemandComponent.ResolveDemands(this);
            }
        }

        public override void OnSocialDuelVictory(SocialDuelResolutionData data)
        {
            ClearDemands();
            SpeechBubble.Show(Personality.GetSocialDuelVictoryDialog());
        }

        public override void OnSocialDuelLoss(SocialDuelResolutionData data)
        {
            SpeechBubble.Show(Personality.GetSurrenderDialog());
            DemandComponent.ResolveDemands(this);
        }

        public override void Receive<TEvent>(TEvent evt)
        {
            base.Receive(evt);

            if (evt is AllowToPassEvent allow && allow.Target as EntityController == this)
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

        public override void ConcedeCombat(IReceiver opponent)
        {
            var concession = DemandFactory.CreateSurrenderDemand(this, opponent);

            GameEventBus.Publish(
                new ConcedeCombatEvent(
                    this,
                    opponent,
                    concession,
                    Time.frameCount));
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

        protected virtual void ClearDemands()
        {
            DemandComponent.Clear();
        }

        #region IReactor

        public override void AcceptSurrender(IReactor opponent, ITargetedEvent evt)
        {
            Debug.Log($"{Name} accepts surrender.");

            // NPC says something
            SpeechBubble.Show(Personality.GetAcceptSurrenderDialog());

            // Clear demands
            ClearDemands();
            var player = opponent as IReceiver;

            // End the current system
            if (evt is ConcedeCombatEvent)
            {
                GameEventBus.Publish(
                    new CombatSurrenderAcceptedEvent( 
                        this,
                        player,
                        Time.frameCount));
            }
            else if (evt is ConcedeSocialDuelEvent)
            {
                GameEventBus.Publish(
                    new SocialDuelSurrenderAcceptedEvent(
                        this, 
                        player,
                        Time.frameCount));
            }
        }

        public override void DenySurrender(IReactor opponent, ITargetedEvent evt)
        {
            Debug.Log($"{Name} refuses surrender.");

            SpeechBubble.Show(Personality.GetDenySurrenderDialog());

            var player = opponent as IReceiver;

            if (evt is ConcedeCombatEvent)
            {
                GameEventBus.Publish(
                    new CombatSurrenderDeniedEvent(
                        this,
                        player,
                        Time.frameCount));
            }
            else if (evt is ConcedeSocialDuelEvent)
            {
                GameEventBus.Publish(
                    new SocialDuelSurrenderDeniedEvent(
                        this,
                        player,
                        Time.frameCount));
            }

            // Nothing else happens
            // Combat continues
        }

        #endregion

        #region IResolver

        public void Leave()
        {
            SetControlMode(ControlMode.Leaving);

            rb.linearVelocity = new Vector2(walkSpeed, rb.linearVelocity.y);

            GameEventBus.Publish(
                new LeaveEvent(this, Time.frameCount));

            this.SpeechBubble.Show(
                this.Personality.GetLeaveDialog());
        }

        #endregion
    }
}