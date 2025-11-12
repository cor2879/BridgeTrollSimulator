#pragma warning disable CS0649
/******************************************************
 *  Blinkronizer.cs
 *  
 *  copyright 2021 Old School Games
 *  
 *  For synchronizing multiple blink behaviour instances
 *******************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using UnityEngine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;

    public class Blinkronizer : MonoBehaviour
    {
        /// <summary>
        /// The interval in seconds between blinks
        /// </summary>
        [SerializeField]
        private float blinkInterval;

        [SerializeField, ReadOnly]
        private bool blinkOn;

        [SerializeField, ReadOnly]
        private float timeDelta = 0.0f;

        public static Blinkronizer Instance { get; private set; }

        public bool BlinkOn { get => this.blinkOn; private set => this.blinkOn = value; }

        public float TimeDelta { get => this.timeDelta; private set => this.timeDelta = value; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public void FixedUpdate()
        {
            if (this.TimeDelta > this.blinkInterval)
            {
                this.TimeDelta = 0.0f;
                this.BlinkOn = !this.BlinkOn;
            }

            this.TimeDelta += Time.fixedDeltaTime;
        }
    }
}
