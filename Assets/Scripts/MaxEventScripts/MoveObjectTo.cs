using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveObjectTo : MonoBehaviour
{
    private bool move = false;

    public bool cycle = false;
    public bool moveAtStart = false;
    public bool resetOnChangeTarget = false;

    private bool isFinalTarget = false;
    public float speed = 2f;
    public Transform target;

    private bool executed = false;
    public UnityEvent OnTargetReached, OnFinalTargetReached;

    private Vector3 basePosition;
    private Vector3 targettedPosition;

    private void Start()
    {
        basePosition = this.transform.position;

        if(target != null)
            targettedPosition = target.position;

        if (moveAtStart)
            move = true;
    }
    public void StartMoving()
    {
        move = true;
    }

    public void StopMoving()
    {
        move = false;
    }

    protected virtual void ExecuteOnTargetReached()
    {
        OnTargetReached.Invoke();
        executed = true;
    }
    protected virtual void ExecuteOnFinalTargetReached()
    {
        OnFinalTargetReached.Invoke();
        executed = true;
    }

    public void TeleportTo(Transform target)
    {
        this.transform.position = target.position;

    }

    public void IsFinalTarget(bool _isFinalTarget)
    {
        isFinalTarget = _isFinalTarget;
    }
    public void SetSpeed(float newSpeed)
    {
        this.speed = newSpeed;
    }

    public void SetTarget(Transform newTarget)
    {
        this.target = newTarget;
        targettedPosition = target.position;

        if (resetOnChangeTarget)
            executed = false;
    }

    void Update()
    {
        if(move)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime);

            if (this.transform.position == targettedPosition && !executed)
            {
                if (isFinalTarget)
                    ExecuteOnFinalTargetReached();
                else
                    ExecuteOnTargetReached();
            }
            if (this.transform.position == targettedPosition && cycle)
                target.position = basePosition;
            else if (this.transform.position == basePosition && cycle)
                target.position = targettedPosition;

        }
    }
}
