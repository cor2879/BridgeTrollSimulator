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

        [Header("Character Stats")]
        [SerializeField]
        protected string entityName;
        [SerializeField]
        protected int gold;
        [SerializeField]
        protected int maxHealth;
        [SerializeField, ReadOnly]
        protected int currentHealth;
        [SerializeField]
        protected int attack = 3;
        [SerializeField]
        protected int defense = 1;

        [SerializeField, ReadOnly]
        protected EntityType entityType;

        [SerializeField, ReadOnly]
        protected Guid instanceId;

        [SerializeField]
        protected ControlMode controlMode = ControlMode.FreeRoam;

        [SerializeField]
        protected string sourceName;

        [Header("Movement")]
        [SerializeField] 
        private float moveSpeed = 1.0f;

        [SerializeField]
        protected float animatorSpeed = 0.0f;

        private Vector2 movementInput;
        private bool facingRight = true;

        #region Properties

        public abstract ControlMode DefaultControlMode { get; }
        public GameObject GameObject => this.gameObject;
        public ControlMode CurrentControlMode => controlMode;
        public IInputSource InputSource => inputSource;
        public string Name { get => entityName; set => this.entityName = value;}
        public int Gold => gold;
        public int MaxHealth => maxHealth;
        public int CurrentHealth { get => currentHealth; set => this.currentHealth = value; }
        public int Attack => attack;
        public int Defense => defense;

        public GameSystemType SystemType => GameSystemType.Entity;

        public string SourceName => sourceName;

        #endregion

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            instanceId = Guid.NewGuid();
            currentHealth = maxHealth;
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

        public void ResetControlMode()
        {
            controlMode = DefaultControlMode;
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
            if (otherEntity == null)
            {
                return;
            }

            var thisEntity = GetComponent<EntityController>();
            if (thisEntity == null)
            {
                return;
            }

            EntityController player = null;
            EntityController npc = null;

            if (thisEntity.CompareTag("Troll"))
            {
                player = thisEntity;
            }
            else if (otherEntity.CompareTag("Troll"))
            {
                player = otherEntity;
            }

            if (thisEntity.CompareTag("NPC"))
            {
                npc = thisEntity;
            }
            else if (otherEntity.CompareTag("NPC"))
            {
                npc = otherEntity;
            }

            if (player == null || npc == null)
            {
                return;
            }

            GameEventBus.Publish(
                new EntityEncounterEvent(player, npc, Time.frameCount));
        }

        public virtual void HandleEncounter(IEncounterable other)
        {
            SetControlMode(ControlMode.Encounter);
        }

        #endregion
    
        #region Interaction

        public virtual int DeductGold(int amount)
        {
            var goldReturned = amount;

            if (this.gold < amount)
            {
                goldReturned = this.gold;
                this.gold = 0;
            }
            else
            {
                this.gold -= amount;
            }
            
            return goldReturned;
        }

        public virtual void AddGold(int amount)
        {
            this.gold += amount;
        }

        #endregion
    }
}