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

    #region In game method

    private void Awake()
    {
        solidController = GetComponent<SolidController>();

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
        startingPoint = controlPoints[0];
        endingPoint = controlPoints[controlPoints.Count - 1];
        transform.position = startingPoint;
        orientation = 1;
        cursor = 0;
        target = controlPoints[cursor + orientation];
        limit = endingPoint;
    }

    #endregion

    #region Editor methods

    [Header("Level building")]
    public GameObject pathNodePrefab;

    /// <summary>
    /// Create GameObjects in edit mode
    /// </summary>
    public void CreateControlPointsGameObjects()
    {
        GameObject sp = FetchChild("Starting Point", transform.parent);
        if (sp == null)
            CreateChild("Starting Point", transform.parent, pathNodePrefab);

        GameObject ep = FetchChild("Ending Point", transform.parent);
        if (ep == null)
            CreateChild("Ending Point", transform.parent, pathNodePrefab);

        if (controlPoints.Count > 2)
        {
            GameObject listCP = FetchChild("_Control Points", transform.parent);
            if (listCP == null)
                listCP = CreateChild("_Control Points", transform.parent);
            for (int i = 1; i < controlPoints.Count - 1; i++)
            {
                GameObject cp = FetchChild("CP" + i, listCP.transform);
                if (cp == null)
                    CreateChild("CP" + i, listCP.transform, pathNodePrefab);
            }
        }
    }

    /// <summary>
    /// Updates control points with gameobjects position
    /// </summary>
    public void UpdateControlPointsArray()
    {
        GameObject listCP = FetchChild("_Control Points", transform.parent);
        GameObject sp = FetchChild("Starting Point", transform.parent);
        GameObject ep = FetchChild("Ending Point", transform.parent);

        if (listCP != null)
        {
            List<GameObject> toDestroy = new List<GameObject>();
            for (int i = 1; i <= listCP.transform.childCount; i++)
            {
                GameObject cp = FetchChild("CP" + i, listCP.transform);
                if (i < controlPoints.Count - 1)
                {
                    controlPoints[i] = cp.transform.position;
                }
                else
                {
                    toDestroy.Add(cp);
                }
            }
            foreach (var go in toDestroy)
                DestroyImmediate(go);

            if (listCP.transform.childCount == 0)
                DestroyImmediate(listCP);
        }

        controlPoints[0] = sp.transform.position;
        controlPoints[controlPoints.Count - 1] = ep.transform.position;

        Reset();
    }

    /// <summary>
    /// Fetches child GameObject with name
    /// </summary>
    /// <param name="name">GameObject's name to look for</param>
    /// <param name="parent">Parent's transform</param>
    /// <returns></returns>
    private GameObject FetchChild(string name, Transform parent)
    {
        Transform tmp = parent.Find(name);
        if (tmp == null)
        {
            return null;
        }
        else
            return tmp.gameObject;
    }

    /// <summary>
    /// Creates a child for parent with name
    /// </summary>
    /// <param name="name">Name of the new GameObject</param>
    /// <param name="parent">Parent's transform</param>
    /// <returns></returns>
    private GameObject CreateChild(string name, Transform parent, GameObject prefab = null)
    {
        GameObject go;
        if (prefab == null)
        {
            go = new GameObject(name);
            go.transform.parent = parent;
        }
        else
        {
            go = Instantiate(prefab);
            go.transform.parent = parent;
            go.name = name;
        }

        return go;
    }

    #endregion
}
