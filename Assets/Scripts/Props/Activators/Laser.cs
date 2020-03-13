using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : ActivatorListener
{
    private LineRenderer lineRenderer;
    private LayerMask collisionMask;
    private Vector3[] points;
    private bool active;
    private Receptor toDeactivate;

    public float range = 100;
    public int maxReflection = 5;

    // Shader Effect
    public Material dissolve;
    public float increaseValue = 5;
    private float value = -110;
    private float max = 100;

    // Use this for initialization
    void Start()
    {
        points = new Vector3[maxReflection + 2];
        lineRenderer = GetComponent<LineRenderer>();
        collisionMask = LayerMask.GetMask("LayeredSolid", "Solid", "Player", "Reflector");
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            points[0] = transform.position;
            CalculateRays(transform.position, transform.right, 1);

            // Shader Effect
            ShaderEffect();
        }
    }

    void ShaderEffect()
    {
        dissolve.SetFloat("Vector1_149EC6A4", value / max);
        value += 5;
    }

    void DrawRays(int nbPoints)
    {
        lineRenderer.positionCount = nbPoints;
        for (int i = 0; i< nbPoints; i ++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }
    }

    void CalculateRays(Vector3 point, Vector3 direction, int index)
    {
        RaycastHit2D hit = Physics2D.Raycast(point, direction, range, collisionMask);
        if (index >= maxReflection + 2)
        {
            DrawRays(index);
        } 
        else if (hit.collider != null && index < maxReflection + 2)
        {
            // Case where the laser reach something
            points[index] = hit.point;
            GameObject go = hit.collider.gameObject;
            if (go.GetComponent<Receptor>() != null)
            {
                // If it is a receptor : stop and activate
                DrawRays(index + 1);
                go.GetComponent<Receptor>().On(gameObject);
                toDeactivate = go.GetComponent<Receptor>();
            } 
            else if (go.layer == LayerMask.NameToLayer("Reflector"))
            {
                // If it is a reflector : continue
                Vector3 dir = Vector3.Reflect(-direction, hit.collider.transform.up);
                float angle = Vector3.Angle(-direction, dir);
                if (angle != 180)
                {
                    CalculateRays(new Vector3(hit.point.x, hit.point.y, 0) + (dir * 0.15f), dir, index + 1);
                } else
                {
                    // Case where the laser is parallele to the reflector
                    DrawRays(index + 1);
                    if (toDeactivate != null)
                        toDeactivate.Off(gameObject);
                }                
            }
            else
            {
                // If it is an obstacle : stop
                DrawRays(index + 1);
                if (toDeactivate != null)
                    toDeactivate.Off(gameObject);
            }
        }
        else
        {
            // Case where the laser doesn't touch anything
            points[index] = point + (direction * range);
            if (toDeactivate != null)
                toDeactivate.Off(gameObject);            
        }
    }

    public override void OnActivate()
    {
        if(!active){
            active = true;
        }
    }

    public override void OnDeactivate()
    {
        if(active){
            active = false;
            Clear();
        }
        if (toDeactivate != null)
            toDeactivate.Off(gameObject);
    }

    private void Clear(){;
        lineRenderer.positionCount = 0;
        points = new Vector3[maxReflection + 2];
    }
}
