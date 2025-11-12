#pragma warning disable CS0649
/**************************************************
 *  MinimapBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.EnhancedTouch;
    using UnityEngine.UI;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Extensions;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
    using System.Drawing;

    /// <summary>
    /// Defines the behaviours for the Minimap
    /// </summary>
    /// <seealso cref="OldSchoolGames.BridgeTrollSimulator.Scripts.UI.UIWindowBehaviour" />
    public class MinimapBehaviour : UIWindowBehaviour
    {
        private float lastMultitouchDistance = 0.0f;

        /// <summary>
        /// The minimap z position
        /// </summary>
        private const float MinimapZPosition = -10.0f;

        /// <summary>
        /// The field of view maximum
        /// </summary>
        private const float FieldOfViewMax = 165.0f;

        /// <summary>
        /// The field of view minimum
        /// </summary>
        private const float FieldOfViewMin = 75.0f;

        /// <summary>
        /// The field of view default
        /// </summary>
        private const float FieldOfViewDefault = 150.0f;

        /// <summary>
        /// The left mouse button down
        /// </summary>
        private bool leftMouseButtonDown = false;

        /// <summary>
        /// The mouse over
        /// </summary>
        [SerializeField, ReadOnly]
        private bool mouseOver = false;

        /// <summary>
        /// The lock input
        /// </summary>
        [SerializeField, ReadOnly]
        private bool lockInput = false;

        /// <summary>
        /// The camera movement speed
        /// </summary>
        [SerializeField]
        private float cameraMovementSpeed;

        /// <summary>
        /// The minimap camera
        /// </summary>
        [SerializeField]
        private Camera minimapCamera;

        [SerializeField]
        private MonoBehaviour minimapBackground;

        /// <summary>
        /// The zoom speed
        /// </summary>
        [SerializeField]
        private float zoomSpeed = 2.5f;

        /// <summary>
        /// The map scroll multiplier
        /// </summary>
        [SerializeField, ReadOnly]
        private float mapScrollMultiplier = -0.5f;

        [SerializeField]
        private RawImage rawImage;

        /// <summary>
        /// The close button
        /// </summary>
        [SerializeField]
        private Button closeButton;

        public RectTransform MinimapBackground
        {
            get
            {
                this.ValidateUnityEditorParameter(this.minimapBackground, nameof(this.minimapBackground));

                return this.minimapBackground.GetComponent<RectTransform>();
            }
        }

        public RawImage MinimapRawImage
        {
            get
            {
                this.ValidateUnityEditorParameter(this.rawImage, nameof(this.rawImage));

                return this.rawImage;
            }
        }

        /// <summary>
        /// Gets the close button.
        /// </summary>
        /// <value>
        /// The close button.
        /// </value>
        public Button CloseButton
        {
            get => this.closeButton;
        }

        /// <summary>
        /// Gets the map collider.
        /// </summary>
        /// <value>
        /// The map collider.
        /// </value>
        public BoxCollider2D MapCollider
        {
            get => this.GetComponentInChildren<BoxCollider2D>();
        }

        /// <summary>
        /// Gets the minimap camera.
        /// </summary>
        /// <value>
        /// The minimap camera.
        /// </value>
        public Camera MinimapCamera
        {
            get => this.minimapCamera;
        }

        /// <summary>
        /// Gets the camera movement speed.
        /// </summary>
        /// <value>
        /// The camera movement speed.
        /// </value>
        public float CameraMovementSpeed
        {
            get => this.cameraMovementSpeed;
        }

        public void Start()
        {
            //this.MinimapRawImage.mainTexture.width = Screen.width;
            //this.MinimapRawImage.mainTexture.height = Screen.height;
            EnhancedTouchSupport.Enable();

            if (this.MapCollider != null)
            {
                this.MapCollider.size = new Vector2(this.MinimapBackground.rect.width, this.MinimapBackground.rect.height);
            }
        }

        /// <summary>
        /// Updates this instance when the Unity Engine updates each frame.
        /// </summary>
        public override void Update()
        {
            if (this.MapCollider != null && this.MapCollider.OverlapPoint(InputExtension.MousePosition))
            {
                this.mouseOver = true;
            }
            else
            {
                this.mouseOver = false;
            }

            if (this.mouseOver && Input.GetMouseButtonDown(InputConfiguration.LeftMouseButton))
            {
                this.leftMouseButtonDown = true;
                InputExtension.HideMouse();
                InputExtension.ClampMouse();
                InputExtension.LockMouse();
            }
            else if (!this.mouseOver || Input.GetMouseButtonUp(InputConfiguration.LeftMouseButton))
            {
                this.leftMouseButtonDown = false;
                InputExtension.UnlockMouse();
                InputExtension.UnClampMouse();
                InputExtension.ShowMouse();
            }

            base.Update();
        }

        /// <summary>
        /// Executes at a fixed interval which is determined by the Unity Engine at runtime.
        /// </summary>
        public void FixedUpdate()
        {
            this.MoveCamera();
            this.ZoomCamera();

            if (InputExtension.IsOpenMinimapPressed() && !this.lockInput)
            {
                this.lockInput = true;

                StartCoroutine(nameof(base.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        InputExtension.IsOpenMinimapPressed,
                        () =>
                        {
                            this.CloseButton.onClick.Invoke();
                            this.lockInput = false;
                        }));
            }
        }

        /// <summary>
        /// Called when this instance is enabled.
        /// </summary>
        public override void OnEnable()
        {
            if (PlayerBehaviour.Instance == null)
            {
                return;
            }

            var minimapPosition = new Vector3(PlayerBehaviour.Instance.Position.x, PlayerBehaviour.Instance.Position.y, MinimapZPosition);

            this.MinimapCamera.transform.position = minimapPosition;
            this.MinimapCamera.fieldOfView = FieldOfViewDefault;

            this.CloseButton.onClick.AddListener(() => this.Disable());

            base.OnEnable();
        }

        /// <summary>
        /// Called when this instance is disabled.
        /// </summary>
        public override void OnDisable()
        {
            this.CloseButton.onClick.RemoveAllListeners();
            base.OnDisable();
        }

        /// <summary>
        /// Moves the camera.
        /// </summary>
        private void MoveCamera()
        {
            var movementVector = Vector2.zero;

            if (this.leftMouseButtonDown)
            {
                movementVector = new Vector2(
                    Input.GetAxis(InputAxes.MouseX) * this.mapScrollMultiplier,
                    Input.GetAxis(InputAxes.MouseY) * this.mapScrollMultiplier);
            }
            else if (Touch.activeFingers.Count > 0)
            {
                if (Touch.activeFingers.Count == 1 && Touch.activeTouches.First().phase == UnityEngine.InputSystem.TouchPhase.Moved)
                {
                    movementVector = new Vector2(
                        -Touch.activeTouches.First().delta.normalized.x,
                        -Touch.activeTouches.First().delta.normalized.y);
                }
                else if (Touch.activeFingers.Count == 2)
                {
                    ZoomCamera(Touch.activeTouches[0], Touch.activeTouches[1]);
                }
            }
            else if (InputExtension.IsCenterMapPressed())
            {
                this.MinimapCamera.transform.position = new Vector3(
                    PlayerBehaviour.Instance.transform.position.x,
                    PlayerBehaviour.Instance.transform.position.y,
                    MinimapZPosition);
                return;
            }
            else
            {
                movementVector = new Vector2(
                    Input.GetAxisRaw(InputAxes.Horizontal) * this.CameraMovementSpeed,
                    Input.GetAxisRaw(InputAxes.Vertical) * this.CameraMovementSpeed);
            }

            movementVector.Normalize();
            this.MoveCamera(movementVector);
        }

        /// <summary>
        /// Moves the camera.
        /// </summary>
        /// <param name="movementVector">The movement vector.</param>
        private void MoveCamera(Vector2 movementVector)
        {
            if (movementVector == Vector2.zero)
            {
                return;
            }

            // TODO: fix placeholder code
            var cameraBoundary = new Scripts.Components.Rectangle();

            var xPosition = MathfExtension.MaxOrMin(
                this.MinimapCamera.transform.position.x + movementVector.x,
                cameraBoundary.RightBound,
                cameraBoundary.LeftBound);

            var yPosition = MathfExtension.MaxOrMin(
                this.MinimapCamera.transform.position.y + movementVector.y,
                cameraBoundary.UpperBound,
                cameraBoundary.LowerBound);

            var newPosition = new Vector3(xPosition, yPosition, this.MinimapCamera.transform.position.z);

            this.MinimapCamera.transform.position = newPosition;
        }

        /// <summary>
        /// Zooms the camera.
        /// </summary>
        private void ZoomCamera()
        {
            var zoomDelta = InputExtension.GetZoomDelta() * this.zoomSpeed;
            this.ZoomCamera(zoomDelta);
        }

        private void ZoomCamera(Touch firstTouch, Touch secondTouch)
        {
            if (firstTouch.phase == UnityEngine.InputSystem.TouchPhase.Began ||
                secondTouch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                this.lastMultitouchDistance = Vector2.Distance(firstTouch.screenPosition,
                    secondTouch.screenPosition);
            }

            if (firstTouch.phase != UnityEngine.InputSystem.TouchPhase.Moved ||
                secondTouch.phase != UnityEngine.InputSystem.TouchPhase.Moved)
            {
                return;
            }

            var newMultiTouchDistance = Vector2.Distance(firstTouch.screenPosition,
                secondTouch.screenPosition);

            ZoomCamera((newMultiTouchDistance - this.lastMultitouchDistance) * this.zoomSpeed);

            this.lastMultitouchDistance = newMultiTouchDistance;
        }

        private void ZoomCamera(float zoomDelta)
        {
            this.MinimapCamera.fieldOfView = MathfExtension.MaxOrMin(this.MinimapCamera.fieldOfView - zoomDelta, FieldOfViewMax, FieldOfViewMin);
            this.mapScrollMultiplier = (this.MinimapCamera.fieldOfView / FieldOfViewMax) * -1.0f;
        }

        /// <summary>
        /// Enables this instance.
        /// </summary>
        public override void Enable()
        {
            if (PlayerBehaviour.Instance.IsWalking)
            {
                return;
            }

            base.Enable();
        }

        public void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(MinimapBehaviour));
        }
    }
}
