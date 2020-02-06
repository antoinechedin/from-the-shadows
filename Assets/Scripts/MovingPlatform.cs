using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SolidController))]
public class MovingPlatform : MonoBehaviour
{
    public Vector2 startingPoint;
    public Vector2 endingPoint;

    private int orientation = 1;

    private SolidController solidController;

    private void Start()
    {
        solidController = GetComponent<SolidController>();
    }

    private void Update()
    {
        float deltaStart = (transform.position - (Vector3)startingPoint).magnitude;
        float deltaEnd = (transform.position - (Vector3)endingPoint).magnitude;

        if (deltaStart < 0.1f || deltaEnd < 0.1f)
            orientation *= -1;

        solidController.Move((endingPoint - startingPoint) * orientation);
    }
}
