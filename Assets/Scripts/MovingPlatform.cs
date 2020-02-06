using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SolidController))]
public class MovingPlatform : MonoBehaviour
{
    private Vector2 startingPoint;
    private Vector2 endingPoint;
    public Vector2 target;
    private Vector2 limit;

    public List<Vector2> controlPoints;
    private int cursor = 0;

    public float threshold;
    private int orientation = 1;

    private SolidController solidController;

    private void Awake()
    {
        solidController = GetComponent<SolidController>();
        startingPoint = controlPoints[0];
        endingPoint = controlPoints[controlPoints.Count - 1];
        target = controlPoints[1];
        limit = endingPoint;
    }

    private void Update()
    {
        FollowTrajectoryBackAndForth();
    }

    /// <summary>
    /// Moves towards given position
    /// </summary>
    /// <param name="target"></param>
    private void MoveTowardsTarget(Vector2 target)
    {
        solidController.Move((Vector3)target - transform.position);
    }

    /// <summary>
    /// Moves back and forth following control points array
    /// </summary>
    private void FollowTrajectoryBackAndForth()
    {
        MoveTowardsTarget(target);
        UpdateCursor(limit);
    }

    /// <summary>
    /// Updates cursor position
    /// </summary>
    /// <param name="bound">Limit of cursor de reverse</param>
    private void UpdateCursor(Vector2 bound)
    {
        float deltaBound = (transform.position - (Vector3)bound).magnitude;
        float deltaTarget = (transform.position - (Vector3)target).magnitude;

        if (deltaBound < threshold)
        {
            cursor += orientation;
            ChangeOrientation();
            target = controlPoints[cursor + orientation];
        }
        else if (deltaTarget < threshold)
        {
            cursor += orientation;
            target = controlPoints[cursor + orientation];
        }
    }

    /// <summary>
    /// Inverts movement orientation
    /// </summary>
    private void ChangeOrientation()
    {
        orientation *= -1;
        if (limit == endingPoint)
            limit = startingPoint;
        else if (limit == startingPoint)
            limit = endingPoint;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, controlPoints[cursor]);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, target);
    }
}
