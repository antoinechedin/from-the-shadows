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
        LayerMask collisionMask = LayerMask.GetMask("LayeredSolid", "Solid", "Player");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, lineDistance, collisionMask);
        if (hit.collider != null)
        {
            Debug.Log("touché qq chose");
            if (hit.transform.GetComponent<PlayerController>() != null) //si on capte le joueur
            {
                Debug.Log("touché PLAYER");
                hit.transform.GetComponent<PlayerController>().Die();
            }   
            else//on a touché un élément de décors
            {
                Debug.Log(hit.point);
                lineRenderer.SetPosition(1, hit.point);
            }
        }
        else
        {
            Debug.Log("rien touché");
        }
    }
}
