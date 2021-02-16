using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(ShadowEntity))]
public class ShadowEntityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ShadowEntity script = (ShadowEntity)target;

        script.enableRotation = GUILayout.Toggle(script.enableRotation, "Enable Rotation");

        if (script.enableRotation)
        {
            script.rotationAxis = EditorGUILayout.Vector3Field("Axis", script.rotationAxis);
            script.rotationSpeed = EditorGUILayout.FloatField("Speed", script.rotationSpeed);
        }

        script.debugLines = GUILayout.Toggle(script.debugLines, "Show Debug Lines");
    }
}
#endif