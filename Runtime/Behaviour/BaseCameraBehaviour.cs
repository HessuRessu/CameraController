using UnityEngine;

namespace Pihkura.Camera.Behaviour
{
    /// <summary>
    /// Base class for all camera behaviours.
    /// Provides common functionality such as rotation, zoom and smoothing.
    /// Concrete behaviours (e.g., RTS or Following) should override HandleMovement
    /// to implement specific movement logic.
    /// </summary>
    public abstract class BaseCameraBehaviour : ICameraBehaviour
    {
        /// <summary>
        /// Configuration values defining limits, speeds, and input state.
        /// </summary>
        protected CameraConfiguration configuration;

        /// <summary>
        /// Shared runtime state data (yaw, pitch, velocities, etc.).
        /// </summary>
        protected CameraData data;

        /// <summary>
        /// Creates a new base camera behaviour.
        /// </summary>
        /// <param name="transform">The camera transform to control.</param>
        /// <param name="configuration">The configuration containing input and settings.</param>
        /// <param name="data">The runtime state data for this behaviour.</param>
        public BaseCameraBehaviour(CameraConfiguration configuration, CameraData data)
        {
            this.configuration = configuration;
            this.data = data;
        }

        /// <summary>
        /// Initializes yaw and pitch values based on the current camera orientation.
        /// Should be called once before the first update.
        /// </summary>
        /// <param name="target">The target transform (if applicable).</param>
        public virtual void Initialize(Transform target)
        {
            this.data.target = target;
        }

        /// <summary>
        /// Handles camera rotation logic using input and smoothing.
        /// Can be overridden if behaviour needs custom rotation handling.
        /// </summary>
        public virtual void HandleRotation()
        {
           if (data.rotationInputX != 0f || data.rotationInputY != 0f)
            {
                if (Mathf.Abs(data.rotationInputY) > 0.0001f)
                {
                    data.targetYaw += data.rotationInputY * configuration.yawSpeed * Time.unscaledDeltaTime;
                }
                if (Mathf.Abs(data.rotationInputX) > 0.0001f)
                {
                    data.targetPitch -= data.rotationInputX * configuration.pitchSpeed * Time.unscaledDeltaTime;
                    data.targetPitch = Mathf.Clamp(data.targetPitch, configuration.minPitch, configuration.maxPitch);
                }
            }

            // Smoothly interpolate yaw and pitch to prevent sudden jumps
            data.yaw = Mathf.SmoothDampAngle(
                data.yaw,
                data.targetYaw,
                ref data.yawVelocity,
                configuration.rotSmoothTime,
                float.PositiveInfinity,
                Time.unscaledDeltaTime
            );

            data.pitch = Mathf.SmoothDampAngle(
                data.pitch,
                data.targetPitch,
                ref data.pitchVelocity,
                configuration.rotSmoothTime,
                float.PositiveInfinity,
                Time.unscaledDeltaTime
            );
        }

        /// <summary>
        /// Handles camera zoom using scroll wheel or other configured input.
        /// Adjusts camera distance within min/max bounds.
        /// </summary>
        public virtual void HandleZoom()
        {
             if (Mathf.Abs(data.zoomInput) > 0.0001f)
            {
                data.distance -= data.zoomInput * configuration.zoomSpeed * data.speedRatio;
                data.distance = Mathf.Clamp(data.distance, configuration.minDistance, configuration.maxDistance);
            }
        }

        /// <summary>
        /// Must be implemented in concrete behaviours.
        /// Defines how the camera moves in the world (e.g., follow target, RTS free move).
        /// </summary>
        public abstract void HandleMovement();

        /// <summary>
        /// Called at the beginning of the update cycle.
        /// Override to insert behaviour-specific pre-update logic.
        /// </summary>
        public virtual void OnUpdateBegin()
        {
            this.data.speedRatio = this.configuration.GetDistanceRatio(this.data.current.position, this.data.origin);
        }

        /// <summary>
        /// Called at the end of the update cycle.
        /// Override to insert behaviour-specific post-update logic.
        /// </summary>
        public virtual void OnUpdateCompleted() { }

        /// <summary>
        /// Called when the behaviour is released or deactivated.
        /// Use to clean up or reset data.
        /// </summary>
        public virtual void Release() { }
    }
}
