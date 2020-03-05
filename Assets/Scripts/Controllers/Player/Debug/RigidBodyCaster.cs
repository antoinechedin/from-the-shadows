using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class RigidBodyCaster : MonoBehaviour
{
    [Range(0, 360)]
    public float castAngle;
    [Range(0, 50)]
    public float castLenght;

    private Rigidbody2D rb;
    private Vector2 direction;
    private RaycastHit2D[] hitBuffer = new RaycastHit2D[8];
    private int hitCount;
    private ContactFilter2D contactFilter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(1 << 9);
        contactFilter.useLayerMask = true;
        contactFilter.useNormalAngle = true;
        contactFilter.minNormalAngle = 170;
        contactFilter.maxNormalAngle = 190;
    }

    private void Update()
    {
        direction.x = Mathf.Cos(castAngle * Mathf.Deg2Rad);
        direction.y = Mathf.Sin(castAngle * Mathf.Deg2Rad);

        hitCount = rb.Cast(direction, contactFilter, hitBuffer, castLenght);
    }

    private void OnDrawGizmos()
    {
        Vector3 direction = new Vector2(Mathf.Cos(castAngle * Mathf.Deg2Rad), Mathf.Sin(castAngle * Mathf.Deg2Rad));
        Bounds bounds = GetComponent<BoxCollider2D>().bounds;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(bounds.center + bounds.extents, direction * castLenght);
        Gizmos.DrawRay(bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y), direction * castLenght);
        Gizmos.DrawRay(bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y), direction * castLenght);
        Gizmos.DrawRay(bounds.center - bounds.extents, direction * castLenght);

        Gizmos.DrawWireCube(bounds.center + direction * castLenght, bounds.extents * 2);

        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D hit = hitBuffer[i];
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(hit.centroid, 0.05f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hit.point, 0.05f);
            Gizmos.DrawRay(hit.point, hit.normal * 2);

            Gizmos.color = new Color(1, 1, 0, 0.4f);
            Gizmos.DrawCube(bounds.center + direction * hit.distance, bounds.extents * 2);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(bounds.center, direction * hit.distance);
            Gizmos.DrawWireSphere(bounds.center + direction * hit.distance, 0.05f);
        }
    }
}
