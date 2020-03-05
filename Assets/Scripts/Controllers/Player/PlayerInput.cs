using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Range(1, 2)]
    public int id = 1;
    public bool doubleJump;
    public bool attack;

    public bool debugControl;
    public bool active = true;

    public Vector2 moveAxis;
    [HideInInspector] public float xMoveAxisSign = 1f;
    public bool pressedJump;
    public bool releasedJump;
    public bool pressedAttack;

    public bool pressRight, pressLeft, pressUp, pressDown;

    private void Update()
    {
        if (!active)
        {
            moveAxis = Vector2.zero;
            pressedJump = false;
            releasedJump = false;
            attack = false;
        }
        else if (debugControl)
        {
            moveAxis = Vector2.zero;
            if (pressRight) moveAxis.x += 1;
            if (pressLeft) moveAxis.x -= 1;
            if (pressUp) moveAxis.y += 1;
            if (pressDown) moveAxis.y -= 1;
        }
        else
        {
            moveAxis.x = Input.GetAxisRaw("Horizontal_" + id);
            moveAxis.y = Input.GetAxisRaw("Vertical_" + id);
            pressedJump = Input.GetButtonDown("A_" + id);
            releasedJump = Input.GetButtonUp("A_" + id);
            if (attack) pressedAttack = Input.GetButtonDown("Y_" + id);
            if (moveAxis.magnitude > 1) moveAxis.Normalize();
        }

        if (moveAxis.x > 0) xMoveAxisSign = 1;
        else if (moveAxis.x < 0) xMoveAxisSign = -1;
    }
}