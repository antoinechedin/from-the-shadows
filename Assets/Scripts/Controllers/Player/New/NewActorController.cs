using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class NewActorController : MonoBehaviour
{
    public LayerMask collisionMask = 1 << 9;

    private const float skinWidth = 0.02f;
    //private const float minMoveMagnitude = 0.001f;

    private const float maxRaySpacing = 0.2f;

    private int hRayCount;
    private int vRayCount;
    private float hRaySpacing;
    private float vRaySpacing;

    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private RaycastOrigins raycastOrigins;

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

        Vector2 move = velocity * deltaTime;

        // Always call MoveX before MoveY
        if (move.x != 0) MoveX(ref move);
        if (move.y != 0) MoveY(ref move);

        body.MovePosition(body.position + move);
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
                move.x = (hit.distance - skinWidth) * xSign;
                rayLength = hit.distance;
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
            }

            Debug.DrawRay(rayOrigin, Vector2.up * ySign * rayLength * 5, Color.red);
        }
    }

    private void UpdateRaycastOrigins()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        this.raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        this.raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        this.raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        this.raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    private void InitRaySpacing()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);
        float width = bounds.size.x;
        float height = bounds.size.y;

        this.hRayCount = Mathf.FloorToInt(height / maxRaySpacing) + 2;
        this.vRayCount = Mathf.FloorToInt(width / maxRaySpacing) + 2;
        this.hRaySpacing = height / (hRayCount - 1);
        this.vRaySpacing = width / (vRayCount - 1);
    }

    struct RaycastOrigins
    {
        public Vector2 bottomLeft, bottomRight, topLeft, topRight;
    }
}
