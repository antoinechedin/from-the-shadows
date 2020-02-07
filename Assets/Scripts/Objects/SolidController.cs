using System;
using UnityEngine;

public class SolidController : MonoBehaviour
{
    private float currentSpeed;
    public float maxSpeed;
    private Vector3 moveVec;

    private void Awake()
    {
        currentSpeed = maxSpeed;
    }

    /// <summary>
    /// Moves the object in a given direction
    /// </summary>
    /// <param name="direction">normalized direction of the movement</param>
    public void Move(Vector2 direction)
    {
        moveVec = direction.normalized * currentSpeed * Time.fixedDeltaTime;
        transform.Translate(moveVec);
    }

    public void Approach(Vector2 bounds)
    {
        // TODO : ease approach of target
        currentSpeed = Mathf.SmoothStep(maxSpeed, 0, currentSpeed);
        Move(bounds);
    }

    public void Leave(Vector2 bounds)
    {
        // TODO : ease bound leaving
        currentSpeed = Mathf.SmoothStep(0, maxSpeed, currentSpeed);
        Move(bounds);
    }
}
