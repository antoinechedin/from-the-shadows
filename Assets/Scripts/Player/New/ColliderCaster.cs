using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ColliderCaster : MonoBehaviour
{
    [Range(0, 360)]
    public float castAngle;
    [Range(0, 50)]
    public float castLenght;

    public LayerMask collisionMask = 1 << 9;

    private BoxCollider2D boxCollider;
    private Vector2 direction;
    private RaycastHit2D hit;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        direction.x = Mathf.Cos(castAngle * Mathf.Deg2Rad);
        direction.y = Mathf.Sin(castAngle * Mathf.Deg2Rad);

        hit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            transform.rotation.z,
            direction,
            castLenght,
            collisionMask
        );
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

        if (hit)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(hit.centroid, 0.05f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hit.point, 0.05f);
            Gizmos.DrawRay(hit.point, hit.normal * 2);
        }

        Gizmos.color = new Color(1, 1, 0, 0.4f);
        Gizmos.DrawCube(bounds.center + direction * hit.distance, bounds.extents * 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(bounds.center, direction * hit.distance);
        Gizmos.DrawWireSphere(bounds.center + direction * hit.distance, 0.05f);
    }
}
