#if (ENABLE_INPUT_SYSTEM)
using Pihkura.Camera.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pihkura.Camera.Control
{
    public class NewInputControlManager : BaseInputControlManager
    {

        private InputAction moveAction;
        private InputAction zoomAction;
        private InputAction mouseRotateAction;
        private InputAction keyboardRotateAction;
        private InputAction targetActions;

        public NewInputControlManager(CameraConfiguration configuration)
            : base(configuration)
        {
            this.moveAction = configuration.inputMap.FindAction("Move");
            this.zoomAction = configuration.inputMap.FindAction("Zoom");
            this.mouseRotateAction = configuration.inputMap.FindAction("MouseRotate");
            this.keyboardRotateAction = configuration.inputMap.FindAction("KeyboardRotate");
            this.targetActions = configuration.inputMap.FindAction("Target");
        }

        public override void Move(CameraData data)
        {
            // Movement input
            Vector2 value = this.moveAction.ReadValue<Vector2>();
            CameraUtils.SmoothInput(value.x, ref data.movementInputX);
            CameraUtils.SmoothInput(value.y, ref data.movementInputY);
        }

        public override void Zoom(CameraData data)
        {
            // We need to multiply this by 0.001 because new input system returns way too high values for zooming.
            Vector2 value = this.zoomAction.ReadValue<Vector2>() * 0.001f;
            CameraUtils.SmoothInput(value.y, ref data.zoomInput, 8f);

        }

        public override void Rotate(CameraData data)
        {
            data.rotationInputX = 0f;
            data.rotationInputY = 0f;

            // Mouse rotation.
            Vector2 mouseValue = this.mouseRotateAction.ReadValue<Vector2>();
            if (Mathf.Abs(mouseValue.y) > 0.0001f)
            {
                if (Mathf.Abs(mouseValue.y) > 2f)
                    mouseValue.y *= 0.1f;
                CameraUtils.SmoothInput(mouseValue.y, ref data.rotationInputX);
            }
            if (Mathf.Abs(mouseValue.x) > 0.0001f)
            {
                if (Mathf.Abs(mouseValue.x) > 2f)
                    mouseValue.x *= 0.1f;
                CameraUtils.SmoothInput(mouseValue.x, ref data.rotationInputY);
            }

            // Keyboard rotation.
            Vector2 keyboardValue = this.keyboardRotateAction.ReadValue<Vector2>();
            if (Mathf.Abs(keyboardValue.x) > 0.0001f)
            {
                if (keyboardValue.x < 0f)
                    data.rotationInputY = configuration.keyboardRotationSpeed;
                else
                    data.rotationInputY = -configuration.keyboardRotationSpeed;
            }

        }

        public override void Control(CameraController controller)
        {
            if (this.targetActions.triggered && this.targetActions.ReadValue<float>() > 0f)
                controller.RotateBehaviour();
        }

        public override void OnEnable() => this.configuration.inputMap.Enable();

        public override void OnDisable() => this.configuration.inputMap.Disable();

    }

}
#endif