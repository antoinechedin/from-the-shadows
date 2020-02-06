using System;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public List<ActivatorListener> listeners = new List<ActivatorListener>();

    public delegate void OnActivate();
    public OnActivate Activate;

    public delegate void OnDeactivate();
    public OnDeactivate Deactivate;

    public void UpdateListenerReferences()
    {
        ActivatorListener[] allListeners = GameObject.FindObjectsOfType<ActivatorListener>();
        for (int i = 0; i < allListeners.Length; i++)
        {
            if (allListeners[i].activators.Contains(this) && !listeners.Contains(allListeners[i]))
            {
                allListeners[i].activators.Remove(this);
            }

            if (listeners.Contains(allListeners[i]) && !allListeners[i].activators.Contains(this))
            {
                allListeners[i].activators.Add(this);
            }
        }
    }
}
