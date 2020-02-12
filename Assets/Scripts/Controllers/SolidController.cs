using System;
using System.Collections;
using UnityEngine;

public class SolidController : MonoBehaviour
{
    public float speed;
    public float maxSpeed;

    private void Awake()
    {

    }

    /// <summary>
    /// Moves the object in a given direction
    /// </summary>
    /// <param name="direction">normalized direction of the movement</param>
    public void Move(Vector2 direction)
    {
        Vector3 moveVec = direction.normalized * speed * Time.fixedDeltaTime;
        Debug.Log(moveVec);
        transform.Translate(moveVec);
    }

    public void Approach(Vector2 bounds, Vector2 origin)
    {
        float t = (transform.position - (Vector3)origin).magnitude / (bounds - origin).magnitude;
        speed = Mathf.Lerp(maxSpeed, 1.5f, t);
        Move((Vector3)bounds - transform.position);
    }

    public void Leave(Vector2 bounds, Vector2 target, Vector2 origin)
    {
        float t = (transform.position - (Vector3)origin).magnitude / (bounds - origin).magnitude;        
        speed = Mathf.Lerp(speed, maxSpeed, t);
        Move((Vector3)target - transform.position);
    }
}
