using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : ActivatorListener
{
    private LineRenderer lineRenderer;
    private LayerMask collisionMask;
    private Vector3[] points;
    private Vector3 position;
    private bool active;

    public float range = 100;
    public int maxReflection = 0;

    // Use this for initialization
    void Start()
    {
        points = new Vector3[maxReflection + 1];
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = maxReflection + 2;
        collisionMask = LayerMask.GetMask("LayeredSolid", "Solid", "Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            points[0] = transform.position;

            CalculateRays();
            DrawRays();
        }
    }

    void DrawRays()
    {
        for (int i=0; i< maxReflection + 1; i ++)
        {
            if( points[i] != null)
            {
                lineRenderer.SetPosition(i, points[i]);
            }
        }
    }

    void CalculateRays()
    {
        for (int i = 1; i < maxReflection + 2; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(points[i-1], transform.right, range, collisionMask);

            if (hit.collider)
                lineRenderer.SetPosition(i, hit.point);
            else
                lineRenderer.SetPosition(i, transform.position + (transform.right * range));
        }
    }

    public override void OnActivate()
    {
        if(!active){
            lineRenderer.positionCount = maxReflection + 2;
            active = true;
        }
    }

    public override void OnDeactivate()
    {
        if(active){
            active = false;
            ClearLineRenderer();
        }
    }

    private void ClearLineRenderer(){;
        lineRenderer.positionCount = 0;
    }
}
