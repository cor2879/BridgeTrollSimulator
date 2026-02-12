using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
using OldSchoolGames.BridgeTrollSimulator.Scripts.InputHandling;
using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class EntityController : MonoBehaviour
    {
        protected Animator animator;
        protected Rigidbody2D rb;
        protected IInputSource inputSource;
        protected ControlMode controlMode = ControlMode.FreeRoam;

        [Header("Movement")]
        [SerializeField] 
        private float moveSpeed = 3f;

        private Vector2 movementInput;
        private bool facingRight = true;

        #region Properties

        public ControlMode CurrentControlMode => controlMode;
        public IInputSource InputSource => inputSource;

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

        protected bool HasInput => inputSource is not null;

        protected virtual void ProcessInput()
        {
            if (!HasInput)
            {
                return;
            }

            if (controlMode == ControlMode.Disabled ||
                controlMode == ControlMode.CutScene ||
                controlMode == ControlMode.Dead)
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
            if (this.movementInput.x > 0 && !this.facingRight)
            {
                this.Flip();
            }

            if (this.movementInput.x < 0 && this.facingRight)
            {
                this.Flip();
            }
        }

        public void SetInputSource(IInputSource source)
        {
            inputSource = source;
        }

        public void ClearInputSource()
        {
            inputSource = null;
        }

        public void SetControlMode(ControlMode mode)
        {
            controlMode = mode;
        }

        #endregion

        #region Movement

        protected virtual void ApplyMovement()
        {
            if (this.CurrentControlMode != ControlMode.FreeRoam)
            {
                rb.linearVelocity = new Vector2(
                    0f, rb.linearVelocity.y);
                return;
            }

            rb.linearVelocity = new Vector2(
                movementInput.x * moveSpeed, 
                GetComponent<Rigidbody>().linearVelocity.y);
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
                Mathf.Abs(moveSpeed));
            animator.SetFloat(
                Constants.AnimatorParams.xDirection, 
                facingRight ? 1f : -1f);
        }

        protected void TriggerAction(string triggerName)
        {
            animator.SetTrigger(triggerName);
        }

        #endregion
    }
}