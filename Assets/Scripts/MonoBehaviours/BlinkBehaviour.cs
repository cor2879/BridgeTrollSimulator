#pragma warning disable CS0649
/**************************************************
 *  BlinkBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using UnityEngine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;

    /// <summary>
    /// Causes the attached <see cref="GameObject" /> to blink
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(SpriteRenderer))]
    public class BlinkBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The sprite renderer
        /// </summary>
        [SerializeField, ReadOnly]
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// The interval in seconds between blinks
        /// </summary>
        [SerializeField]
        private float blinkInterval;

        [SerializeField]
        private bool synchronized = true;

        /// <summary>
        /// Determines whether or not the attached <see cref="GameObject" /> is currently visible
        /// </summary>
        [SerializeField, ReadOnly]
        private bool isVisible;

        /// <summary>
        /// The time delta since the last blink state change
        /// </summary>
        [SerializeField, ReadOnly]
        private float timeDelta = 0.0f;

        [SerializeField]
        private bool blink = true;

        public bool Blink { get => blink; private set => blink = value; }

        /// <summary>
        /// Gets the sprite renderer.
        /// </summary>
        /// <value>
        /// The sprite renderer.
        /// </value>
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

        public bool Enabled
        {
            get => this.gameObject.activeInHierarchy;
            set => this.gameObject.SetActive(value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisible
        {
            get { return this.isVisible; }
            private set { this.isVisible = value; }
        }

        /// <summary>
        /// Executes on a fixed interval which is determined by the Unity Engine at runtime.
        /// </summary>
        public void FixedUpdate()
        {
            if (this.Blink)
            {
                if (this.synchronized)
                {
                    if (Blinkronizer.Instance.TimeDelta > blinkInterval)
                    {
                        timeDelta = 0.0f;
                        this.SpriteRenderer.enabled = Blinkronizer.Instance.BlinkOn;
                    }
                }
                else
                {
                    if (this.timeDelta > this.blinkInterval)
                    {
                        this.timeDelta = 0.0f;
                        this.SpriteRenderer.enabled = !this.SpriteRenderer.enabled;
                    }

                    this.timeDelta += Time.fixedDeltaTime;
                }
            }
        }

        public void StartBlinking()
        {
            this.Blink = true;
        }

        public void StopBlinking()
        {
            this.Blink = false;
            this.SpriteRenderer.enabled = true;
        }
    }
}
