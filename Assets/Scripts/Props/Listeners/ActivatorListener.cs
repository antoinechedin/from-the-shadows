using System.Collections.Generic;
using UnityEngine;

public abstract class ActivatorListener : MonoBehaviour
{
    public enum Operator { Or, And };
    public List<Activator> activators = new List<Activator>();
    public Operator logic;
    public Color gizmoColor;
  
    /// <summary>
    /// Method called by the Activator.Activate delegate
    /// </summary>
    public abstract void OnActivate();

    /// <summary>
    /// Method called by the Activator.Deactivate delegate
    /// </summary>
    public abstract void OnDeactivate();

    /// <summary>
    /// Used by the ActivatorListenerEditor class to update Activators references.
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
                allActivators[i].TryActivate -= TryOnActivate;
                allActivators[i].TryDeactivate -= TryOnDeactivate;
            }

            if (activators.Contains(allActivators[i]) && !allActivators[i].listeners.Contains(this))
            {
                allActivators[i].listeners.Add(this);
                allActivators[i].TryActivate += TryOnActivate;
                allActivators[i].TryDeactivate += TryOnDeactivate;
            }
        }
    }

    public void TryOnActivate()
    {
        if (logic == Operator.Or)
            OnActivate();
        else if (logic == Operator.And && AllActivatorsActive())
            OnActivate();
    }

    public void TryOnDeactivate()
    {
        if (logic == Operator.And)
            OnDeactivate();
        if (logic == Operator.Or && AllActivatorsInactive())
            OnDeactivate();
    }

    public bool AllActivatorsActive()
    {
        bool res = true;
        foreach (Activator activator in activators)
        {
            if (!activator.active)
                res = false;
        }
        return res;
    }

    public bool AllActivatorsInactive()
    {
        bool res = true;
        foreach (Activator activator in activators)
        {
            if (activator.active)
                res = false;
        }
        return res;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b);
        Gizmos.DrawWireSphere(transform.position, 0.6f);
        foreach (Activator activator in activators)
        {
            if (activator != null) Gizmos.DrawWireSphere(activator.transform.position, 0.6f);
        }
    }
}
