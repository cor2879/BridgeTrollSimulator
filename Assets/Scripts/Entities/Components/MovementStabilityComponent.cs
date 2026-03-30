using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.Components
{
    [RequireComponent(typeof(EntityController))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementStabilityComponent : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField, ReadOnly] private EntityController entity;
        [SerializeField, ReadOnly] private Rigidbody2D rb;

        [Header("Stuck Detection")]
        [SerializeField] private float stuckThresholdTime = 0.5f;
        [SerializeField] private float minMovementThreshold = 0.001f;

        [Header("Recovery")]
        [SerializeField] private float nudgeAmount = 0.05f;
        [SerializeField] private bool reapplyVelocity = true;

        private float lastX;
        private float stuckTimer;
        private int stuckCount;

        private void Awake()
        {
            if (rb == null)
                rb = GetComponent<Rigidbody2D>();

            if (entity == null)
                entity = GetComponent<EntityController>();
        }

        private void FixedUpdate()
        {
            if (!ShouldMonitor())
                return;

            float currentX = rb.position.x;
            float delta = Mathf.Abs(currentX - lastX);

            if (delta < minMovementThreshold)
            {
                stuckTimer += Time.fixedDeltaTime;
            }
            else
            {
                stuckTimer = 0f;
                stuckCount = 0;
            }

            if (stuckTimer >= stuckThresholdTime)
            {
                RecoverFromStuck();
                stuckTimer = 0f;
            }

            lastX = currentX;
        }

        private bool ShouldMonitor()
        {
            if (entity == null)
                return false;

            return entity.CurrentControlMode == ControlMode.Passing ||
                   entity.CurrentControlMode == ControlMode.Leaving;
        }

        private void RecoverFromStuck()
        {
            stuckCount++;

            float direction = GetDirection();

            float scaledNudge = nudgeAmount * stuckCount;

            rb.position += new Vector2(direction * scaledNudge, 0f);

            if (reapplyVelocity)
            {
                float speed = entity.MoveSpeed;
                rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
            }

            // Nuclear fallback (very rare)
            if (stuckCount > 3)
            {
                rb.simulated = false;
                rb.simulated = true;
                stuckCount = 0;
            }

#if UNITY_EDITOR
            Debug.Log($"{name} recovered from stuck state (count={stuckCount})");
#endif
        }

        private float GetDirection()
        {
            // Assuming left = -1, right = +1
            return entity.IsFacingRight ? 1f : -1f;
        }
    }
}