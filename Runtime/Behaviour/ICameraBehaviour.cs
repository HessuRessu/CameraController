using UnityEngine;

namespace Pihkura.Camera
{
    /// <summary>
    /// Interface defining the contract for all camera behaviours.
    /// Any camera behaviour (RTS, Following, FreeLook, etc.) must implement these methods.
    /// </summary>
    public interface ICameraBehaviour
    {
        /// <summary>
        /// Initializes the camera behaviour.
        /// Should be called once before the first update.
        /// </summary>
        /// <param name="target">The target transform that the camera may follow or reference.</param>
        void Initialize(Transform target);

        /// <summary>
        /// Handles movement logic for the camera.
        /// Each behaviour implements its own movement rules (e.g., RTS movement, following target).
        /// </summary>
        void HandleMovement(float dt);

        /// <summary>
        /// Handles camera rotation based on user input and internal smoothing.
        /// </summary>
        void HandleRotation(float dt);

        /// <summary>
        /// Handles camera zoom based on input (e.g., scroll wheel) and configuration.
        /// </summary>
        void HandleZoom(float dt);

        /// <summary>
        /// Called at the beginning of the update cycle.
        /// Useful for behaviour-specific pre-update logic.
        /// </summary>
        void OnUpdateBegin();

        /// <summary>
        /// Called at the end of the update cycle.
        /// Useful for behaviour-specific post-update logic.
        /// </summary>
        void OnUpdateCompleted();

        /// <summary>
        /// Called when the behaviour is released or deactivated.
        /// Can be used to clean up runtime state or reset data.
        /// </summary>
        void Release();
    }
}
