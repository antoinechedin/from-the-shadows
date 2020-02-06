using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SolidController))]
public class MovingPlatform : MonoBehaviour
{
    private Vector2 startingPoint;
    public Vector2 target;
    
    public List<Vector2> controlPoints;
    private int indexCursor = 1;

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
        MoveBackAndForth();
    }

    /// <summary>
    /// Moves back and forth from startingPoint to target
    /// </summary>
    private void MoveBackAndForth()
    {
        float deltaFrom = (transform.position - (Vector3)startingPoint).magnitude;
        float deltaTo = (transform.position - (Vector3)target).magnitude;

        if (deltaFrom < threshold && !ignoreStartingPoint)
        {
            ChangeOrientation();
        }
        else if (deltaTo < threshold)
        {
            ChangeOrientation();
            ignoreStartingPoint = false;
        }

        solidController.Move((target - startingPoint) * orientation);
    }

    /// <summary>
    /// Changes movement orientation;
    /// </summary>
    private void ChangeOrientation()
    {
        orientation *= -1;
    }

    /// <summary>
    /// Moves following control points array
    /// </summary>
    private void FollowTrajectory()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, startingPoint);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, target);
    }
}
