using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{

    public float speed;
    public float laserLength;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        lineRenderer.SetPosition(0, gameObject.transform.position);

        LayerMask collisionMask = LayerMask.GetMask("LayeredSolid", "Solid", "Player");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, laserLength, collisionMask);
        if (hit.collider != null)
        {
            if (hit.transform.GetComponent<PlayerController>() != null) //si on capte le joueur
            {
                hit.transform.GetComponent<PlayerController>().Die();
            }
            else//on a touché un élément de décors
            {
                Debug.Log("element de decors");
                lineRenderer.SetPosition(1, hit.point);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + (transform.up * laserLength));
        }

        //détection de collision pour la destruction complète du laser
        LayerMask destructMask = LayerMask.GetMask("LayeredSolid", "Solid");
        if (Physics2D.OverlapCircle(transform.position, 0.01f, destructMask))
        {
            Destroy(gameObject);
        }
    }
}
