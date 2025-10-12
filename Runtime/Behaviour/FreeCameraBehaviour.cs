using UnityEngine;
using Pihkura.Camera.Utils;
using Unity.VisualScripting;

namespace Pihkura.Camera.Behaviour
{
    /// <summary>
    /// A free-flying camera behaviour.
    /// Allows full 3D movement and rotation without being locked to a target.
    /// Useful for debugging, cinematic shots, or level inspection.
    /// </summary>
    public class FreeCameraBehaviour : BaseCameraBehaviour
    {
        public FreeCameraBehaviour(CameraConfiguration configuration, CameraData data)
            : base(configuration, data) { }

        public override void Initialize(Transform target)
        {
            // Initialize rotation values based on current camera transform
            data.yaw = data.current.eulerAngles.y;
            data.pitch = data.current.eulerAngles.x;
            data.targetYaw = data.yaw;
            data.targetPitch = data.pitch;
        }

        public override void HandleMovement()
        {
            // Base direction vectors
            Vector3 forward = data.current.forward;
            Vector3 right = data.current.right;
            Vector3 up = Vector3.up;

            // Normalize to prevent diagonal speed boost
            forward.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 move = Vector3.zero;

            if (Mathf.Abs(data.movementInputY) > 0.0001f)
                move += forward * data.movementInputY * this.data.speedRatio * configuration.movementSpeed * 20f;
            if (Mathf.Abs(data.movementInputX) > 0.0001f)
                move += right * data.movementInputX * this.data.speedRatio * configuration.movementSpeed * 20f;
            if (Mathf.Abs(data.zoomInput) > 0.0001f)
                move += (data.zoomInput < 0 ? -up : up) * this.data.speedRatio * configuration.zoomSpeed * 20f;

            // move.Normalize();

            Vector3 desiredPosition = data.current.position + move;

            // Optional: clamp to area bounds if defined
            desiredPosition = configuration.GetBoundedPosition(ref desiredPosition);
            if (desiredPosition.y - this.configuration.groundRay.Point.y > this.configuration.maxDistance)
                desiredPosition.y = this.configuration.groundRay.Point.y + this.configuration.maxDistance;

            // Smooth movement
            data.next.position = Vector3.SmoothDamp(data.current.position, desiredPosition, ref data.moveVelocity, configuration.moveSmoothTime, float.PositiveInfinity, Time.fixedUnscaledDeltaTime);
            CameraUtils.HandleCameraCollision(configuration, data, ref data.next.position);
        }

        public override void HandleRotation()
        {
            base.HandleRotation();
            data.next.rotation = Quaternion.Euler(data.pitch, data.yaw, 0f);
        }

        public override void HandleZoom() { }

        public override void OnUpdateBegin()
        {
            this.data.origin = this.configuration.downRay.Point;
            this.data.speedRatio = this.configuration.GetDistanceRatio(this.data.current.position, this.data.origin);
        }

        public override void OnUpdateCompleted() { }

        public override void Release() { }
    }
}
