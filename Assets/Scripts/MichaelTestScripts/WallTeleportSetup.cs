using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(WallTeleport))]
public class WallTeleportSetup : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WallTeleport script = (WallTeleport)target;
        if (GUILayout.Button("Change Walls"))
        {
            script.ChangeWalls();
        }
    }
}
#endif