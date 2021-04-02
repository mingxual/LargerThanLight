using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(SCObstacle))]
[CanEditMultipleObjects]
public class SCObstacleEditor : Editor
{
    SerializedProperty enableRotation;
    SerializedProperty rotationAxis;
    SerializedProperty rotationSpeed;
    SerializedProperty debugLines;
    SerializedProperty shadowprojaffect;
    SerializedProperty shadowprojtime;
    //SerializedProperty m_TargetAlphaFade;

    private void OnEnable()
    {
        enableRotation = serializedObject.FindProperty("enableRotation");
        rotationAxis = serializedObject.FindProperty("rotationAxis");
        rotationSpeed = serializedObject.FindProperty("rotationSpeed");
        debugLines = serializedObject.FindProperty("debugLines");
        shadowprojaffect = serializedObject.FindProperty("shadowprojaffect");
        shadowprojtime = serializedObject.FindProperty("shadowprojtime");
        //m_TargetAlphaFade = serializedObject.FindProperty("m_TargetAlphaFade");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(enableRotation, new GUIContent("Enable Rotation"));

        if (enableRotation.boolValue)
        {
            EditorGUILayout.PropertyField(rotationAxis, new GUIContent("Axis"));
            EditorGUILayout.PropertyField(rotationSpeed, new GUIContent("Speed"));
        }

        EditorGUILayout.PropertyField(debugLines, new GUIContent("Show Debug Lines"));

        EditorGUILayout.PropertyField(shadowprojaffect, new GUIContent("Affected by Shadow Projectiles"));
        if (shadowprojaffect.boolValue)
        {
            EditorGUILayout.PropertyField(shadowprojtime, new GUIContent("Time affected by Shadow Projectiles"));
        }

        //EditorGUILayout.PropertyField(m_TargetAlphaFade, new GUIContent("Target alpha value when object is fading"));

        serializedObject.ApplyModifiedProperties();
    }
}
#endif