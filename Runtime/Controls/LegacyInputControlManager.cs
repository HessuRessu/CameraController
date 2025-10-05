using Pihkura.Camera.Utils;
using UnityEngine;

namespace Pihkura.Camera.Control
{
    public class LegacyInputControlManager : BaseInputControlManager
    {

        public LegacyInputControlManager(CameraConfiguration configuration) 
            : base(configuration) { }

        public override void Move(CameraData data)
        {
            CameraUtils.SmoothInput(Input.GetAxisRaw("Horizontal"), ref data.movementInputX);
            CameraUtils.SmoothInput(Input.GetAxisRaw("Vertical"), ref data.movementInputY);
        }

        public override void Zoom(CameraData data)
        {
            CameraUtils.SmoothInput(Input.GetAxisRaw("Mouse ScrollWheel"), ref data.zoomInput, 8f);
        }

        public override void Rotate(CameraData data)
        {
            // Rotation input (mouse)
            if (configuration.rotationButton != -1 && Input.GetMouseButton(configuration.rotationButton))
            {
                CameraUtils.SmoothInput(Input.GetAxisRaw("Mouse Y"), ref data.rotationInputX);
                CameraUtils.SmoothInput(Input.GetAxisRaw("Mouse X"), ref data.rotationInputY);
            }
            else
            {
                data.rotationInputX = 0f;
                data.rotationInputY = 0f;
            }

            // Rotation input (keyboard)
            if (Input.GetKey(KeyCode.Q))
                data.rotationInputY = configuration.keyboardRotationSpeed;
            if (Input.GetKey(KeyCode.E))
                data.rotationInputY = -configuration.keyboardRotationSpeed;
        }

        public override void Control(CameraController controller)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                controller.RotateBehaviour();
        }
    }

}