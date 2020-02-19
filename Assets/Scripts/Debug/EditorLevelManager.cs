using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelManager))]
public class EditorLevelManager : Editor
{
    public override void OnInspectorGUI()
    {
        EditorStyles.label.wordWrap = true;
        EditorGUILayout.LabelField("\n- StartSpawns and EndSpawns represents the parent GameObject that contains the lightSpawn and shadowSpawn for" +
            " the start and the end of the level.\n");

        base.OnInspectorGUI();
    }
}
