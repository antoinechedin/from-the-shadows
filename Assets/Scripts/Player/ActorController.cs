using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class ActorController : MonoBehaviour
{
    #region GameplayFields

    public float skinWidth = 0.005f;
    public float maxClimbAngle = 60f;
    public float maxDescendSlope = 60f;
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
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.previousMoveVec = moveVec;

        if (moveVec.y < 0)
        {
            DescendSlope(ref moveVec);
        }
        if (moveVec.x != 0)
        {
            HorizontalCollision(ref moveVec);
        }
        if (moveVec.y != 0)
        {
            VerticalCollision(ref moveVec);
        }
        rb.MovePosition((Vector2)transform.position + moveVec);
    }

    private void HorizontalCollision(ref Vector2 moveVec)
    {
        float directionX = Mathf.Sign(moveVec.x);
        float rayLength = Mathf.Abs(moveVec.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        moveVec = collisions.previousMoveVec;
                    }
                    
                    float distanceToSlope = 0;
                    if (slopeAngle != collisions.previousSlopeAngle)
                    {
                        distanceToSlope = hit.distance - skinWidth;
                        moveVec.x -= distanceToSlope * directionX;
                    }
                    ClimbSlope(ref moveVec, slopeAngle);
                    moveVec.x += distanceToSlope * directionX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    moveVec.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (collisions.climbingSlope)
                    {
                        moveVec.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveVec.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    private void VerticalCollision(ref Vector2 moveVec)
    {
        float directionY = Mathf.Sign(moveVec.y);
        float rayLength = Mathf.Abs(moveVec.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveVec.x);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            if (hit)
            {
                moveVec.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if (collisions.climbingSlope)
                {
                    moveVec.x = moveVec.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveVec.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }

        if (collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(moveVec.x);
            rayLength = Mathf.Abs(moveVec.x) + skinWidth;
            Vector2 rayOrigin =
                ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight)
                + Vector2.up * moveVec.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle)
                {
                    moveVec.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    private void ClimbSlope(ref Vector2 moveVec, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(moveVec.x);
        float climbMoveVecY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (moveVec.y <= climbMoveVecY)
        {
            moveVec.y = climbMoveVecY;
            moveVec.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Math.Sign(moveVec.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    private void DescendSlope(ref Vector2 moveVec)
    {
        float directionX = Mathf.Sign(moveVec.x);
        Vector2 rayOrigin = directionX == -1 ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendSlope)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveVec.y))
                    {
                        float moveDistance = Mathf.Abs(moveVec.x);
                        float descendMoveVecY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        moveVec.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Math.Sign(moveVec.x);
                        moveVec.y -= descendMoveVecY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
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
        public bool climbingSlope, descendingSlope;
        public float slopeAngle, previousSlopeAngle;
        public Vector2 previousMoveVec;

        public void Reset()
        {
            above = below = left = right = false;
            climbingSlope = descendingSlope = false;
            previousSlopeAngle = slopeAngle;
            slopeAngle = 0;
        }
    }
}



