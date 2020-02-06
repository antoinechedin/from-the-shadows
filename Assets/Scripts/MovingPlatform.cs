using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SolidController))]
public class MovingPlatform : MonoBehaviour
{
    private Vector2 startingPoint;
    public Vector2 target;
    
    public List<Vector2> controlPoints;
    private int cursor = 0;

    public float threshold;
    private int orientation = 1;
    private bool ignoreStartingPoint = true;


    private SolidController solidController;

    private void Awake()
    {
        solidController = GetComponent<SolidController>();
        startingPoint = transform.position;
    }

    private void Update()
    {
        FollowTrajectoryBackAndForth();
    }

    /// <summary>
    /// Moves back and forth from startingPoint to target
    /// </summary>
    private void MoveBackAndForth()
    {
        UpdateOrientation();
        solidController.Move((target - startingPoint) * orientation);
    }

    /// <summary>
    /// Moves following control points array
    /// </summary>
    private void FollowTrajectoryBackAndForth()
    {
        UpdateOrientation();
        solidController.Move(controlPoints[cursor + orientation] - controlPoints[cursor]);
        UpdateCursor();
    }


    /// <summary>
    /// Checks and changes orientation when approaching bounds
    /// </summary>
    private void UpdateOrientation()
    {
        float deltaStart = (transform.position - (Vector3)startingPoint).magnitude;
        float deltaTarget = (transform.position - (Vector3)target).magnitude;

        if (deltaStart < threshold && !ignoreStartingPoint)
        {
            ChangeOrientation();
        }
        else if (deltaTarget < threshold)
        {
            ChangeOrientation();
            ignoreStartingPoint = false;
        }
    }

    /// <summary>
    /// Updates cursor position when approaching bounds
    /// </summary>
    private void UpdateCursor()
    {
        float delta = (transform.position - (Vector3)controlPoints[cursor + orientation]).magnitude;

        if (delta < threshold)
            cursor += orientation;
    }

    /// <summary>
    /// Changes movement orientation
    /// </summary>
    private void ChangeOrientation()
    {
        orientation *= -1;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, startingPoint);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, target);
    }
}
