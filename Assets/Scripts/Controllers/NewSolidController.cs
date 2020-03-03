using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSolidController : RaycastController
{
    public LayerMask passengerMask;
    [HideInInspector]
    public HashSet<Transform> grabingActors = new HashSet<Transform>();

    protected override void Awake()
    {
        base.Awake();
    }

    public void Move(Vector2 move)
    {
        UpdateRaycastOrigins();

        // foreach (Transform actors in grabingActors)
        // {
        //     actors.Translate(move);
        // }
        // grabingActors.Clear();
        MovePassengers(move);
        transform.Translate(move);
    }

    private void MovePassengers(Vector2 move)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();

        float xSign = Mathf.Sign(move.x);
        float ySign = Mathf.Sign(move.y);

        float rayLength = skinWidth * 2;

        Vector2 rayOrigin = raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.left, rayLength, passengerMask);
        if (hit && hit.transform.GetComponent<ActorController>() != null)
        {
            ActorController actor = hit.transform.GetComponent<ActorController>();
            if (!actor.collisions.bellow && !movedPassengers.Contains(hit.transform))
            {
                movedPassengers.Add(hit.transform);
                float pushX = move.x;
                float pushY = move.y;

                hit.transform.Translate(new Vector2(pushX, pushY));
            }
        }
        rayOrigin = raycastOrigins.bottomRight;
        hit = Physics2D.Raycast(rayOrigin, Vector2.right, rayLength, passengerMask);
        if (hit && hit.transform.GetComponent<ActorController>() != null)
        {
            ActorController actor = hit.transform.GetComponent<ActorController>();
            if (!actor.collisions.bellow && !movedPassengers.Contains(hit.transform))
            {
                movedPassengers.Add(hit.transform);
                float pushX = move.x;
                float pushY = move.y;

                hit.transform.Translate(new Vector2(pushX, pushY));
            }
        }


        if (move.y != 0)
        {
            rayLength = Mathf.Abs(move.y) + skinWidth;
            for (int i = 0; i < vRayCount; i++)
            {
                rayOrigin = ySign < 0 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (vRaySpacing * i);

                hit = Physics2D.Raycast(rayOrigin, Vector2.up * ySign, rayLength, passengerMask);
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
            rayLength = Mathf.Abs(move.x) + skinWidth;
            for (int i = 0; i < hRayCount; i++)
            {
                rayOrigin = xSign < 0 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (hRaySpacing * i);

                hit = Physics2D.Raycast(rayOrigin, Vector2.right * xSign, rayLength, passengerMask);
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
            rayLength = skinWidth * 2;
            for (int i = 0; i < vRayCount; i++)
            {
                rayOrigin = raycastOrigins.topLeft + Vector2.right * (vRaySpacing * i);

                hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);
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
