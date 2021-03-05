using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(ShadowEntity))]
[CanEditMultipleObjects]
public class ShadowEntityEditor : Editor
{
    SerializedProperty debugLines;
    SerializedProperty projActive;
    SerializedProperty projPrefab;
    SerializedProperty fireCooldown;
    SerializedProperty targetTransforms;
    SerializedProperty skia;
    SerializedProperty lux;
    SerializedProperty aura;
    SerializedProperty lf;

    private void OnEnable()
    {
        debugLines = serializedObject.FindProperty("debugLines");
        projActive = serializedObject.FindProperty("projActive");
        projPrefab = serializedObject.FindProperty("projPrefab");
        fireCooldown = serializedObject.FindProperty("fireCooldown");
        targetTransforms = serializedObject.FindProperty("targetTransforms");
        skia = serializedObject.FindProperty("skia");
        lux = serializedObject.FindProperty("lux");
        aura = serializedObject.FindProperty("aura");
        lf = serializedObject.FindProperty("lf");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(debugLines, new GUIContent("Show Debug Lines"));

        EditorGUILayout.PropertyField(projActive, new GUIContent("Activate Projectiles"));
        EditorGUILayout.PropertyField(projPrefab, new GUIContent("Projectile"));
        EditorGUILayout.PropertyField(fireCooldown, new GUIContent("Cooldown"));

        EditorGUILayout.PropertyField(skia, new GUIContent("Skia"));
        EditorGUILayout.PropertyField(lux, new GUIContent("Lux"));
        EditorGUILayout.PropertyField(aura, new GUIContent("Aura"));
        EditorGUILayout.PropertyField(lf, new GUIContent("Light Flicking"));
        EditorGUILayout.PropertyField(targetTransforms, new GUIContent("Targets"));

        serializedObject.ApplyModifiedProperties();
    }
}
#endif