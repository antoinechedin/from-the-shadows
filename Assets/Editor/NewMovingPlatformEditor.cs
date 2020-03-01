using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NewMovingPlatform), true)]
public class NewMovingPlatformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NewMovingPlatform movingPlatform = target as NewMovingPlatform;

        if(GUILayout.Button("Center platform in path"))
        {
            movingPlatform.CenterPlatformInPath();
        }
    }
}
