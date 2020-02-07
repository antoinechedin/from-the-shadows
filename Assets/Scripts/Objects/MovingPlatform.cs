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

    private void ApproachBound(Vector2 bound)
    {
        solidController.Approach((Vector3)bound - transform.position);
    }

    private void LeaveBound(Vector2 bound)
    {
        solidController.Leave((Vector3)bound - transform.position);
    }

    /// <summary>
    /// Moves back and forth following control points array
    /// </summary>
    private void FollowTrajectoryBackAndForth()
    {
        if (IsTargetBound() && ((Vector3)target - transform.position).magnitude < 1.5f)
            ApproachBound(target);
        else if (IsCursorBound() && ((Vector3)target - transform.position).magnitude < 1.5f)
            LeaveBound(target);
        else
            MoveTowardsTarget(target);
        
        UpdateCursor(limit);
    }

    private bool IsCursorBound()
    {
        return controlPoints[cursor].Equals(controlPoints[0]) || 
            controlPoints[cursor].Equals(controlPoints[controlPoints.Count - 1]);
    }

    private bool IsTargetBound()
    {
        return target.Equals(controlPoints[0]) || 
            target.Equals(controlPoints[controlPoints.Count - 1]);
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
