using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool isProController = false;
    public bool isKeyPad = true;

    [Range(1, 2)]
    public int id = 1;
    public bool doubleJump;
    public bool attack;
    public Collider2D attackCollider;
    public LayerMask attackMask;

    public bool debugControl;
    public bool active = true;
    public bool noJump = false;

    public Vector2 moveAxis;
    [HideInInspector] public float xMoveAxisSign = 1f;
    public bool pressedJump;
    public bool releasedJump;
    public bool pressedAttack;

    public bool pressRight, pressLeft, pressUp, pressDown;

    private void Awake()
    {
        if (attack && attackCollider == null)
        {
            attack = false;
            Debug.LogWarning("WARN PlayerInput.Awake: Player " + id + " can attack but don't have an attack collider."
                            + " Attack is disable.");
        }

        //Debug.Log(Input.GetJoystickNames()[0]);
    }

    private void Update()
    {
        if (!active)
        {
            moveAxis = Vector2.zero;
            pressedJump = false;
            releasedJump = false;
            pressedAttack = false;
        }
        else if (debugControl)
        {
            moveAxis = Vector2.zero;
            if (pressRight) moveAxis.x += 1;
            if (pressLeft) moveAxis.x -= 1;
            if (pressUp) moveAxis.y += 1;
            if (pressDown) moveAxis.y -= 1;
        }
        else if(Time.timeScale > 0)
        {
            /*moveAxis.x = Input.GetAxisRaw("Horizontal_" + id);
            moveAxis.y = Input.GetAxisRaw("Vertical_" + id);
            pressedJump = Input.GetButtonDown("A_" + id);
            releasedJump = Input.GetButtonUp("A_" + id);
            if (attack) pressedAttack = Input.GetButtonDown("Y_" + id);*/

            moveAxis.x = InputManager.GetHorizontalAxis(id);
            moveAxis.y = InputManager.GetVerticalAxis(id);
            if (!noJump)
            {
                pressedJump = InputManager.GetActionPressed(id, InputAction.Jump);
            }
            releasedJump = InputManager.GetActionReleased(id, InputAction.Jump);

            if (attack) pressedAttack = InputManager.GetActionPressed(id, InputAction.Attack);
            if (moveAxis.magnitude > 1) moveAxis.Normalize();
        }

        if (moveAxis.x > 0) xMoveAxisSign = 1;
        else if (moveAxis.x < 0) xMoveAxisSign = -1;
    }
}