#pragma warning disable CS0649
/**************************************************
 *  BleedBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using System.Collections.Generic;

    using UnityEngine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;

    /// <summary>
    /// Defines a behaviour for game objects that start to bleed.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(SpawnerBehaviour))]
    public class BleedBehaviour : MonoBehaviour
    {
        private static readonly Vector3 IdlePointOffsetVector = new Vector3(0.0f, -0.4f, 0.0f);

        [SerializeField]
        private EntityBehaviour entityBehaviour;

        /// <summary>
        /// The interval in seconds between bleeds
        /// </summary>
        [SerializeField]
        private float bleedInterval;

        /// <summary>
        /// The the interval in seconds that counts the time since the last bleed
        /// </summary>
        [SerializeField, ReadOnly]
        private float interval = 0.0f;

        /// <summary>
        /// Determines whether or not bleeding should occur.
        /// </summary>
        [SerializeField]
        private bool letTheBleedingCommence;

        /// <summary>
        /// The collection for keeping track of all the blood splatter
        /// </summary>
        [SerializeField, ReadOnly]
        private List<BloodSplatterBehaviour> bloodSplatter = new List<BloodSplatterBehaviour>();

        /// <summary>
        /// The spawner behaviour
        /// </summary>
        [SerializeField, ReadOnly]
        private SpawnerBehaviour spawnerBehaviour;

        /// <summary>
        /// Gets a value indicating whether or not bleeding should occur.
        /// </summary>
        /// <value>
        ///   <c>true</c> if bleeding should occur; otherwise, <c>false</c>.
        /// </value>
        public bool LetTheBleedingCommence
        {
            get { return this.letTheBleedingCommence; }
            private set { this.letTheBleedingCommence = value; }
        }

        /// <summary>
        /// Gets the interval in seconds that counts the time since the last bleed
        /// </summary>
        /// <value>
        /// The interval.
        /// </value>
        public float Interval
        {
            get { return this.interval; }
            private set { this.interval = value; }
        }

        /// <summary>
        /// Gets the interval in seconds between bleeds
        /// </summary>
        /// <value>
        /// The bleed interval.
        /// </value>
        public float BleedInterval
        {
            get { return this.bleedInterval; }
        }

        public EntityBehaviour EntityBehaviour
        {
            get
            {
                if (this.entityBehaviour == null)
                {
                    this.entityBehaviour = this.GetComponent<EntityBehaviour>();
                }

                return this.entityBehaviour;
            }
        }

        /* public Vector2 CurrentDestination
        {
            get => this.MovementBehaviour.CurrentDestination;
        } */

        public Vector2 CurrentPosition
        {
            get => this.EntityBehaviour.Position;
        }

        /// <summary>
        /// Gets the spawner behaviour.
        /// </summary>
        /// <value>
        /// The spawner behaviour.
        /// </value>
        public SpawnerBehaviour SpawnerBehaviour
        {
            get
            {
                if (this.spawnerBehaviour == null)
                {
                    this.spawnerBehaviour = this.GetComponent<SpawnerBehaviour>();
                }

                return this.spawnerBehaviour;
            }
        }

        /// <summary>
        /// Gets the collection for keeping track of all the blood splatter
        /// </summary>
        /// <value>
        /// The blood splatter.
        /// </value>
        public List<BloodSplatterBehaviour> BloodSplatter
        {
            get { return this.bloodSplatter; }
        }

        /// <summary>
        /// Executes code on a fixed interval which is determined by the Unity Engine at runtime.
        /// </summary>
        public void FixedUpdate()
        {
            if (this.EntityBehaviour.IsTrapped)
            {
                this.LetTheBleedingCommence = false;
            }

            if (this.LetTheBleedingCommence)
            {
                if (this.Interval >= this.BleedInterval)
                {
                    this.BleedForMe();
                    this.Interval = 0.0f;
                }
                else
                {
                    this.Interval += Time.fixedDeltaTime;
                }
            }
        }

        /// <summary>
        /// Starts the bleeding.
        /// </summary>
        public void StartTheBleeding()
        {
            this.BleedForMe();
            this.Interval = 0.0f;
            this.LetTheBleedingCommence = true;
        }

        public void BleedForMe()
        {
            // var bloodSplatterSelector = Random.Range(0, GameManager.Instance.bloodSplatterPrefabs.Length);
            // var bloodSplatter = this.SpawnerBehaviour.SpawnObject(GameManager.Instance.bloodSplatterPrefabs[bloodSplatterSelector]).GetComponent<BloodSplatterBehaviour>();
            // bloodSplatter.transform.SetParent(this.CurrentDestination ?? this.CurrentPosition);
            // bloodSplatter.transform.SetPositionAndRotation(bloodSplatter.transform.position + BleedBehaviour.IdlePointOffsetVector, Quaternion.identity);
            // this.BloodSplatter.Add(bloodSplatter);
        }

        /// <summary>
        /// Stops the bleeding.
        /// </summary>
        public void StopTheBleeding()
        {
            this.LetTheBleedingCommence = false;
        }

        /// <summary>
        /// Cleans up blood.
        /// </summary>
        public void CleanUpBlood()
        {
            foreach (var bloodSplatter in this.BloodSplatter)
            {
                bloodSplatter.transform.SetParent(null);
                bloodSplatter.Destroy();
            }

            this.BloodSplatter.Clear();
        }
    }
}
