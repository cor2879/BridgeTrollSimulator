/**************************************************
 *  CameraTargetBehaviour.cs
 *  
 *  copyright (c) 2023 Old Skool Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using UnityEngine;

    /// <summary>
    /// Defines a behaviour that allows the camera to shake.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class ShakeBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float duration = 0.0f;

        [SerializeField]
        private float magnitude = 0.0f;

        [SerializeField]
        private float dampingSpeed;

        [SerializeField]
        private bool shake = false;

        private Vector3 startingPosition;

        public float Duration
        {
            get => this.duration;
            private set => this.duration = value;
        }

        public float Magnitude
        {
            get => this.magnitude;
            private set => this.magnitude = value;
        }

        public float DampingSpeed
        {
            get => this.dampingSpeed;
            set => this.dampingSpeed = value;
        }

        public bool doShake
        {
            get => shake;
            set => shake = value;
        }

        public Vector3 StartingPosition
        {
            get => this.startingPosition;
            private set => this.startingPosition = value;
        }

        public void StartShake(float duration, float magnitude, float dampingSpeed)
        {
            if (duration < 0.0f)
            {
                duration = 0.0f;
            }

            if (magnitude < 0.0f)
            {
                magnitude = 0.0f;
            }

            if (dampingSpeed < 0.0f)
            {
                dampingSpeed = 0.0f;
            }

            this.StartingPosition = this.transform.localPosition;
            this.Magnitude = magnitude;
            this.DampingSpeed = dampingSpeed;
            this.doShake = true;
        }

        public void StartShake(float duration, float magnitude)
        {
            this.StartShake(duration, magnitude, 0.0f);
        }

        public void StopShake()
        {
            this.doShake = false;
            this.transform.localPosition = this.StartingPosition;
        }

        public void Update()
        {
            if (this.doShake)
            {
                this.transform.localPosition = this.StartingPosition + Random.insideUnitSphere * this.Magnitude;
            }
        }
    }
}
