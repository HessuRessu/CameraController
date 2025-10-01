using UnityEngine;
using Pihkura.Camera.Utils;
using Pihkura.Camera.Behaviour;
using System.Collections.Generic;

namespace Pihkura.Camera
{
    /// <summary>
    /// Central camera controller that manages multiple camera behaviours,
    /// handles input, zoom, rotation, movement, and updates rays for collision and LOS.
    /// </summary>
    public class CameraController : MonoBehaviour
    {

        public const int BEHAVIOUR_RTS = 0;
        public const int BEHAVIOUR_FOLLOW = 1;
        public const int BEHAVIOUR_FREE = 2;
        public const int BEHAVIOUR_CINEMATIC = 3;

        /// <summary>
        /// The target Transform the camera follows or looks at.
        /// </summary>
        public Transform target;

        /// <summary>
        /// Camera configuration parameters.
        /// </summary>
        public CameraConfiguration configuration;

        /// <summary>
        /// Camera runtime state data.
        /// </summary>
        public CameraData data;

        /// <summary>
        /// Current active behaviour index.
        /// </summary>
        public int behaviourIndex = 0;

        /// <summary>
        /// Array of available camera behaviours (e.g., RTS, Targeted).
        /// </summary>
        private ICameraBehaviour[] availabeBehaviours;

        /// <summary>
        /// Initializes the camera controller and sets up behaviours and rays.
        /// </summary>
        void Start()
        {
            this.data = new CameraData(0, 30);
            this.availabeBehaviours = new ICameraBehaviour[4] {
                new RTSCameraBehaviour(this.configuration, this.data),
                new FollowingCameraBehaviour(this.configuration, this.data),
                new FreeCameraBehaviour(this.configuration, this.data),
                new CinematicCameraBehaviour(this.configuration, this.data)
            };

            this.data.Update(this.transform);
            this.UpdateRays();
            this.SetBehaviour(this.behaviourIndex, null);
        }

        /// <summary>
        /// Updates the camera each frame after all other updates.
        /// Handles input, zoom, rotation, movement, and behaviour updates.
        /// </summary>
        void LateUpdate()
        {
            this.OnUpdateBegin();
            this.UpdateRays();
            this.HandleInput();

            this.availabeBehaviours[this.behaviourIndex].HandleZoom();
            this.availabeBehaviours[this.behaviourIndex].HandleRotation();
            this.availabeBehaviours[this.behaviourIndex].HandleMovement();

            this.OnUpdateCompleted();
        }

        /// <summary>
        /// Switches to camera behaviour in the list defined by index.
        /// Releases the current behaviour if necessary and initializes the new one.
        /// <param name="index">Index of desired behaviour.</param>
        /// </summary>
        private void SetBehaviour(int index, Transform target)
        {
            if (index >= this.availabeBehaviours.Length || index < 0)
                return;
            if (target != null)
                this.target = target;
            this.behaviourIndex = index;
            this.data.current.Update(this.transform);
            this.availabeBehaviours[this.behaviourIndex].Initialize(this.target);
        }

        /// <summary>
        /// Rotates to the next camera behaviour in the list.
        /// Releases the current behaviour if necessary and initializes the new one.
        /// </summary>
        private void RotateBehaviour()
        {
            if (this.behaviourIndex != -1)
                this.availabeBehaviours[this.behaviourIndex].Release();

            this.behaviourIndex++;
            if (this.behaviourIndex >= this.availabeBehaviours.Length)
                this.behaviourIndex = 0;

            this.data.current.Update(this.transform);
            this.availabeBehaviours[this.behaviourIndex].Initialize(this.target);
        }

        /// <summary>
        /// Called at the end of LateUpdate.
        /// Handles behaviour switching via Tab key and triggers behaviour completion callbacks.
        /// </summary>
        private void OnUpdateCompleted()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                this.RotateBehaviour();

            this.availabeBehaviours[this.behaviourIndex].OnUpdateCompleted();
            this.transform.position = this.data.next.position;
            this.transform.rotation = this.data.next.rotation;
        }

        /// <summary>
        /// Called at the beginning of LateUpdate to allow behaviour-specific pre-update logic.
        /// </summary>
        private void OnUpdateBegin()
        {
            this.data.Update(this.transform);
            this.availabeBehaviours[this.behaviourIndex].OnUpdateBegin();
        }

        /// <summary>
        /// Updates forward and down rays for LOS and collision detection.
        /// Clamps the down ray height between reasonable bounds.
        /// </summary>
        private void UpdateRays()
        {
            this.configuration.forwardRay.Cast(this.transform.position, this.transform.forward);
            this.configuration.downRay.Cast(this.transform.position, -Vector3.up);
            this.configuration.downRay.ClampHeight(0f, 1000f);
            this.configuration.groundRay.Cast(this.data.origin, -Vector3.up);
        }

        /// <summary>
        /// Reads player input for movement, zoom, and rotation.
        /// Mouse and keyboard inputs are handled and stored in CameraData.
        /// </summary>
        private void HandleInput()
        {
            // Movement input
            CameraUtils.GetAxis("Horizontal", ref this.data.movementInputX);
            CameraUtils.GetAxis("Vertical", ref this.data.movementInputY);

            // Zoom input
            CameraUtils.GetAxis("Mouse ScrollWheel", ref this.data.zoomInput, 8f);

            // Rotation input (mouse)
            if (this.configuration.rotationButton != -1 && Input.GetMouseButton(this.configuration.rotationButton))
            {
                CameraUtils.GetAxis("Mouse Y", ref this.data.rotationInputX);
                CameraUtils.GetAxis("Mouse X", ref this.data.rotationInputY);
            }
            else
            {
                this.data.rotationInputX = 0f;
                this.data.rotationInputY = 0f;
            }

            // Rotation input (keyboard)
            if (Input.GetKey(KeyCode.Q))
                this.data.rotationInputY = this.configuration.keyboardRotationSpeed;
            if (Input.GetKey(KeyCode.E))
                this.data.rotationInputY = -this.configuration.keyboardRotationSpeed;
        }
    }
}