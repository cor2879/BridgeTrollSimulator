/**************************************************
 *  ShakeBehaviour.cs
 *  
 *  copyright (c) 2023 Old Skool Games
 **************************************************/
 using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Effects
{
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
        private float dampingSpeed = 1f;

        [SerializeField, ReadOnly]
        private bool shaking;
        [SerializeField, ReadOnly]
        private float remainingTime;
        private Vector3 startingPosition;

        public void StartShake(float duration, float magnitude, float dampingSpeed = 1f)
        {
            this.duration = Mathf.Max(0f, duration);
            this.magnitude = Mathf.Max(0f, magnitude);
            this.dampingSpeed = Mathf.Max(0f, dampingSpeed);

            startingPosition = this.transform.localPosition;
            remainingTime = this.duration;
            this.shaking = true;
        }

        public void StopShake()
        {
            shaking = false;
            transform.localPosition = startingPosition;
        }

        public void Update()
        {
            if (!shaking)
            {
                return;
            }

            if (remainingTime > 0)
            {
                transform.localPosition = 
                    startingPosition +
                    Random.insideUnitSphere * magnitude;

                remainingTime -= Time.unscaledDeltaTime;
                magnitude = Mathf.Lerp(magnitude, 0f, dampingSpeed * Time.unscaledDeltaTime);
            }
            else
            {
                StopShake();
            }
        }
    }
}
