#pragma warning disable CS0649
/**************************************************
 *  FootstepBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Interfaces;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    /// <summary>
    /// Enables footsteps to play when a GameEntity moves from one spot to another.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(Animator))]
    public class FootstepBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The animator
        /// </summary>
        [SerializeField, ReadOnly]
        private Animator animator;

        [SerializeField, ReadOnly]
        private EntityBehaviour entity;

        /// <summary>
        /// The interval
        /// </summary>
        [SerializeField, ReadOnly]
        private float interval = 0;

        /// <summary>
        /// The duration
        /// </summary>
        [SerializeField, ReadOnly]
        private float duration = 0.0f;

        [SerializeField, ReadOnly]
        private int currentClipIndex = 0;

        /// <summary>
        /// Gets the animator.
        /// </summary>
        /// <value>
        /// The animator.
        /// </value>
        private Animator Animator
        {
            get
            {
                if (this.animator == null)
                {
                    this.animator = this.GetComponent<Animator>();
                }

                return this.animator;
            }
        }

        public EntityBehaviour Entity
        {
            get
            {
                if (this.entity == null)
                {
                    this.entity = this.GetComponent<EntityBehaviour>();
                }

                return this.entity;
            }
        }

        public int CurrentClipIndex
        {
            get => this.currentClipIndex;
            private set => this.currentClipIndex = value;
        }

        public IList<string> MovementSounds { get => this.Entity.MovementSounds; }

        /// <summary>
        /// Gets or sets the interval in seconds since the last footstep audio clip was started.
        /// </summary>
        /// <value>
        /// The interval.
        /// </value>
        private float Interval
        {
            get => this.interval;
            set => this.interval = value;
        }

        /// <summary>
        /// Gets or sets the duration of the footstep audio clip.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        private float Duration
        {
            get
            {
                if (this.duration == 0.0f)
                {
                    this.duration = GameManager.Instance.SoundEffectManager.GetAudioClip(this.MovementSounds[this.CurrentClipIndex]).length;
                }

                return this.duration;
            }

            set => this.duration = value;
        }

        /// <summary>
        /// Executes on a fixed interval which is determined by the UnityEngine at runtime.
        /// </summary>
        public void FixedUpdate()
        {
            if (!GameManager.Instance.PauseAction && this.Animator.GetBool(Constants.IsWalking))
            {
                if (this.Interval >= this.Duration)
                {
                    this.Interval = 0;
                }

                if (this.Interval < float.Epsilon)
                {
                    if (PlayerBehaviour.Instance != null && this.Entity.IsNear(PlayerBehaviour.Instance))
                    {
                        this.PlayFootStepSound();
                    }
                }

                this.Interval += Time.fixedDeltaTime;
            }
        }

        /// <summary>
        /// Plays the footstep sound.
        /// </summary>
        private void PlayFootStepSound()
        {
            ++this.CurrentClipIndex;
            this.CurrentClipIndex = this.CurrentClipIndex % this.MovementSounds.Count;
            var distanceVector = this.transform.position - PlayerBehaviour.Instance.transform.position;
            var distance = Math.Max(Math.Abs(distanceVector.x), Math.Abs(distanceVector.y));
            var inverse = Constants.HearingRange - distance;

            if (inverse > 0)
            {
                var volume = inverse / Constants.HearingRange;
                GameManager.Instance.SoundEffectManager.PlayAudioOnceAtVolume(this.MovementSounds[this.CurrentClipIndex], volume);
            }
        }
    }
}
