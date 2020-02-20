using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PatrolUnit), true)]
public class GhostEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PatrolUnit ghost = (PatrolUnit)target;

        for (int i = 0; i < ghost.nbCheckPoint; i++)
        {
            GUILayout.Label("> Check Point " + i);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Set CheckPoint " + i))
                ghost.SetCheckPoint(i);
            if (GUILayout.Button("Go To CheckPoint " + i))
                ghost.GoToCheckPoint(i);
            EditorGUILayout.EndHorizontal();
        }

        base.OnInspectorGUI();
    }
}
