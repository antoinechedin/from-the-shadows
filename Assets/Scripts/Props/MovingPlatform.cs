using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SolidController))]
public class MovingPlatform : MonoBehaviour, IResetable
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
        
        Reset();
    }
    
    private void FixedUpdate()
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
    /// Easing movement towards bounds
    /// </summary>
    /// <param name="bounds">target boundary</param>
    private void ApproachBound(Vector2 bounds)
    {
        solidController.Approach(bounds, controlPoints[cursor]);
    }

    /// <summary>
    /// Easing movement leaving bounds
    /// </summary>
    /// <param name="bounds"></param>
    private void LeaveBound(Vector2 bounds)
    {
        solidController.Leave(bounds, target, controlPoints[cursor]);
    }

    /// <summary>
    /// Moves back and forth following control points array
    /// </summary>
    private void FollowTrajectoryBackAndForth()
    {
        if (IsTargetBounds())
            ApproachBound(target);
        else if (IsCursorBounds())
            LeaveBound(target);
        else
            MoveTowardsTarget(target);
        
        UpdateCursor(limit);
    }

    /// <summary>
    /// Returns if current control point is a bounds
    /// </summary>
    private bool IsCursorBounds()
    {
        return controlPoints[cursor].Equals(controlPoints[0]) || 
            controlPoints[cursor].Equals(controlPoints[controlPoints.Count - 1]);
    }

    /// <summary>
    /// Returns if current target is a bounds
    /// </summary>
    private bool IsTargetBounds()
    {
        return target.Equals(controlPoints[0]) || 
            target.Equals(controlPoints[controlPoints.Count - 1]);
    }

    /// <summary>
    /// Updates cursor position in control points array
    /// </summary>
    /// <param name="bounds">Limit of cursor de reverse</param>
    private void UpdateCursor(Vector2 bounds)
    {
        float deltaBounds = (transform.position - (Vector3)bounds).magnitude;
        float deltaTarget = (transform.position - (Vector3)target).magnitude;

        if (deltaBounds < threshold)
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
    /// Inverts movement orientation in control points array
    /// </summary>
    private void ChangeOrientation()
    {
        orientation *= -1;
        if (limit == endingPoint)
            limit = startingPoint;
        else if (limit == startingPoint)
            limit = endingPoint;
    }

    /// <summary>
    /// Resets platform to initial position
    /// </summary>
    public void Reset()
    {
        if (controlPoints.Count < 2)
        {
            Debug.LogError("MovingPlatform : Control points count must be over 1");
        }
        transform.position = startingPoint;
        orientation = 1;
        cursor = 0;
        target = controlPoints[cursor + orientation];
        limit = endingPoint;
    }

    #region Editor functions
    
    /// <summary>
    /// Create GameObjects in edit mode
    /// </summary>
    public void CreateControlPointsGameObjects()
    {
        startingPoint = controlPoints[0];
        endingPoint = controlPoints[controlPoints.Count - 1];

        GameObject listCP = FindOrCreateEmptyChild("_Control Points", transform);
        GameObject sp = FindOrCreateEmptyChild("Starting Point", transform);
        GameObject ep = FindOrCreateEmptyChild("Ending Point", transform);
               
        sp.transform.position = startingPoint;
        ep.transform.position = endingPoint;

        for (int i = 1; i < controlPoints.Count - 1; i++)
        {
            GameObject cp = FindOrCreateEmptyChild("CP" + i, listCP.transform);
            cp.transform.position = controlPoints[i];
        }
    }

    /// <summary>
    /// Updates control points with gameobjects position
    /// </summary>
    public void UpdateControlPointsArray()
    {
        Transform listCP = transform.Find("_Control Points");
    }

    /// <summary>
    /// Looks for child GameObject with name, returns existing if found, new if not
    /// </summary>
    /// <param name="name">GameObject's name to look for</param>
    /// <param name="parent">Parent of the searched GameObject</param>
    /// <returns></returns>
    private GameObject FindOrCreateEmptyChild(string name, Transform parent)
    {
        Transform tmp = parent.Find(name);
        GameObject go;
        if (tmp == null)
        {
            go = new GameObject(name);
            go.transform.parent = parent;
        }
        else
            go = tmp.gameObject;

        return go;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, controlPoints[cursor]);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, target);
    }
}
