using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Path_Player))]
[CanEditMultipleObjects]
public class Path_PlayerEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Path_Player path = (Path_Player)target;

        GUILayout.BeginHorizontal();
        if(GUILayout.Button ("Set point_A"))
        {
            path.markA();
        }

        if (GUILayout.Button("Set point_B"))
        {
            path.markB();
        }
        GUILayout.EndHorizontal();
    }

    
}
