using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererDrawForward : MonoBehaviour
{
    public float lineDistance;

    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 aimPoint = transform.position + (transform.up * lineDistance);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, aimPoint, out hit, lineDistance))
        {
            if (hit.transform.GetComponent<PlayerController>() != null) //si on capte le joueur
            {
                hit.transform.GetComponent<PlayerController>().Die();
            }
            else//on a touché un élément de décors
            {
                lineRenderer.SetPosition(1, hit.point);
            }
        }
    }
}
