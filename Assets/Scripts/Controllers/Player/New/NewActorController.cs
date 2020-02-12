using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class NewActorController : MonoBehaviour
{
    public LayerMask collisionMask = 1 << 9;
    public float maxSlopeAngle = 60;

    private const float skinWidth = 0.021f;

    private const float maxRaySpacing = 0.2f;

    private int hRayCount;
    private int vRayCount;
    private float hRaySpacing;
    private float vRaySpacing;

    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private RaycastOrigins raycastOrigins;

    public CollisionInfo collisions;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Kinematic;
        boxCollider = GetComponent<BoxCollider2D>();
        InitRaySpacing();
    }

    public Vector2 Move(Vector2 velocity, float deltaTime)
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        Vector2 move = velocity * deltaTime;
        collisions.moveOld = move;

        if (move.y < 0)
        {
            DescendSlope(ref move);
        }
        // Always call MoveX before MoveY
        if (move.x != 0) MoveX(ref move);
        if (move.y != 0) MoveY(ref move);


        body.MovePosition(body.position + move);
        collisions.move = move;
        return move / deltaTime;
    }

    private void MoveX(ref Vector2 move)
    {
        float xSign = Mathf.Sign(move.x);
        float rayLength = Mathf.Abs(move.x) + skinWidth;

        for (int i = 0; i < hRayCount; i++)
        {
            Vector2 rayOrigin = xSign < 0 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (hRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * xSign, rayLength, collisionMask);
            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (i == 0 && slopeAngle <= maxSlopeAngle)
                {
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        move = collisions.moveOld;
                    }

                    float dstToSlope = 0;
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        dstToSlope = hit.distance - skinWidth;
                        move.x -= dstToSlope * xSign;
                    }
                    ClimbSlope(ref move, slopeAngle);

                    move.x += dstToSlope * xSign;
                }

                if (!collisions.climbingSlope || slopeAngle > maxSlopeAngle)
                {
                    move.x = (hit.distance - skinWidth) * xSign;
                    rayLength = hit.distance;
                    if (collisions.climbingSlope)
                    {
                        move.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(move.x);
                    }

                    collisions.left = xSign < 0;
                    collisions.right = xSign >= 0;
                }
            }

            Debug.DrawRay(rayOrigin, Vector2.right * xSign * rayLength * 5, Color.red);
        }
    }

    private void MoveY(ref Vector2 move)
    {
        float ySign = Mathf.Sign(move.y);
        float rayLength = Mathf.Abs(move.y) + skinWidth;

        for (int i = 0; i < vRayCount; i++)
        {
            Vector2 rayOrigin = ySign < 0 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (vRaySpacing * i + move.x);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * ySign, rayLength, collisionMask);
            if (hit)
            {
                move.y = (hit.distance - skinWidth) * ySign;
                rayLength = hit.distance;

                if (collisions.climbingSlope)
                {
                    move.x = move.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(move.x);
                }

                collisions.bellow = ySign < 0;
                collisions.above = ySign >= 0;
            }


            Debug.DrawRay(rayOrigin, Vector2.up * ySign * rayLength * 5, Color.red);
        }

        if (collisions.climbingSlope)
        {
            float xSign = Mathf.Sign(move.x);
            rayLength = Mathf.Abs(move.x) + skinWidth;
            Vector2 rayOrigin = (xSign == -1 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * move.y;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * xSign, rayLength, collisionMask);
            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle)
                {
                    move.x = (hit.distance - skinWidth) * xSign;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    private void ClimbSlope(ref Vector2 move, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(move.x);
        float climbMoveY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        if (climbMoveY > move.y)
        {
            move.y = climbMoveY;
            move.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(move.x);
            collisions.bellow = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    private void DescendSlope(ref Vector2 move)
    {
        RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast(raycastOrigins.bottomLeft, Vector2.down,
            Mathf.Abs(move.y) + skinWidth, collisionMask
        );
        RaycastHit2D maxSlopeHitRight = Physics2D.Raycast(raycastOrigins.bottomRight, Vector2.down,
            Mathf.Abs(move.y) + skinWidth, collisionMask
        );

        SlideDown(maxSlopeHitLeft, ref move);
        SlideDown(maxSlopeHitRight, ref move);

        if (!collisions.slidingSlope)
        {
            float xSign = Mathf.Sign(move.x);
            Vector2 rayOrigin = xSign < 0 ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionMask);
            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != 0 && slopeAngle <= maxSlopeAngle)
                {
                    if (Mathf.Sign(hit.normal.x) == xSign)
                    {
                        if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(move.x))
                        {
                            float moveDistance = Mathf.Abs(move.x);
                            float descendMoveY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                            move.y -= descendMoveY;
                            move.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * xSign;

                            collisions.slopeAngle = slopeAngle;
                            collisions.descendingSlope = true;
                            collisions.bellow = true;
                        }
                    }
                }
            }
        }
    }

    private void SlideDown(RaycastHit2D hit, ref Vector2 move)
    {
        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle > maxSlopeAngle)
            {
                move.x = hit.normal.x * (Mathf.Abs(move.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);

                collisions.slopeAngle = slopeAngle;
                collisions.slidingSlope = true;
            }
        }
    }

    private void UpdateRaycastOrigins()
    {
        this.raycastOrigins.bottomLeft = new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.min.y);
        this.raycastOrigins.bottomRight = new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.min.y);
        this.raycastOrigins.topLeft = new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.max.y);
        this.raycastOrigins.topRight = new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.max.y);
    }

    private void InitRaySpacing()
    {
        float width = boxCollider.bounds.size.x;
        float height = boxCollider.bounds.size.y;

        this.hRayCount = Mathf.FloorToInt(height / maxRaySpacing) + 2;
        this.vRayCount = Mathf.FloorToInt(width / maxRaySpacing) + 2;
        this.hRaySpacing = height / (hRayCount - 1);
        this.vRaySpacing = width / (vRayCount - 1);
    }

    struct RaycastOrigins
    {
        public Vector2 bottomLeft, bottomRight, topLeft, topRight;
    }

    public struct CollisionInfo
    {
        public bool above, bellow, left, right;
        public bool climbingSlope, descendingSlope, slidingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector2 move;
        public Vector2 moveOld;

        public void Reset()
        {
            above = bellow = left = right = false;
            climbingSlope = descendingSlope = slidingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}
