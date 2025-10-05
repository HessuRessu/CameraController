using System.Collections;
using System.Collections.Generic;
using Pihkura.Camera.Utils;
using UnityEngine;

namespace Pihkura.Camera.Control
{
    public abstract class BaseInputControlManager : IInputControlManager
    {

        protected CameraConfiguration configuration;

        public BaseInputControlManager(CameraConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public virtual void Move(CameraData data) { }

        public virtual void Zoom(CameraData data) { }

        public virtual void Rotate(CameraData data) { }

        public virtual void Control(CameraController controller) { }

        public virtual void OnEnable() { }
        public virtual void OnDisable() { }

        public static IInputControlManager Initialize(CameraConfiguration configuration)
        {
#if (ENABLE_INPUT_SYSTEM && ENABLE_LEGACY_INPUT_MANAGER)
            if (configuration.inputController == CameraConfiguration.InputController.Legacy)
                return new LegacyInputControlManager(configuration);
            else
                return new NewInputControlManager(configuration);
#elif (ENABLE_INPUT_SYSTEM)
            return new NewInputControlManager(configuration);
#elif (ENABLE_LEGACY_INPUT_MANAGER)
            return new LegacyInputControlManager(configuration);
#else
            return new LegacyInputControlManager(configuration);
#endif
        }

    }

}