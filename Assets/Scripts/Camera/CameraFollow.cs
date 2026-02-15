using System;
using System.Collections.Generic;
using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField]
        private Transform target;

        [Header("Follow Settings")]
        [SerializeField]
        private float smoothTime = 0.2f;
        [SerializeField]
        private bool followX = true;
        [SerializeField]
        private bool followY = true;

        [Header("Dead Zone")]
        [SerializeField]
        private Vector2 deadZoneSize = new Vector2(1.5f, 1.0f);

        [Header("Look Ahead")]
        [SerializeField]
        private float lookAheadDistance = 1.5f;
        [SerializeField]
        private float lookAheadSmoothing = 3f;

        [Header("Bounds (Optional)")]
        [SerializeField]
        private bool useBounds = false;
        [SerializeField]
        private Vector2 minBounds;
        [SerializeField]
        private Vector2 maxBounds;

        [SerializeField]
        private Vector3 offset = new Vector3(0f, 0f, 0f);

        private Vector3 velocity = Vector3.zero;
        private float currentLookAhead;
        private float targetLookAhead;

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            var currentPosition = transform.position;
            var desiredPosition = transform.position;

            // Dead Zone Check
            var delta = target.position - currentPosition;

            if (followX && Mathf.Abs(delta.x) > deadZoneSize.x * 0.5f)
            {
                desiredPosition.x = target.position.x;
            }

            if (followY && Mathf.Abs(delta.x) > deadZoneSize.y * 0.5f)
            {
                desiredPosition.y = target.position.y;
            }

            // Look Ahead (horizontal only)
            var targetVelocityX = target.GetComponent<Rigidbody2D>()?.linearVelocity.x ?? 0f;

            if (Mathf.Abs(targetVelocityX) > 0.1f)
            {
                targetLookAhead = Mathf.Sign(targetVelocityX) * lookAheadDistance;
            }
            else
            {
                targetLookAhead = 0f;
            }

            currentLookAhead = Mathf.MoveTowards(
                currentLookAhead,
                targetLookAhead,
                Time.deltaTime * lookAheadSmoothing);
            
            desiredPosition.x += currentLookAhead;

            // Lock camera Z-Axis
            desiredPosition.z = currentPosition.z;

            var smoothedPosition = Vector3.SmoothDamp(
                currentPosition,
                desiredPosition,
                ref velocity,
                smoothTime);

            // Respect Boundaries
            if (useBounds)
            {
                smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
                smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);
            }

            transform.position = smoothedPosition;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}