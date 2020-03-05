using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderOverlap : MonoBehaviour
{
    public LayerMask collisionMask = 1 << 9;

    private BoxCollider2D boxCollider;
    private ColliderDistance2D[] colliderDistances;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            boxCollider.bounds.center,
            boxCollider.bounds.extents * 2,
            0,
            collisionMask
        );

        colliderDistances = new ColliderDistance2D[hits.Length];
        for (int i = 0; i < hits.Length; i++)
        {
            colliderDistances[i] = boxCollider.Distance(hits[i]);
        }
        
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Vector2 distance = new Vector2();
            for (int i = 0; i < colliderDistances.Length; i++)
            {   
                distance += colliderDistances[i].pointB - colliderDistances[i].pointA;
            }
            transform.Translate(distance);
        }
    }

    private void OnDrawGizmos()
    {
        Bounds bounds = GetComponent<BoxCollider2D>().bounds;

        if (colliderDistances != null)
        {
            Vector2 distance = new Vector2();
            for (int i = 0; i < colliderDistances.Length; i++)
            {   
                distance += colliderDistances[i].pointB - colliderDistances[i].pointA;

                Gizmos.color = colliderDistances[i].isValid ? Color.green : Color.red;
                Gizmos.DrawWireSphere(colliderDistances[i].pointA, 0.05f);
                Gizmos.DrawLine(colliderDistances[i].pointA, colliderDistances[i].pointB);
            }
            
            Gizmos.color = new Color(0, 1, 1, 0.3f);
            Gizmos.DrawCube(bounds.center + (Vector3)distance, bounds.extents * 2);

            //Debug.Log("Pos: ["+ transform.position.x + ", " + transform.position.y + "] # Distance: " + distance.magnitude);
        }
    }
}
