using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SolidController))]
public class MovingPlatform : ActivatorListener, IResetable
{
    public bool controledByActivators;
    private bool activated;

    private SolidController solid;
    public float timer;
    private Vector3 start;
    private Vector3 end;

    private bool awake;

    public float cycleDuration = 4;
    public float startOffset = 0.25f;

    private void Awake()
    {
        solid = GetComponent<SolidController>();
        Transform startTransform = transform.Find("Start");
        Transform endTransform = transform.Find("End");

        if (startTransform == null || endTransform == null)
        {
            Debug.LogWarning(
                "WARN MovingPlatform.Awake: Can't find start/end point of " + Utils.GetFullName(transform)
           );
        }
        else
        {
            start = startTransform.position;
            end = endTransform.position;
            startTransform.gameObject.SetActive(false);
            endTransform.gameObject.SetActive(false);
            awake = true;
        }
    }

    private float MoveFunc(float x)
    {
        return (1 - Mathf.Cos(2 * Mathf.PI * (x / cycleDuration + startOffset))) / 2f;
    }

    private float GotoFunc(float x)
    {
        return -1f / 2f * (Mathf.Cos(Mathf.PI * x / cycleDuration) - 1f);
    }

    private void FixedUpdate()
    {
        if (controledByActivators)
        {
            timer = Mathf.Clamp(timer + (activated ? Time.fixedDeltaTime : -Time.deltaTime), 0, cycleDuration);
            Vector3 newPosition = start + (end - start) * GotoFunc(timer);
            solid.Move(newPosition - transform.position);
        }
        else
        {
            timer += Time.fixedDeltaTime;
            Vector2 path = end - start;
            Vector3 newPosition = start + (Vector3)path * MoveFunc(timer);

            solid.Move(newPosition - transform.position);
        }
    }

    public void Reset()
    {
        timer = 0;
        activated = false;
    }

    public override void OnActivate()
    {
        activated = true;
    }

    public override void OnDeactivate()
    {
        activated = false;
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
        else if (transform.childCount >= 2 && GetComponent<SolidController>() != null)
        {
            SolidController solid = GetComponent<SolidController>();
            if (solid.GetComponent<BoxCollider2D>() != null)
            {
                Gizmos.color = gizmoColor;
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
