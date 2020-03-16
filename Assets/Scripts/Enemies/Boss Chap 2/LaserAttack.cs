using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : MonoBehaviour
{
    private LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(1, transform.forward * 100);
    }
}
