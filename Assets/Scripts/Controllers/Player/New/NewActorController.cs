using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class NewActorController : MonoBehaviour
{
    private float xRemainder = 0;
    private float yRemainder = 0;

    public LayerMask collisionMask = 1 << 9;
    private BoxCollider2D boxCollider;

    private float xMove;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void MoveX(float amount)
    {
        xRemainder += amount;
        float move = (float)System.Math.Round(xRemainder, 2);

        if (move != 0)
        {
            xRemainder -= move;
            float xSign = move < 0 ? -0.01f : 0.01f;
            float distance = 0;
            int stop = 0;
            while (Mathf.Abs(distance) <= Mathf.Abs(move) && stop < 100000)
            {
                stop++;
                Vector2 position = (Vector2)boxCollider.bounds.center + Vector2.right * distance;
                if (!Physics2D.OverlapBox(position + Vector2.right * xSign, boxCollider.bounds.size, 0, collisionMask))
                {
                    distance += xSign;
                }
                else
                {
                    xRemainder = 0;
                    break;
                }
            }
            //transform.position = transform.position + Vector3.right * distance;
            transform.Translate((float)System.Math.Round(distance, 2), 0, 0);
            xMove = (float)System.Math.Round(distance, 2);

            if (stop >= 100000) Debug.Log("no");
        }
    }

    public void MoveY(float amount)
    {
        yRemainder += amount;
        float move = (float)System.Math.Round(yRemainder, 2);

        if (move != 0)
        {
            yRemainder -= move;
            float ySign = move < 0 ? -0.01f : 0.01f;
            float distance = 0;
            int stop = 0;
            while (Mathf.Abs(distance) <= Mathf.Abs(move) && stop < 100000)
            {
                stop++;
                Vector2 position = (Vector2)boxCollider.bounds.center + Vector2.up * distance;
                if (!Physics2D.OverlapBox(position + Vector2.up * ySign + Vector2.right * xMove, boxCollider.bounds.size, 0, collisionMask))
                {
                    distance += ySign;
                }
                else
                {
                    yRemainder = 0;
                    break;
                }
            }
            //transform.position = transform.position + Vector3.right * distance;
            transform.Translate(0, (float)System.Math.Round(distance, 2), 0);

            if (stop >= 100000) Debug.Log("no");
            xMove = 0;
        }
    }
}
