/**************************************************
 *  BloodSplatterBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using UnityEngine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;

    /// <summary>
    /// Defines the behaviour for Blood Splatter
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(SpriteRenderer))]
    public class BloodSplatterBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The sprite renderer
        /// </summary>
        [SerializeField, ReadOnly]
        private SpriteRenderer spriteRenderer;

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

        /// <summary>
        /// Destroys this instance.
        /// </summary>
        public void Destroy()
        {
            if (this.SpriteRenderer != null)
            {
                this.SpriteRenderer.enabled = false;
            }

            Destroy(this);
        }
    }
}
