using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target;
    public bool lockYAxis = false;
    private bool followInstant = false;

    public void SetFollowInstant(bool instant)
    {
        followInstant = instant;
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }
    void Update()
    {
        if(target != null)
        {
            if (followInstant)
            {
                transform.LookAt(target);
            }
            else
            {
                Vector3 targetDirection = target.position - transform.position;

                if(lockYAxis)
                    targetDirection.y = 0;

                if(targetDirection == Vector3.zero) targetDirection = Vector3.forward;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), 8 * Time.deltaTime);
            }
        }
    }
}
