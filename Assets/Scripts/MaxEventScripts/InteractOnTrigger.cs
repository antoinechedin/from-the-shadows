using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class InteractOnTrigger : MonoBehaviour
{
    public LayerMask layers;
    public bool triggerOnEnable = false;
    public UnityEvent OnEnter;
    
    public float delay = 5f;
    public UnityEvent OnEnterDelayed;

    public List<DelayedEvent> delayedEvents;

    [Space(15)]
    public UnityEvent OnExit;
    new Collider2D collider;

    void Reset()
    {
        layers = LayerMask.NameToLayer("Everything");
        collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    private void OnEnable()
    {
        if(triggerOnEnable)
        {
            ExecuteOnEnter();

            StartCoroutine(ExecuteOnEnterDelayed(delay));

            foreach (DelayedEvent delayedEvent in delayedEvents)
                StartCoroutine(delayedEvent.ExecuteOnEnterDelayed(delayedEvent.delay));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (0 != (layers.value & 1 << other.gameObject.layer))
        {
            ExecuteOnEnter();

            StartCoroutine(ExecuteOnEnterDelayed(delay));

            foreach (DelayedEvent delayedEvent in delayedEvents)
                StartCoroutine(delayedEvent.ExecuteOnEnterDelayed(delayedEvent.delay));
        }
    }

    protected virtual void ExecuteOnEnter()
    {
        OnEnter.Invoke();
    }

    IEnumerator ExecuteOnEnterDelayed(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        OnEnterDelayed.Invoke();
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (0 != (layers.value & 1 << other.gameObject.layer))
        {
            ExecuteOnExit();
        }
    }

    protected virtual void ExecuteOnExit()
    {
        OnExit.Invoke();
    }

    [System.Serializable]
    public class DelayedEvent
    {
        public float delay;
        public UnityEvent OnDelayEvent;

        public DelayedEvent()
        {

        }

        public IEnumerator ExecuteOnEnterDelayed(float _delay)
        {
            yield return new WaitForSeconds(_delay);
            OnDelayEvent.Invoke();
        }

    }

}