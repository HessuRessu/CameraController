using UnityEngine;
using Pihkura.Camera.Utils;

namespace Pihkura.Camera.Behaviour
{
    /// <summary>
    /// A cinematic camera behaviour that smoothly orbits around the target
    /// with automatic position and rotation interpolation. Intended for cutscenes
    /// or special moments where manual player input is disabled.
    /// </summary>
    public class CinematicCameraBehaviour : BaseCameraBehaviour
    {
        private Vector3 desiredPosition;
        private Quaternion desiredRotation;
        private float orbitAngle = 0f;

        public CinematicCameraBehaviour(CameraConfiguration configuration, CameraData data)
            : base(configuration, data)
        {
        }

        public override void Initialize(Transform target)
        {
            base.Initialize(target);

            // Start aligned with current camera position
            desiredPosition = target.position + new Vector3(0f, data.distance, -data.distance);
            desiredRotation = Quaternion.LookRotation(target.position - desiredPosition);
        }

        public override void HandleMovement()
        {
            // Slowly orbit around target
            orbitAngle += Time.fixedUnscaledDeltaTime * 10f; // degrees per second
            float radians = orbitAngle * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(
                Mathf.Cos(radians) * data.distance,
                data.distance * 0.5f, // keep some height
                Mathf.Sin(radians) * data.distance
            );

            desiredPosition = data.target.position + offset;
            desiredRotation = Quaternion.LookRotation(data.target.position - desiredPosition);

            // Smooth interpolation
            Vector3 smoothedPos = Vector3.Lerp(data.current.position, desiredPosition, Time.fixedUnscaledDeltaTime);
            Quaternion smoothedRot = Quaternion.Slerp(data.current.rotation, desiredRotation, Time.fixedUnscaledDeltaTime);
            CameraUtils.HandleCameraCollision(configuration, data, ref smoothedPos);

            // Instead of writing to transform directly, update CameraData
            data.next.position = smoothedPos;
            data.next.rotation = smoothedRot;
            data.next.forward = smoothedRot * Vector3.forward;
            data.next.right = smoothedRot * Vector3.right;
        }

        public override void HandleRotation()
        {
            // Rotation handled in HandleMovement
        }

        public override void HandleZoom()
        {

        }
    }
}
