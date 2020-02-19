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
        EditorGUILayout.LabelField("\n- Parent each light/shadow spawn point pairs to an object and the drag the parent gomeobject into the \"playerSpawns\" list.\n");
        EditorGUILayout.LabelField("\n- The ChangeLevelTriggers now have to have to reference to the new playerSpawn they should set it to\n");

        base.OnInspectorGUI();
    }
}
