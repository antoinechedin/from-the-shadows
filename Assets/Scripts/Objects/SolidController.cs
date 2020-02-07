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
        moveVec = direction.normalized * currentSpeed * Time.deltaTime;
        transform.Translate(moveVec);
    }

    public void Approach(Vector2 bound)
    {
        // TODO : ease approach of target
        currentSpeed = Mathf.SmoothStep(maxSpeed, 0, currentSpeed);
        Move(bound);
    }

    public void Leave(Vector2 bound)
    {
        // TODO : ease bound leaving
        currentSpeed = Mathf.SmoothStep(0, maxSpeed, currentSpeed);
        Move(bound);
    }
}
