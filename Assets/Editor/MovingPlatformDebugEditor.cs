using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingPlatformDebug), true)]
public class MovingPlatformDebugEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MovingPlatformDebug mpd = target as MovingPlatformDebug;

        if(GUILayout.Button("Toggle draw debug"))
        {
            ToggleDrawDebug(mpd);
        }
    }

    private void ToggleDrawDebug(MovingPlatformDebug mpd)
    {
        mpd.ToggleDrawDebug();
    }
}
