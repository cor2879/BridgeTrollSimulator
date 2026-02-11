using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Input;
using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class EntityController : MonoBehaviour
    {
        protected Animator animator;
        protected Rigidbody2D rb;
        protected IInputSource inputSource;

        [Header("Movement")]
        [SerializeField] 
        private float moveSpeed = 3f;

        private float movementInput;
        private bool facingRight = true;

        #region Properties

        public float MoveSpeed { get; set; }

        public float MovementInput { get; protected set; }

        public bool FacingRight { get; protected set; }

        #endregion

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void Update()
        {
            this.ProcessInput();
            this.UpdateAnimator();
        }

        protected virtual void FixedUpdate()
        {
            this.ApplyMovement();
        }

        #region Input

        protected virtual void ProcessInput()
        {
            if (this.inputSource is null)
            {
                return;
            }

            this.movementInput = new Vector2(
                this.inputSource.GetHorizontal(),
                this.inputSource.GetVertical()
            );
        }

        protected void HandleFacing()
        {
            if (this.movmentInput.x > 0 && !this.facingRight)
            {
                this.Flip();
            }

            if (this.movementInput.x < 0 && this.facingRight)
            {
                this.Flip();
            }
        }

        #endregion

        #region Movement

        protected virtual void ApplyMovement()
        {
            rb.velocity = new Vector2(
                movementInput * moveSpeed, 
                rb.velocity.y);
        }

        protected void Flip()
        {
            facingRight = !facingRight;

            var scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }

        #endregion

        #region Animation

        protected virtual void UpdateAnimator()
        {
            animator.SetFloat(
                Constants.AnimatorParams.Speed, 
                Mathf.Abs(movementInput));
            animator.SetFloat(
                Constants.AnimatorPrams.xDirection, 
                facingRight ? 1f : -1f);
        }

        protected void TriggerAction(string triggerName)
        {
            animator.SetTrigger(triggerName);
        }

        #endregion
    }
}