#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Pihkura.Camera.Core
{
    /// <summary>
    /// Utility class for resolving assets.
    /// from both the Unity project Assets folder and UPM package paths.
    /// </summary>
    public static class AssetResolver
    {
        private const string TargetResourcesPath = "Pihkura/unity-camera-controller/Assets/";

        /// <summary>
        /// Attempts to load a asset by name. Searches Resources first, 
        /// then falls back to AssetDatabase lookup in the Packages folder (Editor only).
        /// </summary>
        public static T LoadAsset<T>(string name, string path = TargetResourcesPath) where T : UnityEngine.Object
        {
            // 1. Try Resources first (runtime-safe)
            var asset = Resources.Load<T>(path + name);
            if (asset != null)
                return asset;

#if UNITY_EDITOR
                // 2. Try to find by AssetDatabase search (Editor only)
                string[] guids = AssetDatabase.FindAssets($"{name} t:{typeof(T).Name}");
            if (guids.Length > 0)
            {
                path = AssetDatabase.GUIDToAssetPath(guids[0]);
                asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset != null)
                    return asset;
            }
#endif
            Debug.LogWarning($"[AssetResolver] Asset '{name}' (type {typeof(T).Name}) not found in Resources or Packages.");
            return null;
        }
    }
}
