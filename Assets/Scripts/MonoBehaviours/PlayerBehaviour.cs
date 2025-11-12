#pragma warning disable CS0649
/**************************************************
 *  PlayerBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.EventSystems;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours.GameplayManagement;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    /// <summary>
    /// Defines the state and behaviours for the Player object
    /// </summary>
    /// <seealso cref="OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours.EntityBehaviour" />
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(InventoryBehaviour))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerBehaviour : EntityBehaviour
    {
        [SerializeField, ReadOnly]
        private int moveCount;

        /// <summary>
        /// Indicates whether or not this instance is currently in the Walking state.
        /// </summary>
        [SerializeField, ReadOnly]
        private bool isWalking;

        /// <summary>
        /// Indicates whether or not this instance is currently in the Firing state.
        /// </summary>
        [SerializeField, ReadOnly]
        private bool isFiring;

        /// <summary>
        /// The animator
        /// </summary>
        private Animator animator;

        /// <summary>
        /// The movement behaviour
        /// </summary>
        private MovementBehaviour movementBehaviour;

        /// <summary>
        /// The inventory behaviour
        /// </summary>
        private InventoryBehaviour inventoryBehaviour;

        /// <summary>
        /// The can move
        /// </summary>
        [SerializeField, ReadOnly]
        private bool canMove = true;

        /// <summary>
        /// The Sprite Renderer
        /// </summary>
        [SerializeField, ReadOnly]
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// The lock input
        /// </summary>
        private bool lockInput;

        [SerializeField, ReadOnly]
        private bool hasCameraFocus;

        /// <summary>
        /// Gets the movement behaviour.
        /// </summary>
        /// <value>
        /// The movement behaviour.
        /// </value>
        public MovementBehaviour MovementBehaviour
        {
            get
            {
                if (this.movementBehaviour == null)
                {
                    this.movementBehaviour = MovementBehaviour.GetMovementBehaviour(this);
                }

                return this.movementBehaviour;
            }
        }

        /// <summary>
        /// Gets the inventory behaviour.
        /// </summary>
        /// <value>
        /// The inventory behaviour.
        /// </value>
        public InventoryBehaviour Inventory
        {
            get
            {
                if (this.inventoryBehaviour == null)
                {
                    this.inventoryBehaviour = this.GetComponent<InventoryBehaviour>();
                }

                return this.inventoryBehaviour;
            }
        }

        /// <summary>
        /// Gets the animator.
        /// </summary>
        /// <value>
        /// The animator.
        /// </value>
        public Animator Animator
        {
            get
            {
                if (this.animator == null)
                {
                    try
                    {
                        this.animator = this.GetComponent<Animator>();
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }

                return this.animator;
            }
        }

        /// <summary>
        /// Gets the Sprite Renderer
        /// </summary>
        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (this.spriteRenderer == null)
                {
                    this.spriteRenderer = this.GetComponent<SpriteRenderer>();
                }

                return this.spriteRenderer;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can move.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can move; otherwise, <c>false</c>.
        /// </value>
        public bool CanMove
        {
            get => this.canMove;
            set => this.canMove = value;
        }

        /// <summary>
        /// The instance
        /// </summary>
        private static PlayerBehaviour instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static PlayerBehaviour Instance { get => instance; }

        public bool HasCameraFocus { get => this.hasCameraFocus; private set => this.hasCameraFocus = value; }

        public bool IsDying { get; private set; } = false;

        public bool IsVisible { get => this.SpriteRenderer != null && this.SpriteRenderer.enabled; }

        /// <summary>
        /// Gets a value indicating whether this instance is walking.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is walking; otherwise, <c>false</c>.
        /// </value>
        public bool IsWalking { get => this.isWalking; }

        public bool IsIdle
        {
            get => IsVisible && !IsWalking && CanMove && !IsFalling && !IsDying;
        }

        public bool IsFalling { get; set; } = false;

        /// <summary>
        /// Gets a value indicating whether this instance is firing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is firing; otherwise, <c>false</c>.
        /// </value>
        public bool IsFiring { get => this.isFiring; private set => this.isFiring = value; }

        public int MoveCount { get => this.moveCount; private set => this.moveCount = value; }

        public override IList<string> MovementSounds { get => SoundClips.PlayerFootsteps; }

        /// <summary>
        /// Gets the death sound.
        /// </summary>
        /// <param name="killer">The killer.</param>
        /// <returns></returns>
        public string GetDeathSound(EntityBehaviour killer)
        {
            return SoundClips.Wilhelm;
        }

        /// <summary>
        /// Executes when this instance is awakened
        /// </summary>
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        /// <summary>
        /// Updates this instance when each frame is updated by the Unity Engine.
        /// </summary>
        public void FixedUpdate()
        {
            this.HasCameraFocus = CameraManager.Instance.CameraTarget.IsFocusedOnTarget(this.gameObject);

            this.Animator.SetBool(Constants.IsFiring, false);

            if (GameManager.Instance.PauseAction)
            {
                return;
            }

            // TODO: figure out what to do about the Crown
            //
            // (this.crownBehaviour != null ?
            //     (Action)GameManager.Instance.CrownInventoryPanel.Enable :
            //     GameManager.Instance.CrownInventoryPanel.Disable).Invoke();

            this.isWalking = this.Animator.GetBool(Constants.IsWalking);

            if (!CameraManager.IsFollowing(this.gameObject))
            {
                return;
            }
        }

        public void IncrementMoveCount()
        {
        
        }

        /// <summary>
        /// Sets the parent.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public void SetParent(Transform parent)
        {
            this.gameObject.transform.SetParent(parent);
        }

        /// <summary>
        /// Stops the walking.
        /// </summary>
        public void StopWalking()
        {
            this.isWalking = false;
            this.MovementBehaviour.StopMotion();
        }

        /// <summary>
        /// Attempts to Kill this instance.  Whether or not the kill is successful may be determined
        /// by the implementation code of this method.
        /// </summary>
        /// <param name="options">The options.</param>
        public override void Kill(KillOptions options)
        {
            this.IsDying = true;

            options?.OnKill?.Invoke(this);

            if (options != null && options.HideSpriteRenderer)
            {
                this.SpriteRenderer.enabled = false;
            }

            StartCoroutine(nameof(PlayerBehaviour.KillPlayer), options);
        }

        /// <summary>
        /// Kills the player.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        private IEnumerator KillPlayer(KillOptions options)
        {
            if (this.isWalking)
            {
                this.StopWalking();
            }

            if (!string.IsNullOrWhiteSpace(options?.AudioClip))
            {
                GameManager.Instance.SoundEffectManager.PlayAudioOnce(options.AudioClip);
                yield return new WaitForSeconds(GameManager.Instance.SoundEffectManager.GetAudioClip(options.AudioClip).length / 2);
            }

            GameManager.Instance.MusicManager.PlayDistinctAudioOnce(SoundClips.Defeat);

            yield return new WaitForSeconds(GameManager.Instance.MusicManager.GetAudioClip(SoundClips.Defeat).length);

            // GameManager.Instance.GameOver(options.GameOverCondition);

            base.Kill(options);
        }

        public void SetAnimatorValue(string parameter, bool value)
        {
            this.Animator?.SetBool(parameter, value);
        }
    }
}
