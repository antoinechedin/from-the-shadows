using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target;
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
        if(followInstant)
        {
            transform.LookAt(target);
        }
        else
        {
            Vector3 targetDirection = target.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), 8 * Time.deltaTime);
        }
    }
}
