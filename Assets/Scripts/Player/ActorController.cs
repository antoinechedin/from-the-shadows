using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class ActorController : MonoBehaviour
{
    #region GameplayFields

    public float skinWidth = 0.015f;
    public LayerMask collisionMask = 1 << 9; // 9 is the Obstacle layer id

    #endregion

    // private field. You shouldn't change them unless you knwon what you're
    // doing.
    [Range(2, 50)]
    private int horizontalRayCount = 6;
    private int verticalRayCount = 6;
    private float horizontalRaySpacing;
    private float verticalRaySpacing;

    // Components and other stuff. 
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private RaycastOrigins raycastOrigins;
    public CollisionInfos collisions;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        ComputeRaySpacing();
    }

    public void Move(Vector2 moveVec)
    {
        collisions.Reset();
        UpdateRaycastOrigins();

        HorizontalCollision(ref moveVec);
        VerticalCollision(ref moveVec);
        rb.MovePosition((Vector2)transform.position + moveVec);
    }

    private void HorizontalCollision(ref Vector2 moveVec)
    {
        float sign = Mathf.Sign(moveVec.x);
        float rayLength = Mathf.Abs(moveVec.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (sign == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * sign, rayLength, collisionMask);
            if (hit)
            {
                moveVec.x = (hit.distance - skinWidth) * sign;
                rayLength = hit.distance;

                collisions.left = sign == -1;
                collisions.right = sign == 1;
            }
        }
    }

    private void VerticalCollision(ref Vector2 moveVec)
    {
        float sign = Mathf.Sign(moveVec.y);
        float rayLength = Mathf.Abs(moveVec.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (sign == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveVec.x);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * sign, rayLength, collisionMask);
            if (hit)
            {
                moveVec.y = (hit.distance - skinWidth) * sign;
                rayLength = hit.distance;

                collisions.below = sign == -1;
                collisions.above = sign == 1;
            }
        }
    }

    /// <summary>
    /// Update origin position of the raycast use for collision detection. This
    /// method has to be called each time before moving.
    /// </summary>
    private void UpdateRaycastOrigins()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    /// <summary>
    /// Compute the rayspacing field. It depends of the boxCollider size and the
    /// raySpacing fields. Unless rayCount values change during runtime, they
    /// shouldn't change.
    /// </summary>
    private void ComputeRaySpacing()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }


    private struct RaycastOrigins
    {
        public Vector2 bottomLeft, bottomRight, topLeft, topRight;
    }

    public struct CollisionInfos
    {
        public bool above, below, left, right;
        public void Reset()
        {
            above = below = left = right = false;
        }
    }
}



