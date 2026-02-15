using System;
using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(EntityController))]
    public class JumpController : MonoBehaviour
    {
        [Header("Jump Settings")]
        [SerializeField]
        private float jumpForce = 6f;
        [SerializeField]
        private float coyoteTime = 0.1f;

        [Header("Ground Check")]
        [SerializeField]
        private Transform groundCheck;
        [SerializeField]
        private float groundCheckRadius = 0.1f;
        [SerializeField]
        private LayerMask groundLayer;

        [SerializeField, ReadOnly]
        private bool isGrounded;

        private float coyoteTimer;
        private Rigidbody2D rb;
        private EntityController entity;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            entity = GetComponent<EntityController>();
        }

        private void Update()
        {
            CheckGround();

            if (isGrounded)
            {
                coyoteTimer = coyoteTime;
            }
            else
            {
                coyoteTimer -= Time.deltaTime;
            }

            if (entity.InputSource?.JumpPressed() == true && coyoteTimer > 0f)
            {
                PerformJump();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (groundCheck is null)
            {
                return;
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(
                groundCheck.position,
                groundCheckRadius);
        }

        private void PerformJump()
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce);
            
            coyoteTimer = 0f;
        }

        private void CheckGround()
        {
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundLayer);
        }
    }
}