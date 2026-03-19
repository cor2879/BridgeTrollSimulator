#pragma warning disable CS0618
/**************************************************
 *  CameraManager.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using System.Linq;

    using UnityEngine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Effects;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Interfaces;
    using Unity.Cinemachine;

    /// <summary>
    /// Defines a Singleton class which is used for managing the primary game camera.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class CameraManager : MonoBehaviour
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static CameraManager instance;

        /// <summary>
        /// The <see cref="CameraTargetBehaviour" />
        /// </summary>
        private CameraTargetBehaviour cameraTarget;

        private Camera mainCamera;

        private Camera minimapCamera;

        private ShakeBehaviour shakeBehaviour;

        /// <summary>
        /// Gets the camera target.
        /// </summary>
        /// <value>
        /// The camera target.
        /// </value>
        public CameraTargetBehaviour CameraTarget
        {
            get
            {
                if (this.cameraTarget == null)
                {
                    // this.cameraTarget = Instantiate(GameManager.Instance.cameraTargetPrefab)?.GetComponent<CameraTargetBehaviour>();
                    this.cameraTarget.GameObject.SetActive(false);
                }

                return this.cameraTarget;
            }

            private set
            {
                this.cameraTarget = value;
            }
        }

        public ShakeBehaviour ShakeBehaviour
        {
            get
            {
                if (this.shakeBehaviour == null)
                {
                    this.shakeBehaviour = this.GetComponent<ShakeBehaviour>();
                }

                return this.shakeBehaviour;
            }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static CameraManager Instance { get { return instance; } }

        /// <summary>
        /// Gets the virtual camera.
        /// </summary>
        /// <value>
        /// The virtual camera.
        /// </value>
        public CinemachineVirtualCamera VirtualCamera { get; private set; }

        public Camera MainCamera { get; private set; }

        public Camera MinimapCamera { get; private set; }

        /// <summary>
        /// Gets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public CinemachineFramingTransposer Body { get => this.VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>(); }

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }

            var virtualCameraGameObject = GameObject.FindWithTag(Constants.Cameras.VirtualCamera);
            this.VirtualCamera = virtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();

            this.MainCamera = GameObject.FindWithTag(Constants.Cameras.MainCamera).GetComponent<Camera>();
            this.MinimapCamera = GameObject.FindWithTag(Constants.Cameras.MinimapCamera).GetComponent<Camera>();
        }

        /// <summary>
        /// Follows the specified game object.
        /// </summary>
        /// <param name="gameObject">The game object.</param>
        public static void Follow(GameObject gameObject)
        {
            Instance.VirtualCamera.Follow = gameObject.transform;
        }

        public static void FocusOn(GameObject gameObject)
        {
            Instance.CameraTarget.Target = gameObject;
            Instance.VirtualCamera.Follow = gameObject.transform;
            CameraManager.SetXDamping(0.0f);
            CameraManager.SetYDamping(0.0f);
        }

        public static void InitializeCamera()
        {
            Instance.CameraTarget.GameObject.SetActive(true);
            var position = new Vector2(); // PlayerBehaviour.Instance.transform.position;
            var startingPosition = position;
            Instance.CameraTarget.transform.position = startingPosition;
            // Instance.CameraTarget.DragCameraToTarget(PlayerBehaviour.Instance.gameObject);
        }

        /// <summary>
        /// Transitions the camera from the current target to the specified new <see cref="GameObject" /> in a smooth motion.
        /// </summary>
        /// <param name="gameObject">The game object.</param>
        public static void TransitionTo(GameObject gameObject)
        {
            Instance.CameraTarget.GameObject.SetActive(true);

            var position = Instance.VirtualCamera.Follow != null ? Instance.VirtualCamera.Follow.transform.position : Instance.transform.position;
            Instance.CameraTarget.transform.SetPositionAndRotation(position, Quaternion.identity);
            Instance.CameraTarget.DragCameraToTarget(gameObject);
        }

        /// <summary>
        /// Transitions the camera from the current target to the specified new <see cref="EntityBehaviour" /> in a smooth motion.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public static void TransitionTo(EntityBehaviour entity)
        {
            TransitionTo(entity.gameObject);
        }

        /// <summary>
        /// Sets the x damping.
        /// </summary>
        /// <param name="xDamping">The x damping.</param>
        public static void SetXDamping(float xDamping)
        {
            Instance.Body.m_XDamping = xDamping;
        }

        /// <summary>
        /// Sets the y damping.
        /// </summary>
        /// <param name="yDamping">The y damping.</param>
        public static void SetYDamping(float yDamping)
        {
            Instance.Body.m_YDamping = yDamping;
        }

        /// <summary>
        /// Determines whether the specified game object is following.
        /// </summary>
        /// <param name="gameObject">The game object.</param>
        /// <returns>
        ///   <c>true</c> if the specified game object is following; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFollowing(GameObject gameObject)
        {
            return Instance.VirtualCamera.Follow == gameObject.transform;
        }
    }
}
