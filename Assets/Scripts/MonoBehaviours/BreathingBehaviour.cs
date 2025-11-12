#pragma warning disable CS0649
#pragma warning disable IDE003
/**************************************************
 *  FootstepBehaviour.cs
 *  
 *  copyright (c) 2023 Old Skool Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;

    public class BreathingBehaviour : MonoBehaviour
    {
        private EntityBehaviour entity;

        [SerializeField, ReadOnly]
        private bool canBreathe = true;

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

        public IList<string> IdleSounds { get => this.Entity.IdleSounds; }

        public int CurrentClipIndex
        {
            get => this.currentClipIndex;
            private set => this.currentClipIndex = value;
        }

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
                    this.duration = GameManager.Instance.SoundEffectManager.GetAudioClip(this.IdleSounds[this.CurrentClipIndex]).length;
                }

                return this.duration;
            }

            set => this.duration = value;
        }

        public bool CanBreathe { get => this.canBreathe; set => this.canBreathe = value; }

        public void Start()
        {
            this.CanBreathe = true;
        }

        /// <summary>
        /// Executes on a fixed interval which is determined by the UnityEngine at runtime.
        /// </summary>
        public void FixedUpdate()
        {
            if (GameManager.Instance == null)
            {
                return;
            }

            if (!GameManager.Instance.PauseAction &&
                this.CanBreathe &&
                (CameraManager.Instance.CameraTarget.Target == this.gameObject || (PlayerBehaviour.Instance != null && this.Entity.IsAdjacent(PlayerBehaviour.Instance))))
            {
                if (this.Interval >= this.Duration)
                {
                    this.Interval = 0;
                }

                if (this.Interval < float.Epsilon)
                {
                    if (PlayerBehaviour.Instance != null && this.Entity.IsNear(PlayerBehaviour.Instance))
                    {
                        this.PlayIdleSound();
                    }
                }

                this.Interval += Time.fixedDeltaTime;
            }
        }

        /// <summary>
        /// Plays the footstep sound.
        /// </summary>
        private void PlayIdleSound()
        {
            ++this.CurrentClipIndex;
            this.CurrentClipIndex %= this.IdleSounds.Count;
            var distanceVector = this.transform.position - PlayerBehaviour.Instance.transform.position;
            var distance = Math.Max(Math.Abs(distanceVector.x), Math.Abs(distanceVector.y));

            var inverse = Constants.HearingRange - distance;

            if (inverse > 0)
            {
                var volume = inverse / Constants.HearingRange;
                // Debug.Log($"Breathing volume {volume}");
                GameManager.Instance.SoundEffectManager.PlayAudioOnceAtVolume(this.IdleSounds[this.CurrentClipIndex], volume);
            }
        }
    }
}
