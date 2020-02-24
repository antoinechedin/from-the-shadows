using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Range(1, 2)]
    public int id = 1;
    public bool doubleJump;

    public bool debugControl;
    public bool active = true;
    
    public Vector2 moveAxis;
    public bool pressedJump;
    public bool releasedJump;

    public bool pressRight, pressLeft, pressUp, pressDown;

    private void Update()
    {
        if (!active)
        {
            moveAxis = Vector2.zero;
            pressedJump = false;
            releasedJump = false;
        }
        else if (!debugControl)
        {
            moveAxis.x = Input.GetAxisRaw("Horizontal_" + id);
            moveAxis.y = Input.GetAxisRaw("Vertical_" + id);
            pressedJump = Input.GetButtonDown("A_" + id);
            releasedJump = Input.GetButtonUp("A_" + id);
            if (moveAxis.magnitude > 1) moveAxis.Normalize();
        }
        else
        {
            moveAxis = Vector2.zero;
            if (pressRight) moveAxis.x += 1;
            if (pressLeft) moveAxis.x -= 1;
            if (pressUp) moveAxis.y += 1;
            if (pressDown) moveAxis.y -= 1;
        }
    }
}