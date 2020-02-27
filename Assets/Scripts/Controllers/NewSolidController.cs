using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSolidController : RaycastController
{
    public LayerMask passengerMask;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Move(Vector2 move)
    {
        UpdateRaycastOrigins();

        MovePassengers(move);
        transform.Translate(move);
    }

    private void MovePassengers(Vector2 move)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();

        float xSign = Mathf.Sign(move.x);
        float ySign = Mathf.Sign(move.y);

        if (move.y != 0)
        {
            float rayLength = Mathf.Abs(move.y) + skinWidth;
            for (int i = 0; i < vRayCount; i++)
            {
                Vector2 rayOrigin = ySign < 0 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (vRaySpacing * i);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * ySign, rayLength, passengerMask);
                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = ySign == 1 ? move.x : 0;
                        float pushY = move.y - (hit.distance - skinWidth) * ySign;

                        hit.transform.Translate(new Vector2(pushX, pushY));
                    }
                }
            }
        }

        if (move.x != 0)
        {
            float rayLength = Mathf.Abs(move.x) + skinWidth;
            for (int i = 0; i < hRayCount; i++)
            {
                Vector2 rayOrigin = xSign < 0 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (hRaySpacing * i);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * xSign, rayLength, passengerMask);
                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = move.x - (hit.distance - skinWidth) * xSign;
                        float pushY = 0;

                        hit.transform.Translate(new Vector2(pushX, pushY));
                    }
                }
            }
        }

        if (ySign == -1 || move.y == 0 && move.x != 0)
        {
            float rayLength = skinWidth * 2;
            for (int i = 0; i < vRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (vRaySpacing * i);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);
                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = move.x;
                        float pushY = move.y;

                        hit.transform.Translate(new Vector2(pushX, pushY));
                    }
                }
            }
        }
    }
}
