using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class InteractOnTrigger : MonoBehaviour
{
    public LayerMask layers;
    public UnityEvent OnEnter;
    
    public float delay = 5f;
    public UnityEvent OnEnterDelayed, OnExit;
    new Collider2D collider;

    void Reset()
    {
        layers = LayerMask.NameToLayer("Everything");
        collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (0 != (layers.value & 1 << other.gameObject.layer))
        {
            ExecuteOnEnter(other);
            StartCoroutine(ExecuteOnEnterDelayed());
        }
    }

    protected virtual void ExecuteOnEnter(Collider2D other)
    {
        OnEnter.Invoke();
    }

    IEnumerator ExecuteOnEnterDelayed()
    {
        yield return new WaitForSeconds(delay);
        OnEnterDelayed.Invoke();
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (0 != (layers.value & 1 << other.gameObject.layer))
        {
            ExecuteOnExit(other);
        }
    }

    protected virtual void ExecuteOnExit(Collider2D other)
    {
        OnExit.Invoke();
    }
}
