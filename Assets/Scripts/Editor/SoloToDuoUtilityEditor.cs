using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoloToDuoUtility))]
public class SoloToDuoUtilityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SoloToDuoUtility myScript = (SoloToDuoUtility)target;

        if (GUILayout.Button("Switch to Solo mode"))
        {
            myScript.SwitchToSolo();
            EditorWindow.GetWindow<SceneView>().Repaint();
        }
        if (GUILayout.Button("Switch to Duo mode"))
        {
            myScript.SwitchToDuo();
            EditorWindow.GetWindow<SceneView>().Repaint();
        }
        DrawDefaultInspector();
    }
}