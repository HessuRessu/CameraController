using UnityEngine;

namespace Pihkura.Camera.Utils
{
    /// <summary>
    /// Represents a 3D bounding area defined by minimum and maximum coordinates.
    /// Can be used to constrain camera movement within a defined space.
    /// </summary>
    [System.Serializable]
    public class Area
    {
        /// <summary>
        /// Maximum bounds of the area (X, Y, Z).
        /// </summary>
        public Vector3 maxBounds;

        /// <summary>
        /// Minimum bounds of the area (X, Y, Z).
        /// </summary>
        public Vector3 minBounds;
    }
}
