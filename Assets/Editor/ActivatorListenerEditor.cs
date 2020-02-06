using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActivatorListener), true)]
public class ActivatorListenerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ActivatorListener listener = target as ActivatorListener;
        
        if(GUILayout.Button("Update Activators references"))
        {
           listener.UpdateActivatorReferences();
        }
    }
}