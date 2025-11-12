/**************************************************
 *  CameraTargetBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using UnityEngine;

    /// <summary>
    /// Defines a behaviour that allows the camera to transition between targets with smooth motions.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(SpriteRenderer))]
    public class CameraTargetBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The velocity
        /// </summary>
        [SerializeField]
        private float velocity;

        /// <summary>
        /// Gets or sets the velocity.
        /// </summary>
        /// <value>
        /// The velocity.
        /// </value>
        public float Velocity
        {
            get { return this.velocity; }
            set { this.velocity = value; }
        }

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public GameObject Target { get; set; }

        /// <summary>
        /// Gets the game object.
        /// </summary>
        /// <value>
        /// The game object.
        /// </value>
        public GameObject GameObject { get => this.gameObject; }

        /// <summary>
        /// Drags the camera to target.
        /// </summary>
        /// <param name="target">The target.</param>
        public void DragCameraToTarget(GameObject target)
        {
            this.Target = target;
            CameraManager.Follow(this.gameObject);
            CameraManager.SetXDamping(1.0f);
            CameraManager.SetYDamping(1.0f);
        }

        public bool IsFocusedOnTarget(GameObject target)
        {
            return CameraManager.IsFollowing(this.gameObject) &&
                Vector2.Distance(this.transform.position, target.transform.position) < 0.25f;
        }

        /// <summary>
        /// Executes on a fixed interval which is determined by the Unity Engine at runtime.
        /// </summary>
        public void FixedUpdate()
        {
            if (this.Target != null)
            {
                if (this.transform.position != this.Target.transform.position)
                {
                    var movementVector = this.GetMovementVector();
                    this.transform.SetPositionAndRotation(this.transform.position + movementVector, Quaternion.identity);
                }
                else
                {
                    //CameraManager.Follow(this.Target);
                    //CameraManager.SetXDamping(1.0f);
                    //CameraManager.SetYDamping(1.0f);
                    //this.Target = null;
                }
            }
        }

        /// <summary>
        /// Gets the movement vector.
        /// </summary>
        /// <returns></returns>
        private Vector3 GetMovementVector()
        {
            float yValue = 0.0f;
            float xValue = 0.0f;

            if (this.Target.transform.position.y > this.transform.position.y)
            {
                yValue = Mathf.Min(this.velocity, this.Target.transform.position.y - this.transform.position.y);
            }
            else if (this.Target.transform.position.y < this.transform.position.y)
            {
                yValue = Mathf.Max(this.velocity * -1, this.Target.transform.position.y - this.transform.position.y);
            }

            if (this.Target.transform.position.x > this.transform.position.x)
            {
                xValue = Mathf.Min(this.velocity, this.Target.transform.position.x - this.transform.position.x);
            }
            else if (this.Target.transform.position.x < this.transform.position.x)
            {
                xValue = Mathf.Max(this.velocity * -1, this.Target.transform.position.x - this.transform.position.x);
            }

            return new Vector3(xValue, yValue, 0);
        }

        /// <summary>
        /// Called when the destination reached.
        /// </summary>
        public void OnDestinationReached()
        {
            CameraManager.Follow(this.Target);
        }
    }
}
