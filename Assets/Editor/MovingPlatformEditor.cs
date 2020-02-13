using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingPlatform), true)]
public class MovingPlatformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MovingPlatform movingPlatform = target as MovingPlatform;

        if(GUILayout.Button("Create control points GameObjects"))
        {
            movingPlatform.CreateControlPointsGameObjects();
        }

        if(GUILayout.Button("Update control points array"))
        {
            movingPlatform.UpdateControlPointsArray();
        }
    }
}
