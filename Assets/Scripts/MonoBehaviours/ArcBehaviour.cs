/**************************************************
 *  ArcBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using System;

    using UnityEngine;

    /// <summary>
    /// Defines a behaviour for enabling a game object to move in a horizontal arc.
    /// </summary>
    /// <seealso cref="MonoBehaviour" />
    public class ArcBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The duration
        /// </summary>
        private float duration;

        /// <summary>
        /// The destination
        /// </summary>
        private Vector3 destination;

        /// <summary>
        /// The rotation axis
        /// </summary>
        private Vector3 rotationAxis;

        /// <summary>
        /// The <see cref="Action" /> to invoke once the arcing object
        /// has reached its destination, if any.
        /// </summary>
        private Action onDestinationReached;

        /// <summary>
        /// The start position
        /// </summary>
        Vector3 startPosition;

        /// <summary>
        /// The percent complete
        /// </summary>
        private float percentComplete = 1.0f;

        /// <summary>
        /// The total percentage
        /// </summary>
        private float totalPercentage = 1.0f;

        /// <summary>
        /// Gets the destination.
        /// </summary>
        /// <value>
        /// The destination.
        /// </value>
        public Vector3 Destination
        {
            get { return this.destination; }
        }

        public float Duration
        {
            get { return this.duration; }
        }

        /// <summary>
        /// Begins the arc.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="rotationAxis">The rotation axis.</param>
        /// <param name="onDestinationReached">The <see cref="Action" /> to invoke once the arcing object
        /// has reached its destination, if any.</param>
        public void BeginArc(Vector3 destination, float duration, Vector3 rotationAxis, Action onDestinationReached)
        {
            this.startPosition = this.transform.position;
            this.duration = duration;
            this.destination = destination;
            this.onDestinationReached = onDestinationReached;
            this.percentComplete = 0.0f;
            this.rotationAxis = rotationAxis;
        }

        /// <summary>
        /// Begins the arc.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="rotationAxis">The rotation axis.</param>
        /// <param name="onDestinationReached">The <see cref="Action" /> to invoke once the arcing object
        /// has reached its destination, if any.</param>
        public void BeginArc(Vector3 destination, float duration, Vector3 rotationAxis, float totalPercentage, Action onDestinationReached)
        {
            this.startPosition = this.transform.position;
            this.duration = duration;
            this.destination = destination;
            this.onDestinationReached = onDestinationReached;
            this.percentComplete = 0.0f;
            this.rotationAxis = rotationAxis;
            this.totalPercentage = totalPercentage;
        }

        public void ContinueArc(Vector3 destination, float duration, Vector3 rotationAxis, Quaternion rotation, float percentComplete, float totalPercentage, Action onDestinationReached)
        {
            this.startPosition = this.transform.position;
            this.duration = duration;
            this.destination = destination;
            this.onDestinationReached = onDestinationReached;
            this.percentComplete = percentComplete;
            this.rotationAxis = rotationAxis;
            this.totalPercentage = totalPercentage;
            this.transform.rotation = rotation;
        }

        /// <summary>
        /// Updates the object state at a fixed interval determined by the Unity runtime.
        /// </summary>
        public void FixedUpdate()
        {
            if (GameManager.Instance.PauseAction)
            {
                return;
            }

            if (percentComplete < this.totalPercentage)
            {
                percentComplete += Time.fixedDeltaTime / duration;
                var currentHeight = Mathf.Sin(Mathf.PI * percentComplete);
                this.transform.position = Vector3.Lerp(startPosition, destination, percentComplete) +
                    Vector3.up * currentHeight;

                this.transform.Rotate(0, 0, rotationAxis.z / 2, Space.Self);

                if (percentComplete >= this.totalPercentage)
                {
                    onDestinationReached?.Invoke();

                    this.Reset();
                }
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            this.startPosition = this.transform.position;
            this.duration = 0.0f;
            this.onDestinationReached = null;
            this.percentComplete = 1.0f;
        }
    }
}