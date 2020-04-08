using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomBaseInput : BaseInput
{
    public override float GetAxisRaw(string axisName)
    {
        if (axisName == "Horizontal")
        {
            return InputManager.GetHorizontalAxis(0);
        }
        else if (axisName == "Vertical")
        {
            return InputManager.GetVerticalAxis(0);
        }
        return 0f;
    }

    public override bool GetButtonDown(string buttonName)
    {
        if (buttonName == "Submit")
        {
            return InputManager.GetActionPressed(0, InputAction.Jump)
                || Input.GetKeyDown(KeyCode.Return);
        }
        else if (buttonName == "Cancel")
        {
            return InputManager.GetActionPressed(0, InputAction.Return)
                || Input.GetKeyDown(KeyCode.Escape)
                || Input.GetKeyDown(KeyCode.Backspace);
        }
        return false;
    }

    public override bool GetMouseButtonDown(int button)
    {
        return false;
    }

    public override bool GetMouseButtonUp(int button)
    {
        return false;
    }

    public override bool GetMouseButton(int button)
    {
        return false;
    }

}
