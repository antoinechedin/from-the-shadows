using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target;

    public void SetTarget(Transform t)
    {
        target = t;
    }
    void Update()
    {
        this.transform.LookAt(target);
    }
}
