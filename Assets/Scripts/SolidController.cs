using UnityEngine;

public class SolidController : MonoBehaviour
{
    public float speed;

    /// <summary>
    /// Moves the object in a given direction
    /// </summary>
    /// <param name="direction">normalized direction of the movement</param>
    public void Move(Vector2 direction)
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }
}
