using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public List<ActivatorListener> listeners = new List<ActivatorListener>();

    /// <summary>
    /// Activate delegate. Don't forget to test if(Activate != null) before
    /// calling it.
    /// </summary>
    public OnActivate Activate;
    public delegate void OnActivate();

    /// <summary>
    /// Activate delegate. Don't forget to test if(Activate != null) before
    /// calling it.
    /// </summary>
    public OnDeactivate Deactivate;
    public delegate void OnDeactivate();

    /// <summary>
    /// Used by the ActivatorEditor class to update Listeners references.
    /// Do not call this method outside of the Editor.
    /// </summary>
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

    protected virtual void OnEnable()
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            if (listeners[i] != null)
            {
                Activate += listeners[i].OnActivate;
                Deactivate += listeners[i].OnDeactivate;
            }
        }
    }

     protected virtual void OnDisable()
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            if (listeners[i] != null)
            {
                Activate -= listeners[i].OnActivate;
                Deactivate -= listeners[i].OnDeactivate;
            }
        }
    }
}
