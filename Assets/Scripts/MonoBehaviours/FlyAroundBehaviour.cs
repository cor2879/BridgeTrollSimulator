/**************************************************
 *  FlyAroundBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using UnityEngine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Extensions;

    public class FlyAroundBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The hover radius
        /// </summary>
        public float hoverRadius;

        /// <summary>
        /// The hover speed
        /// </summary>
        public float hoverSpeed;

        /// <summary>
        /// The direction
        /// </summary>
        public Vector2 direction;

        /// <summary>
        /// The origin
        /// </summary>
        [SerializeField, ReadOnly]
        private Vector3 origin;

        /// <summary>
        /// Gets or sets the origin.
        /// </summary>
        /// <value>
        /// The origin.
        /// </value>
        public Vector3 Origin { get => origin; set => origin = value; }

        /// <summary>
        /// Executed during the Start event of the object lifecycle.
        /// </summary>
        public void Start()
        {
            this.origin = this.gameObject.transform.position;
        }

        /// <summary>
        /// Executes on a fixed interval which is determined by the Unity Engine.
        /// </summary>
        public void FixedUpdate()
        {
            this.FlyAround();
        }

        /// <summary>
        /// Chooses the direction.
        /// </summary>
        private void ChooseDirection()
        {
            var currentPosition = this.gameObject.transform.position;
            var offsetPosition = currentPosition - this.origin;

            if (direction.IsBelowTolerance(0.5f))
            {
                direction.x = Random.Range(-1f, 1f);
                direction.y = Random.Range(-1f, 2f);
            }

            if (this.direction.IsMovingEast() && offsetPosition.x >= this.hoverRadius)
            {
                this.direction.x = Random.Range(-1f, 1f);
                this.direction.y = Random.Range(-1f, 1f);
            }
            else if (this.direction.IsMovingWest() && offsetPosition.x <= this.hoverRadius * -1)
            {
                this.direction.x = Random.Range(0f, 1f);
                this.direction.y = Random.Range(-1f, 1f);
            }

            if (this.direction.IsMovingNorth() && offsetPosition.y >= this.hoverRadius)
            {
                this.direction.x = Random.Range(-1f, 1f);
                this.direction.y = Random.Range(-1f, 1f);
            }
            else if (this.direction.IsMovingSouth() && offsetPosition.y <= this.hoverRadius * -1)
            {
                this.direction.x = Random.Range(-1f, 1f);
                this.direction.y = Random.Range(0, 1f);
            }
        }

        /// <summary>
        /// Moves this instance.
        /// </summary>
        private void Move()
        {
            var currentPosition = this.gameObject.transform.position;
            var movementVector = new Vector3(
                this.hoverSpeed * this.hoverRadius * this.direction.x,
                this.hoverSpeed * this.hoverRadius * this.direction.y,
                0);
            this.gameObject.transform.SetPositionAndRotation(currentPosition + movementVector, Quaternion.identity);
        }

        /// <summary>
        /// Flies around.
        /// </summary>
        private void FlyAround()
        {
            this.ChooseDirection();
            this.Move();
        }
    }
}
