using UnityEngine;

namespace Pihkura.Camera.Utils
{
    /// <summary>
    /// Represents a 1D boundary with a minimum and maximum value.
    /// Can be used to track limits or extend bounds dynamically.
    /// </summary>
    [System.Serializable]
    public class Boundary
    {
        /// <summary>
        /// Minimum value of the boundary.
        /// </summary>
        public float min;

        /// <summary>
        /// Maximum value of the boundary.
        /// </summary>
        public float max;

        /// <summary>
        /// Expands the boundary to include the specified value.
        /// Updates min if value is smaller and max if value is larger.
        /// </summary>
        /// <param name="value">The value to include in the boundary.</param>
        public void Add(float value)
        {
            if (value < min)
                min = value;
            if (value > max)
                max = value;
        }
    }
}
