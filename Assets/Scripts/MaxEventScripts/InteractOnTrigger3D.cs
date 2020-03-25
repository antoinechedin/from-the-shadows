using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractOnTrigger3D : MonoBehaviour
{
    public LayerMask layers;
    public UnityEvent OnEnter;
    
    public float delay = 5f;
    public UnityEvent OnEnterDelayed, OnExit;
    new Collider collider;

    void Reset()
    {
        layers = LayerMask.NameToLayer("Everything");
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("a");
        if (0 != (layers.value & 1 << other.gameObject.layer))
        {
            ExecuteOnEnter(other);
            StartCoroutine(ExecuteOnEnterDelayed());
        }
    }

    protected virtual void ExecuteOnEnter(Collider other)
    {
        OnEnter.Invoke();
    }

    IEnumerator ExecuteOnEnterDelayed()
    {
        yield return new WaitForSeconds(delay);
        OnEnterDelayed.Invoke();
    }
    void OnTriggerExit(Collider other)
    {
        if (0 != (layers.value & 1 << other.gameObject.layer))
        {
            ExecuteOnExit(other);
        }
    }

    protected virtual void ExecuteOnExit(Collider other)
    {
        OnExit.Invoke();
    }
}
