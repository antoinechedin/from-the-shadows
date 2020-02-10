using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public List<ActivatorListener> listeners = new List<ActivatorListener>();
    public bool active;
    public bool hasTimer;
    public float timer;
    public Material activeMat;
    public Material inactiveMat;

    protected GameObject child;

    /// <summary>
    /// Activate delegate. Don't forget to test if(Activate != null) before
    /// calling it.
    /// </summary>
    public OnActivate Activate;
    public delegate void OnActivate();

    /// <summary>
    /// Deactivate delegate. Don't forget to test if(Deactivate != null) before
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
                Activate -= allListeners[i].OnActivate;
                Deactivate -= allListeners[i].OnDeactivate;
            }

            if (listeners.Contains(allListeners[i]) && !allListeners[i].activators.Contains(this))
            {
                allListeners[i].activators.Add(this);
                Activate += allListeners[i].OnActivate;
                Deactivate += allListeners[i].OnDeactivate;
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

    /// <summary>
    /// Activate the activator
    /// </summary>
    /// <param name="ignoreTimer"> Ignore timer reset (when the activator is active at the beginning of the level</param>
    protected void On(bool ignoreTimer)
    {
        if (Activate != null)
        {
            Activate();
            active = true;
            if (child != null)
            {
                child.GetComponent<MeshRenderer>().material = activeMat;
            }
            if (hasTimer && !ignoreTimer)
            {
                StartCoroutine(DeactivateAfterTimer());
            }
        }       
    }

    /// <summary>
    /// Deactivate the activator 
    /// </summary>
    protected void Off()
    {
        if (Deactivate != null)
        {
            Deactivate();
            active = false;
            if (child != null)
            {
                child.GetComponent<MeshRenderer>().material = inactiveMat;
            }
        }
    }

    /// <summary>
    /// Deactivate the activator at the end of the timer
    /// </summary>
    /// <returns></returns>
    protected IEnumerator DeactivateAfterTimer()
    {
        yield return new WaitForSeconds(timer);
        Off();
    }
}

