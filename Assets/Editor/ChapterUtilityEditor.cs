using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChapterUtility))]
public class LevelScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Label("/!\\ Before Using : be sure to Add the following tags :");
        GUILayout.Label("- Level : GameObjects including the level content");
        GUILayout.Label("- InvisibleWalls : GameObject including all Invisible Wall");
        GUILayout.Label("- Triggers : GameObject including all Triggers of a level");
        ChapterUtility myScript = (ChapterUtility)target;

        if (GUILayout.Button("Init ChapterUtility"))
        {
            myScript.LevelScriptInit();
            EditorWindow.GetWindow<SceneView>().Repaint();
        }
        DrawDefaultInspector(); 
    }
}
