﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SCManager))]
public class SCWallSetup : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SCManager script = (SCManager)target;
        if(GUILayout.Button("Build Walls"))
        {
            script.BuildWalls();
        }
    }
}