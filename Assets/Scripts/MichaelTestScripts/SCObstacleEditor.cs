using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(SCObstacle))]
public class SCObstacleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SCObstacle script = (SCObstacle)target;

        script.enableRotation = GUILayout.Toggle(script.enableRotation, "Enable Rotation");

        if (script.enableRotation)
        {
            script.rotationAxis = EditorGUILayout.Vector3Field("Axis", script.rotationAxis);
            script.rotationSpeed = EditorGUILayout.FloatField("Speed", script.rotationSpeed);
        }
    }
}
#endif