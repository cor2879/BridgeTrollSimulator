/**************************************************
 *  FeedbackManager.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using UnityEngine;
#if UNITY_STEAM
    using Steamworks;
#endif
#if XINPUT
    using XInputDotNetPure;
#endif

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;

    /// <summary>
    /// Handles input feedback such as vibration.  Singleton.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class FeedbackManager : MonoBehaviour
    {
        /// <summary>
        /// The default vibration duration used if none is specified.
        /// </summary>
        private const float quickVibrationDuration = 0.25f;

        /// <summary>
        /// The duration of the vibration.
        /// </summary>
        [SerializeField, ReadOnly]
        private float duration = 0.0f;

        /// <summary>
        /// The vibration intensity
        /// </summary>
        [SerializeField, ReadOnly]
        private float vibrationIntensity;

        /// <summary>
        /// The instance
        /// </summary>
        private static FeedbackManager instance = null;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static FeedbackManager Instance
        {
            get { return instance; }
            private set { instance = value; }
        }

        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public float Duration
        {
            get { return this.duration; }
            private set { this.duration = value; }
        }

        /// <summary>
        /// Executes when this instance is awakened.
        /// </summary>
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

        public void Start()
        {
#if XINPUT
            Debug.Log("XInput detected");
            Debug.Log($"PlayerIndex One: {GamePad.GetState(PlayerIndex.One).IsConnected}");
            Debug.Log($"PlayerIndex Two: {GamePad.GetState(PlayerIndex.Two).IsConnected}");
            Debug.Log($"PlayerIndex Three: {GamePad.GetState(PlayerIndex.Three).IsConnected}");
            Debug.Log($"PlayerIndex Four: {GamePad.GetState(PlayerIndex.Four).IsConnected}");
#endif
        }

        /// <summary>
        /// Updates the state of this instance on a fixed interval determined by the Unity Engine at runtime.
        /// </summary>
        public void FixedUpdate()
        {
            if (!Settings.EnableVibration)
            {
                this.Duration = 0.0f;
            }

            if (this.Duration > float.Epsilon)
            {
                this.Duration -= Time.fixedDeltaTime;
            }
            else
            {
                this.vibrationIntensity = 0.0f;
            }
#if XINPUT
            GamePad.SetVibration(PlayerIndex.One, this.vibrationIntensity, this.vibrationIntensity);
#endif
        }

        /// <summary>
        /// Starts the vibration.
        /// </summary>
        /// <param name="duration">The duration in seconds.</param>
        /// <param name="intensity">The intensity.</param>
        public void StartVibration(float duration, float intensity)
        {
            if (Settings.EnableVibration)
            {
                this.Duration = Mathf.Max(this.Duration, this.Duration + duration);
                this.vibrationIntensity = intensity;
            }
        }

        /// <summary>
        /// Starts a quick vibration that lasts for the default time specified by the
        /// quickVibrationDuration constant.
        /// </summary>
        /// <param name="intensity">The intensity.</param>
        public void QuickVibration(float intensity)
        {
            this.StartVibration(quickVibrationDuration, intensity);
        }
    }
}
