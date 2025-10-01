using UnityEngine;
using Pihkura.Camera.Utils;

namespace Pihkura.Camera
{
    /// <summary>
    /// Stores configuration parameters for a camera, including distances, rotation, smoothing,
    /// collision, LOS correction, and movement limits within the scene or map area.
    /// </summary>
    [System.Serializable]
    public class CameraConfiguration
    {

        [Header("Distance & Zoom")]
        /// <summary>
        /// Vertical offset above the target's Y position.
        /// </summary>
        public float heightOffset = 0f;
        /// <summary>
        /// Minimum allowed distance.
        /// </summary>
        public float minDistance = 10f;
        /// <summary>
        /// Maximum allowed distance.
        /// </summary>
        public float maxDistance = 150f;
        /// <summary>
        /// Speed at which camera zooms in/out.
        /// </summary>
        public float zoomSpeed = 2f;

        [Header("Rotation")]
        /// <summary>
        /// Mouse button index used to rotate the camera.
        /// </summary>
        public int rotationButton = 2;
        /// <summary>
        /// Yaw rotation speed (horizontal).
        /// </summary>
        public float yawSpeed = 200f;
        /// <summary>
        /// Pitch rotation speed (vertical).
        /// </summary>
        public float pitchSpeed = 200f;
        /// <summary>
        /// Minimum allowed pitch angle.
        /// </summary>
        public float minPitch = 30f;
        /// <summary>
        /// Maximum allowed pitch angle.
        /// </summary>
        public float maxPitch = 80f;
        /// <summary>
        /// Keyboard rotation speed multiplier.
        /// </summary>
        public float keyboardRotationSpeed = 0.5f;

        [Header("Smoothing")]
        /// <summary>
        /// Time in seconds to smooth position changes.
        /// </summary>
        public float moveSmoothTime = 0.1f;
        /// <summary>
        /// Time in seconds to smooth rotation changes.
        /// </summary>
        public float rotSmoothTime = 0.1f;

        [Header("Collision")]
        /// <summary>
        /// Layer mask used for collision detection.
        /// </summary>
        public LayerMask collisionMask = ~0;
        /// <summary>
        /// Collision radius for camera (if used in calculations).
        /// </summary>
        public float collisionRadius = 2.5f;
        /// <summary>
        /// Vertical offset above terrain or obstacles to avoid clipping.
        /// </summary>
        public float collisionOffset = 2f;

        [Header("Auto LOS Correction")]
        /// <summary>
        /// Speed at which camera raises its pitch when LOS is blocked (degrees/sec).
        /// </summary>
        public float autoPitchSpeed = 45f;
        /// <summary>
        /// Maximum additional pitch when auto-correcting LOS.
        /// </summary>
        public float maxAutoPitch = 45f;
        /// <summary>
        /// Current dynamic pitch added by LOS correction.
        /// </summary>
        [System.NonSerialized] public float autoPitch = 0f;

        [Header("Area / Map settings")]
        /// <summary>
        /// Bounding area for camera movement (map or level limits).
        /// </summary>
        public Area areaBounds;

        [Header("Movement settings")]
        /// <summary>
        /// Speed at which the camera moves across the map.
        /// </summary>
        public float movementSpeed;

        [Header("Ray settings")]
        /// <summary>
        /// Ray used to detect the point forward from the camera.
        /// </summary>
        public CameraRay forwardRay;
        /// <summary>
        /// Ray used to detect ground or terrain below the camera.
        /// </summary>
        public CameraRay downRay;
        /// <summary>
        /// Ray used to detect ground or terrain below the camera origin.
        /// </summary>
        public CameraRay groundRay;

        /// <summary>
        /// Clamps the given position within the configured area bounds.
        /// </summary>
        /// <param name="position">Position to clamp (passed by reference).</param>
        /// <returns>Clamped position within area bounds.</returns>
        public Vector3 GetBoundedPosition(ref Vector3 position)
        {
            if (position.x > this.areaBounds.maxBounds.x)
                position.x = this.areaBounds.maxBounds.x;
            if (position.z > this.areaBounds.maxBounds.z)
                position.z = this.areaBounds.maxBounds.z;
            if (position.x < this.areaBounds.minBounds.x)
                position.x = this.areaBounds.minBounds.x;
            if (position.z < this.areaBounds.minBounds.z)
                position.z = this.areaBounds.minBounds.z;
            if (position.y < this.areaBounds.minBounds.y)
                position.y = this.areaBounds.minBounds.y;

            return position;
        }

        /// <summary>
        /// Computes a normalized distance ratio between the camera and a target position.
        /// </summary>
        /// <param name="position">Current camera position.</param>
        /// <param name="target">Target position to measure distance to.</param>
        /// <returns>A value between 0.1 and 1.0 representing the distance ratio.</returns>
        public float GetDistanceRatio(Vector3 position, Vector3 target) =>
            Mathf.Clamp((Vector3.Distance(position, target) - this.minDistance) / (this.maxDistance - this.minDistance), 0.1f, 1f);
    }
}
