using System;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.InputHandling;
using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class EntityController 
        : MonoBehaviour, IEventSource, IEncounterable
    {
        protected Animator animator;
        protected Rigidbody2D rb;
        protected IInputSource inputSource;
        [SerializeField, ReadOnly]
        protected EntityType entityType;

        [SerializeField, ReadOnly]
        protected Guid instanceId;

        [SerializeField]
        protected ControlMode controlMode = ControlMode.FreeRoam;

        [Header("Movement")]
        [SerializeField] 
        private float moveSpeed = 1.0f;

        [SerializeField]
        protected float animatorSpeed = 0.0f;

        private Vector2 movementInput;
        private bool facingRight = true;

        #region Properties

        public GameObject GameObject => this.gameObject;
        public ControlMode CurrentControlMode => controlMode;
        public IInputSource InputSource => inputSource;


        public string SourceName => $"{entityType}_{instanceId}";
        public GameSystemType SystemType => GameSystemType.Entity;

        #endregion

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            instanceId = Guid.NewGuid();
        }

        protected virtual void Update()
        {
            this.ProcessInput();
            this.UpdateAnimator();
        }

        protected virtual void FixedUpdate()
        {
            this.ApplyMovement();
        }

        #region Input

        protected bool HasInput => inputSource is not null;

        protected virtual void ProcessInput()
        {
            if (!HasInput)
            {
                return;
            }

            if (controlMode == ControlMode.Disabled ||
                controlMode == ControlMode.CutScene ||
                controlMode == ControlMode.Dead ||
                controlMode == ControlMode.Npc ||
                controlMode == ControlMode.Encounter)
            {
                return;
            }

            var horizontalAxis = inputSource.GetHorizontal();

            animatorSpeed = horizontalAxis != 0.0f ? 0.5f : 0.0f;

            this.movementInput = new Vector2(
                this.inputSource.GetHorizontal(),
                this.inputSource.GetVertical()
            );

            HandleFacing();
        }

        protected void HandleFacing()
        {
            if (this.movementInput.x > 0 && !this.facingRight)
            {
                this.Flip();
            }

            if (this.movementInput.x < 0 && this.facingRight)
            {
                this.Flip();
            }
        }

        public void SetInputSource(IInputSource source)
        {
            inputSource = source;
        }

        public void ClearInputSource()
        {
            inputSource = null;
        }

        public void SetControlMode(ControlMode mode)
        {
            controlMode = mode;
        }

        #endregion

        #region Movement

        protected virtual void ApplyMovement()
        {
            if (CurrentControlMode == ControlMode.Encounter)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            if (this.CurrentControlMode != ControlMode.FreeRoam)
            {
                rb.linearVelocity = new Vector2(
                    0f, rb.linearVelocity.y);
                return;
            }

            rb.linearVelocity = new Vector2(
                movementInput.x * moveSpeed * transform.localScale.x, 
                GetComponent<Rigidbody2D>().linearVelocity.y);
        }

        protected void Flip()
        {
            facingRight = !facingRight;

            animator.SetFloat(
                Constants.AnimatorParams.xDirection,
                !facingRight ? -1f : 1f);
        }

        #endregion

        #region Animation

        protected virtual void UpdateAnimator()
        {
            animator.SetFloat(
                Constants.AnimatorParams.Speed, 
                Mathf.Abs(animatorSpeed));
            animator.SetFloat(
                Constants.AnimatorParams.xDirection, 
                facingRight ? 1f : -1f);
        }

        protected void TriggerAction(string triggerName)
        {
            animator.SetTrigger(triggerName);
        }

        #endregion

        #region Collision Detection

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            var otherEntity = other.GetComponent<EntityController>();

            if (otherEntity is null)
            {
                return;
            }

            GameEventBus.Publish(
                new EntityEncounterEvent(this, otherEntity, Time.frameCount));
        }

        public virtual void HandleEncounter(IEncounterable other)
        {
            SetControlMode(ControlMode.Encounter);
        }

        #endregion
    }
}