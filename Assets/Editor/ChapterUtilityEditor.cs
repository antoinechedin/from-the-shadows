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
        GUILayout.Label("- CameraPoint : CameraPoints of each Level (1/lvl)");
        GUILayout.Label("- Put id of CameraPoint for example CameraPoint1");
        GUILayout.Label("- InvisibleWalls : GameObject including all Invisible Wall");
        GUILayout.Label("- Triggers : GameObject including all Triggers of a level");
        ChapterUtility myScript = (ChapterUtility)target;

        if (GUILayout.Button("Init ChapterUtility"))
        {
            myScript.LevelScriptInit();
            EditorWindow.GetWindow<SceneView>().Repaint();
        }


        DrawDefaultInspector();

        for(int i = 0; i < myScript.countCameraPoint; i++)
        {
            GUILayout.Label("> Camera Point " + i);
            if (GUILayout.Button("Go To CameraPoint LB " + i))
                myScript.GoToCameraPointLB(i) ;
            if (GUILayout.Button("Go To CameraPoint RT " + i))
                myScript.GoToCameraPointRT(i) ;
            if (GUILayout.Button("Set CameraPoint LB " + i))
                myScript.SetCameraPointLB(i);
            if (GUILayout.Button("Set CameraPoint RT " + i))
                myScript.SetCameraPointRT(i);
        }  
    }
}
