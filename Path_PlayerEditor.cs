using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PathClass;
//THIS SCRIPT IS FOR THE PATH EDITOR UI
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

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Prev"))
        {
            path.prevTarget();
        }
        if (GUILayout.Button("Next"))
        {
            path.nextTarget();
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button ("Move to Point A"))
        {
            path.moveMarkerA();
        }
        if (GUILayout.Button ("Move to Point B"))
        {
            path.moveMarkerB();
        }

        EditorGUILayout.Space();

        EditorGUILayout.IntSlider("Turn Target", path.turnTarget,0,path.pathLine[path.target].turns.Length - 1);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button ("Prev Turn"))
        {
            path.prevTurnTarget();
        }
        if (GUILayout.Button ("Mark Turn LookAt Point"))
        {
            path.markTurn();
        }
        if (GUILayout.Button ("Next Turn"))
        {
            path.nextTurnTarget();
        }
        GUILayout.EndHorizontal();

    }


}
