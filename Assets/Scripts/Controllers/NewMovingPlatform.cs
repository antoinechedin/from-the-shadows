using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NewSolidController))]
public class NewMovingPlatform : MonoBehaviour, IResetable
{
    private NewSolidController solid;
    private float timer;
    private Vector3 start;
    private Vector3 end;

    private bool awake;

    public float cycleDuration = 4;
    public float startOffset = 0.25f;

    private void Awake()
    {
        solid = GetComponent<NewSolidController>();
        if (transform.childCount < 2)
        {
            Debug.LogWarning(
                "WARN MovingPlatform.Awake: Can't find start/end point of " + Utils.GetFullName(transform)
           );
        }
        else
        {
            start = transform.GetChild(0).position;
            end = transform.GetChild(1).position;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            awake = true;
        }
    }

    private float MoveFunc(float x)
    {
        return (1 - Mathf.Cos(2 * Mathf.PI * (x / cycleDuration + startOffset))) / 2f;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (awake)
        {
            Vector2 path = end - start;
            Vector3 newPosition = start + (Vector3)path * MoveFunc(timer);

            solid.Move(newPosition - transform.position);
        }
    }

    public void Reset()
    {
        timer = 0;
    }

    public void CenterPlatformInPath()
    {
        if (transform.childCount < 2)
        {
            Debug.LogWarning(
                "WARN MovingPlatform.CenterPlatformInPath: Can't find start/end point. Platform can't be center"
            );
        }
        else
        {
            Transform start = transform.GetChild(0);
            Transform end = transform.GetChild(1);
            Vector3 pathCenter = (end.position + start.position) / 2f;
            Vector3 move = pathCenter - transform.position;
            transform.Translate(move);
            start.Translate(-move);
            end.Translate(-move);
        }
    }

    private void OnDrawGizmos()
    {
        if (awake)
        {
            Gizmos.color = Color.cyan;
            Vector2 boxSize = transform.TransformVector(solid.boxCollider.size);
            
            Gizmos.DrawWireCube(start, boxSize);
            Gizmos.DrawWireCube(end, boxSize);
            Gizmos.DrawLine(
                start + (Vector3)(boxSize * new Vector2(-0.5f, -0.5f)),
                end + (Vector3)(boxSize * new Vector2(-0.5f, -0.5f))
            );
            Gizmos.DrawLine(
                start + (Vector3)(boxSize * new Vector2(-0.5f, 0.5f)),
                end + (Vector3)(boxSize * new Vector2(-0.5f, 0.5f))
            );
            Gizmos.DrawLine(
                start + (Vector3)(boxSize * new Vector2(0.5f, 0.5f)),
                end + (Vector3)(boxSize * new Vector2(0.5f, 0.5f))
            );
            Gizmos.DrawLine(
                start + (Vector3)(boxSize * new Vector2(0.5f, -0.5f)),
                end + (Vector3)(boxSize * new Vector2(0.5f, -0.5f))
            );
        }
        else if (transform.childCount >= 2 && GetComponent<NewSolidController>() != null)
        {
            NewSolidController solid = GetComponent<NewSolidController>();
            if (solid.GetComponent<BoxCollider2D>() != null)
            {
                Gizmos.color = Color.cyan;
                Transform start = transform.GetChild(0);
                Transform end = transform.GetChild(1);
                Vector2 boxSize = transform.TransformVector(solid.GetComponent<BoxCollider2D>().size);

                Gizmos.DrawWireCube(start.position, boxSize);
                Gizmos.DrawWireCube(end.position, boxSize);
                Gizmos.DrawLine(
                    start.position + (Vector3)(boxSize * new Vector2(-0.5f, -0.5f)),
                    end.position + (Vector3)(boxSize * new Vector2(-0.5f, -0.5f))
                );
                Gizmos.DrawLine(
                    start.position + (Vector3)(boxSize * new Vector2(-0.5f, 0.5f)),
                    end.position + (Vector3)(boxSize * new Vector2(-0.5f, 0.5f))
                );
                Gizmos.DrawLine(
                    start.position + (Vector3)(boxSize * new Vector2(0.5f, 0.5f)),
                    end.position + (Vector3)(boxSize * new Vector2(0.5f, 0.5f))
                );
                Gizmos.DrawLine(
                    start.position + (Vector3)(boxSize * new Vector2(0.5f, -0.5f)),
                    end.position + (Vector3)(boxSize * new Vector2(0.5f, -0.5f))
                );
            }
        }
    }
}
