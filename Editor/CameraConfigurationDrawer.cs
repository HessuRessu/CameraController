using UnityEngine;
using UnityEditor;
using Pihkura.Camera;
using Pihkura.Camera.Utils;

[CustomPropertyDrawer(typeof(CameraConfiguration))]
public class CameraConfigurationDrawer : PropertyDrawer
{
    private string GetFoldoutKey(string propertyName, string foldName) => $"CameraConfig_{propertyName}_{foldName}_Foldout";

    private bool GetFoldout(string key, bool defaultValue = true)
    {
        return EditorPrefs.GetBool(key, defaultValue);
    }

    private void SetFoldout(string key, bool value)
    {
        EditorPrefs.SetBool(key, value);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        EditorGUILayout.Space();

        string propName = property.name;

        // --- Distance & Zoom ---
        string distanceKey = GetFoldoutKey(propName, "DistanceZoom");
        bool showDistanceZoom = GetFoldout(distanceKey);
        showDistanceZoom = EditorGUILayout.Foldout(showDistanceZoom, "Distance & Zoom", true);
        SetFoldout(distanceKey, showDistanceZoom);
        if (showDistanceZoom)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(property.FindPropertyRelative("heightOffset"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("minDistance"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("maxDistance"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("zoomSpeed"));
            EditorGUILayout.EndVertical();
        }

        // --- Rotation ---
        string rotationKey = GetFoldoutKey(propName, "Rotation");
        bool showRotation = GetFoldout(rotationKey);
        showRotation = EditorGUILayout.Foldout(showRotation, "Rotation", true);
        SetFoldout(rotationKey, showRotation);
        if (showRotation)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(property.FindPropertyRelative("rotationButton"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("yawSpeed"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("pitchSpeed"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("minPitch"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("maxPitch"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("keyboardRotationSpeed"));
            EditorGUILayout.EndVertical();
        }

        // --- Smoothing ---
        string smoothingKey = GetFoldoutKey(propName, "Smoothing");
        bool showSmoothing = GetFoldout(smoothingKey);
        showSmoothing = EditorGUILayout.Foldout(showSmoothing, "Smoothing", true);
        SetFoldout(smoothingKey, showSmoothing);
        if (showSmoothing)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(property.FindPropertyRelative("moveSmoothTime"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("rotSmoothTime"));
            EditorGUILayout.EndVertical();
        }

        // --- Collision ---
        string collisionKey = GetFoldoutKey(propName, "Collision");
        bool showCollision = GetFoldout(collisionKey);
        showCollision = EditorGUILayout.Foldout(showCollision, "Collision", true);
        SetFoldout(collisionKey, showCollision);
        if (showCollision)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(property.FindPropertyRelative("collisionMask"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("collisionRadius"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("collisionOffset"));
            EditorGUILayout.EndVertical();
        }

        // --- Auto LOS Correction ---
        string losKey = GetFoldoutKey(propName, "AutoLOS");
        bool showAutoLOS = GetFoldout(losKey);
        showAutoLOS = EditorGUILayout.Foldout(showAutoLOS, "Auto LOS Correction", true);
        SetFoldout(losKey, showAutoLOS);
        if (showAutoLOS)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(property.FindPropertyRelative("autoPitchSpeed"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("maxAutoPitch"));
            EditorGUILayout.EndVertical();
        }

        // --- Movement ---
        string movementKey = GetFoldoutKey(propName, "Movement");
        bool showMovement = GetFoldout(movementKey);
        showMovement = EditorGUILayout.Foldout(showMovement, "Movement", true);
        SetFoldout(movementKey, showMovement);
        if (showMovement)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(property.FindPropertyRelative("movementSpeed"));
            EditorGUILayout.EndVertical();
        }

        // --- Area / Map Settings ---
        string areaKey = GetFoldoutKey(propName, "AreaBounds");
        bool showArea = GetFoldout(areaKey);
        showArea = EditorGUILayout.Foldout(showArea, "Area / Map Settings", true);
        SetFoldout(areaKey, showArea);
        if (showArea)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(property.FindPropertyRelative("areaBounds"), true);
            EditorGUILayout.EndVertical();
        }

        // --- Ray Settings ---
        string rayKey = GetFoldoutKey(propName, "RaySettings");
        bool showRaySettings = GetFoldout(rayKey);
        showRaySettings = EditorGUILayout.Foldout(showRaySettings, "Ray Settings", true);
        SetFoldout(rayKey, showRaySettings);
        if (showRaySettings)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(property.FindPropertyRelative("forwardRay"), true);
            EditorGUILayout.PropertyField(property.FindPropertyRelative("downRay"), true);
            EditorGUILayout.PropertyField(property.FindPropertyRelative("groundRay"), true);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space();
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
}