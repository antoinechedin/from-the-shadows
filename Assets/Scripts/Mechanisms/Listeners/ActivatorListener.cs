using System.Collections.Generic;
using UnityEngine;

public abstract class ActivatorListener : MonoBehaviour
{
    public List<Activator> activators = new List<Activator>();
    
    public abstract void OnActivate();

    public abstract void OnDeactivate();

    /// <summary>
    /// Used by the ActivatorEditor class to update Activator/Activable references.static
    /// Do not call this method outside of the Editor.
    /// </summary>
    public void UpdateActivatorReferences()
    {
        Activator[] allActivators = GameObject.FindObjectsOfType<Activator>();
        for (int i = 0; i < allActivators.Length; i++)
        {
            if (allActivators[i].listeners.Contains(this) && !activators.Contains(allActivators[i]))
            {
                allActivators[i].listeners.Remove(this);
            }

            if (activators.Contains(allActivators[i]) && !allActivators[i].listeners.Contains(this))
            {
                allActivators[i].listeners.Add(this);
            }
        }
    }
}
