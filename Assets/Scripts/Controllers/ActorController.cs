using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ActorController : RaycastController
{

    [HideInInspector] public float maxSlopeAngle = 60;

    private float minFloorLength = 0.01f;

    private Rigidbody2D body;
    public CollisionInfo collisions;
    public CollisionInfo collisionsPrevious;

    protected override void Awake()
    {
        base.Awake();
        body = GetComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Dynamic;
    }

    public void Move(Vector2 velocity, float deltaTime)
    {
        Move(velocity * deltaTime);
    }

    public void Move(Vector2 move)
    {
        UpdateRaycastOrigins();
        collisionsPrevious = collisions;
        collisions.Reset();
        collisionsPrevious.move = move;

        if (move.y < 0)
        {
            DescendSlope(ref move);
        }
        // Always call MoveX before MoveY
        if (move.x != 0) MoveX(ref move);
        if (move.y != 0) MoveY(ref move);

        if (collisionsPrevious.move.y < 0 && collisionsPrevious.bellow && !collisions.bellow)
        {
            GroundActor(ref move);
        }

        transform.Translate(move);
        collisions.move = move;
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
                        move = collisionsPrevious.move;
                    }

                    float dstToSlope = 0;
                    if (slopeAngle != collisionsPrevious.slopeAngle)
                    {

                        dstToSlope = Mathf.Clamp(hit.distance - skinWidth, 0, Mathf.Infinity);
                        move.x -= dstToSlope * xSign;
                    }
                    ClimbSlope(ref move, slopeAngle, hit.normal);

                    move.x += dstToSlope * xSign;
                }

                if (!collisions.climbingSlope || slopeAngle > maxSlopeAngle)
                {
                    move.x = (Mathf.Clamp(hit.distance - skinWidth, 0, Mathf.Infinity)) * xSign;
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
                move.y = (Mathf.Clamp(hit.distance - skinWidth, 0, Mathf.Infinity)) * ySign;
                rayLength = hit.distance;

                if (collisions.climbingSlope)
                {
                    move.x = move.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(move.x);
                }

                if (ySign < 0)
                {
                    collisions.bellow = true;
                    collisions.groundNormal = hit.normal;
                }
                else
                {
                    collisions.above = ySign >= 0;
                }
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
                    move.x = (Mathf.Clamp(hit.distance - skinWidth, 0, Mathf.Infinity)) * xSign;
                    collisions.slopeAngle = slopeAngle;
                    collisions.groundNormal = hit.normal;
                }
            }
        }
    }

    private void ClimbSlope(ref Vector2 move, float slopeAngle, Vector2 slopeNormal)
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
            collisions.groundNormal = slopeNormal;
        }
    }

    private void DescendSlope(ref Vector2 move)
    {
        if (collisionsPrevious.bellow)
            TrySlideDown(ref move);

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
                    if (move.x != 0 && Mathf.Sign(hit.normal.x) == xSign)
                    {
                        if (Mathf.Clamp(hit.distance - skinWidth, 0, Mathf.Infinity) <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(move.x))
                        {
                            float moveDistance = Mathf.Abs(move.x);
                            float descendMoveY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                            move.y -= descendMoveY + hit.distance - skinWidth;
                            move.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * xSign;

                            collisions.slopeAngle = slopeAngle;
                            collisions.descendingSlope = true;
                            collisions.bellow = true;
                            collisions.groundNormal = hit.normal;
                        }
                    }
                }
            }
        }
    }

    private void TrySlideDown(ref Vector2 move)
    {
        bool slidingLeft = false;
        bool slidingRight = false;

        RaycastHit2D hitLeft = Physics2D.Raycast(raycastOrigins.bottomLeft, Vector2.down,
            Mathf.Abs(move.y) + skinWidth, collisionMask
        );
        RaycastHit2D hitRight = Physics2D.Raycast(raycastOrigins.bottomRight, Vector2.down,
            Mathf.Abs(move.y) + skinWidth, collisionMask
        );

        if (hitLeft)
        {
            float slopeAngle = Vector2.Angle(hitLeft.normal, Vector2.up);
            if (slopeAngle > maxSlopeAngle) slidingLeft = true;
        }

        if (hitRight)
        {
            float slopeAngle = Vector2.Angle(hitRight.normal, Vector2.up);
            if (slopeAngle > maxSlopeAngle) slidingRight = true;
        }

        if (slidingLeft && !slidingRight)
        {
            if (!hitRight || hitRight.distance > hitLeft.distance)
            {
                SlideDown(hitLeft, ref move);
            }
        }
        else if (slidingRight && !slidingLeft)
        {
            if (!hitLeft || hitLeft.distance > hitRight.distance)
            {
                SlideDown(hitRight, ref move);
            }
        }
    }

    private void SlideDown(RaycastHit2D hit, ref Vector2 move)
    {
        float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

        move.x = hit.normal.x * (Mathf.Abs(move.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);

        collisions.slopeAngle = slopeAngle;
        collisions.slidingSlope = true;
        collisions.bellow = true;
        collisions.groundNormal = hit.normal;
    }

    private void GroundActor(ref Vector2 move)
    {
        float xSign = Math.Sign(move.x);
        float rayLength = Mathf.Infinity;
        float dst2Ground = 0;

        for (int i = 0; i < vRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.bottomLeft + move;
            rayOrigin += Vector2.right * vRaySpacing * i;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, collisionMask);
            if (hit)
            {
                rayLength = hit.distance;

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle <= maxSlopeAngle)
                {
                    float maxDst2Ground =
                        Mathf.Sin(collisionsPrevious.slopeAngle * Mathf.Deg2Rad) * move.magnitude
                        + Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(move.x);
                    if (rayLength - skinWidth <= maxDst2Ground)
                    {
                        dst2Ground = rayLength - skinWidth;
                        collisions.bellow = true;
                        collisions.slopeAngle = slopeAngle;
                        collisions.groundNormal = hit.normal;
                        if (hit.normal.x != 0 && Mathf.Sign(hit.normal.x) == xSign) collisions.descendingSlope = true;
                    }
                }
            }
        }
        move.y -= dst2Ground;
    }

    public bool LedgeGrab(float facing, bool checkOnly)
    {
        float heightOffset = 0.5f;
        float floorOffset = 0.08f;
        float hLedgeGrabRayCount = Mathf.FloorToInt(Mathf.Abs(collisions.move.y) / maxRaySpacing) + 2;
        float hLedgeGrabRaySpacing = Mathf.Clamp(Mathf.Abs(collisions.move.y), maxRaySpacing, Mathf.Infinity)
                                     / (hLedgeGrabRayCount - 1);
        hLedgeGrabRayCount += 5;
        float ledgeGrabRayLength = skinWidth + minFloorLength;

        for (int i = 0; i < hLedgeGrabRayCount; i++)
        {
            Vector2 rayOrigin = facing < 0 ? raycastOrigins.topLeft : raycastOrigins.topRight;
            rayOrigin += Vector2.up * (collisions.move.y - (hLedgeGrabRaySpacing * i) + heightOffset);
            rayOrigin += Vector2.right * collisions.move.x;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * facing, ledgeGrabRayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * facing * ledgeGrabRayLength, hit ? Color.blue : Color.yellow);

            if (hit)
            {
                if (i == 0) return false;
                //if (hit.normal.y < Mathf.Sin(45f * Mathf.Deg2Rad)) return false;

                rayOrigin = facing < 0 ? raycastOrigins.topLeft : raycastOrigins.topRight;
                rayOrigin += Vector2.up * (collisions.move.y + heightOffset);
                rayOrigin += Vector2.right * collisions.move.x;
                RaycastHit2D headHit = Physics2D.Raycast(rayOrigin, Vector2.down, heightOffset, collisionMask);
                Debug.DrawRay(rayOrigin, Vector2.down * heightOffset, Color.blue);
                if (headHit) return false;

                if (hit.distance - skinWidth < minFloorLength)
                {
                    rayOrigin = facing < 0 ? raycastOrigins.topLeft : raycastOrigins.topRight;
                    rayOrigin += Vector2.up * (collisions.move.y + heightOffset);
                    rayOrigin += Vector2.right * collisions.move.x;
                    rayOrigin += Vector2.right * facing * (hit.distance + minFloorLength);

                    if (!checkOnly)
                    {
                        RaycastHit2D floorHit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionMask);
                        if (floorHit)
                        {
                            Debug.DrawRay(rayOrigin, Vector2.down * floorHit.distance, Color.magenta);
                            if (Mathf.Sign(floorHit.normal.x) != facing && floorHit.normal.y < Mathf.Sin(maxSlopeAngle * Mathf.Deg2Rad)) return false;
                            if (floorHit.distance < floorOffset) return false;

                            transform.Translate(Vector2.down * (floorHit.distance - floorOffset));
                        }
                    }
                    return true;
                }
                else return false;
            }
        }

        return false;
    }

    public struct CollisionInfo
    {
        public bool above, bellow, left, right;
        public bool climbingSlope, descendingSlope, slidingSlope;
        public float slopeAngle;
        public Vector2 move;
        public Vector2 groundNormal;

        public void Reset()
        {
            above = bellow = left = right = false;
            climbingSlope = descendingSlope = slidingSlope = false;
            slopeAngle = 0;
            groundNormal = Vector2.zero;
        }
    }
}
