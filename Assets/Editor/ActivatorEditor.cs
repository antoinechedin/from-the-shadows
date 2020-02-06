using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Activator), true)]
public class ActivatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Activator activator = target as Activator;

        if(GUILayout.Button("Update Activators references"))
        {
           activator.UpdateListenerReferences();
        }
    }
}