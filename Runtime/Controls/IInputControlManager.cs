using Pihkura.Camera.Core;

namespace Pihkura.Camera.Control
{
    /// <summary>
    /// Interface defining the contract for all camera input controls.
    /// Any camera input controller must implement these methods.
    /// </summary>
    public interface IInputControlManager
    {

        void Move(CameraData data);

        void Zoom(CameraData data);

        void Rotate(CameraData data);

        void Control(CameraController controller);

        void OnEnable();

        void OnDisable();
    }
}
