using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : ActivatorListener
{
    public enum Direction { Up, Down, Left, Right};
    private bool open;
    private Vector3 stopPosition;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    public Direction direction;
    [Range(0,10)]
    public float speed;
    public float distance;

    public void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition;
        switch (direction)
        {
            case Direction.Up:
                stopPosition = startPosition + (Vector3.up * distance);
                break;
            case Direction.Down:
                stopPosition = startPosition - (Vector3.up * distance);
                break;
            case Direction.Left:
                stopPosition = startPosition + (Vector3.left * distance);
                break;
            case Direction.Right:
                stopPosition = startPosition - (Vector3.left * distance);
                break;
        }
    }

    public override void OnActivate()
    {
        open = true;
        targetPosition = stopPosition;
         
    }

    public override void OnDeactivate()
    {
        if (open)
        {
            open = false;
            targetPosition = startPosition;
        }
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * speed);
    }
}
