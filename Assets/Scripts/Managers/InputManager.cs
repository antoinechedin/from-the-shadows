using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Actions of the player
/// </summary>
public enum InputAction
{
    Up,
    Down,
    Left,
    Right,
    Jump,
    Interact,
    Switch,
    Attack,
    Pause,
    Select,
    Return,
    Restart,
}

/// <summary>
/// Devices used
/// </summary>
public enum InputDevice
{
    Keyboard = 0,
    Controller = 1
}

public class InputManager
{
    // Verify if the controller is a Switch Pro or an Xbox/PS4 controller
    public static bool isProController = false;
    public static bool isKeyPad = false;

    public static Dictionary<InputAction, KeyCode>[] Player1 = GetKeyMappingFromPlayerPrefs(1);

    public static Dictionary<InputAction, KeyCode>[] Player2 = GetKeyMappingFromPlayerPrefs(2);

    private static Dictionary<InputAction, KeyCode>[] GetKeyMappingFromPlayerPrefs(int id)
    {
        if (id != 1 && id != 2)
        {
            Debug.LogError("InputManager.GetKeyMappingFromPlayerPrefs: id isn't 1 or 2");
            return null;
        }

        Dictionary<InputAction, KeyCode>[] keymap = new Dictionary<InputAction, KeyCode>[]
        {
            new Dictionary<InputAction, KeyCode>
            {
                {
                    InputAction.Up,
                    (KeyCode)PlayerPrefs.GetInt("P" + id + "_Up", (int)(id == 1 ? KeyCode.Z : KeyCode.UpArrow))
                },
                {
                    InputAction.Down,
                    (KeyCode)PlayerPrefs.GetInt("P" + id + "_Down", (int)(id == 1 ? KeyCode.S : KeyCode.DownArrow))
                },
                {
                    InputAction.Left,
                    (KeyCode)PlayerPrefs.GetInt("P" + id + "_Left", (int)(id == 1 ? KeyCode.Q : KeyCode.LeftArrow))
                },
                {
                    InputAction.Right,
                    (KeyCode)PlayerPrefs.GetInt("P" + id + "_Right", (int)(id == 1 ? KeyCode.D : KeyCode.RightArrow))
                },
                {
                    InputAction.Jump,
                    (KeyCode)PlayerPrefs.GetInt("P" + id + "_Jump", (int)(id == 1 ? KeyCode.Space : KeyCode.RightControl))
                },
                {
                    InputAction.Interact,
                    (KeyCode)PlayerPrefs.GetInt("P" + id + "_Interact", (int)(id == 1 ? KeyCode.E : KeyCode.RightShift))
                },
                {
                    InputAction.Switch,
                    (KeyCode)PlayerPrefs.GetInt("P" + id + "_Switch", (int)(id == 1 ? KeyCode.LeftShift : KeyCode.Keypad0))
                },
                {
                    InputAction.Attack,
                    (KeyCode)PlayerPrefs.GetInt("P" + id + "_Attack", (int)(id == 1 ? KeyCode.F : KeyCode.Keypad1))
                },
                {
                    InputAction.Pause,
                    KeyCode.Escape
                },
                {
                    InputAction.Select,
                    KeyCode.Return
                },
                {
                    InputAction.Return,
                    KeyCode.Escape
                },
                {
                    InputAction.Restart,
                    KeyCode.Delete
                },
            },
            new Dictionary<InputAction, KeyCode>
            {
                { InputAction.Jump, id == 1 ? KeyCode.Joystick1Button0 : KeyCode.Joystick2Button0 },
                { InputAction.Attack, id == 1 ? KeyCode.Joystick1Button1 : KeyCode.Joystick2Button1 },
                { InputAction.Interact, id == 1 ? KeyCode.Joystick1Button2 : KeyCode.Joystick2Button2 },
                { InputAction.Switch, id == 1 ? KeyCode.Joystick1Button4 : KeyCode.Joystick2Button4 },
                { InputAction.Select, id == 1 ? KeyCode.Joystick1Button0 : KeyCode.Joystick2Button0 },
                { InputAction.Return, id == 1 ? KeyCode.Joystick1Button1 : KeyCode.Joystick2Button1 },
                { InputAction.Pause, id == 1 ? KeyCode.Joystick1Button7 : KeyCode.Joystick2Button7 },
                { InputAction.Restart, id == 1 ? KeyCode.Joystick1Button6 : KeyCode.Joystick2Button6 }
                
            }
        };
        return keymap;
    }

    public static void UpdateKeyMapping()
    {
        InputManager.Player1 = GetKeyMappingFromPlayerPrefs(1);
        InputManager.Player2 = GetKeyMappingFromPlayerPrefs(2);
    }

    /// <summary>
    /// Get horizontal axis.
    /// </summary>
    /// <param name="id">ID of the player</param>
    /// <returns>float X Axis</returns>
    public static float GetHorizontalAxis(int id)
    {
        float xAxis = 0;

        if (id == 0)
        {
            if (
                Input.GetKey(InputManager.Player1[0][InputAction.Right])
                || Input.GetKey(InputManager.Player2[0][InputAction.Right])
            ) xAxis += 1;

            if (
                Input.GetKey(InputManager.Player1[0][InputAction.Left])
                || Input.GetKey(InputManager.Player2[0][InputAction.Left])
            ) xAxis -= 1;
        }
        else
        {
            Dictionary<InputAction, KeyCode>[] Player = id == 1 ? InputManager.Player1 : InputManager.Player2;
            if (Input.GetKey(Player[0][InputAction.Right]))
                xAxis += 1;
            if (Input.GetKey(Player[0][InputAction.Left]))
                xAxis -= 1;
        }

        // Temporary (Controller)
        string idStr = id == 0 ? "G" : id.ToString();
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal_" + idStr)) > 0)
        {
            xAxis = Input.GetAxisRaw("Horizontal_" + idStr);
        }
        return xAxis;
    }

    /// <summary>
    /// Get vertical axis.
    /// </summary>
    /// <param name="id">ID of the player</param>
    /// <returns>float Y Axis</returns>
    public static float GetVerticalAxis(int id)
    {
        float yAxis = 0;

        if (id == 0)
        {
            if (
                Input.GetKey(InputManager.Player1[0][InputAction.Up])
                || Input.GetKey(InputManager.Player2[0][InputAction.Up])
            ) yAxis += 1;

            if (
                Input.GetKey(InputManager.Player1[0][InputAction.Down])
                || Input.GetKey(InputManager.Player2[0][InputAction.Down])
            ) yAxis -= 1;
        }
        else
        {
            Dictionary<InputAction, KeyCode>[] Player = id == 1 ? InputManager.Player1 : InputManager.Player2;
            if (Input.GetKey(Player[0][InputAction.Up]))
                yAxis += 1;
            if (Input.GetKey(Player[0][InputAction.Down]))
                yAxis -= 1;
        }

        // Temporary (Controller)
        string idStr = id == 0 ? "G" : id.ToString();
        if (Mathf.Abs(Input.GetAxisRaw("Vertical_" + idStr)) > 0)
        {
            yAxis = Input.GetAxisRaw("Vertical_" + idStr);
        }
        return yAxis;
    }

    /// <summary>
    /// If the action is pressed by the player.
    /// </summary>
    /// <param name="id">ID of the player</param>
    /// <param name="action">Action wanted</param>
    /// <returns>bool if the action is pressed</returns>
    public static bool GetActionPressed(int id, InputAction action)
    {
        bool IsPressed = false;

        if (id == 0)
        {
            IsPressed = Input.GetKeyDown(Player1[(int)InputDevice.Keyboard][action]);
            IsPressed = IsPressed || Input.GetKeyDown(Player1[(int)InputDevice.Controller][action]);
            IsPressed = IsPressed || Input.GetKeyDown(Player2[(int)InputDevice.Keyboard][action]);
            IsPressed = IsPressed || Input.GetKeyDown(Player2[(int)InputDevice.Controller][action]);
        }
        else
        {
            Dictionary<InputAction, KeyCode>[] Player = id == 1 ? InputManager.Player1 : InputManager.Player2;
            IsPressed = Input.GetKeyDown(Player[(int)InputDevice.Keyboard][action]);

            IsPressed = IsPressed || Input.GetKeyDown(Player[(int)InputDevice.Controller][action]);
        }
        return IsPressed;
    }

    /// <summary>
    /// If the action is released by the player.
    /// </summary>
    /// <param name="id">ID of the player</param>
    /// <param name="action">Action wanted</param>
    /// <returns>bool if the action is released</returns>
    public static bool GetActionReleased(int id, InputAction action)
    {
        bool IsReleased = false;

        if (id == 0)
        {
            IsReleased = Input.GetKeyUp(Player1[(int)InputDevice.Keyboard][action]);
            IsReleased = IsReleased || Input.GetKeyUp(Player1[(int)InputDevice.Controller][action]);
            IsReleased = IsReleased || Input.GetKeyUp(Player2[(int)InputDevice.Keyboard][action]);
            IsReleased = IsReleased || Input.GetKeyUp(Player2[(int)InputDevice.Controller][action]);
        }
        else
        {
            Dictionary<InputAction, KeyCode>[] Player = id == 1 ? InputManager.Player1 : InputManager.Player2;
            IsReleased = Input.GetKeyUp(Player[(int)InputDevice.Keyboard][action]);
            IsReleased = IsReleased || Input.GetKeyUp(Player[(int)InputDevice.Controller][action]);
        }

        //return IsReleased = (Input.GetKeyUp(Player[(int)InputDevice.Keyboard][action]) || Input.GetButtonUp("A_" + id));
        return IsReleased;
    }

    /// <summary>
    /// Get the key code for a precise action
    /// </summary>
    /// <param name="id">ID of the player</param>
    /// <param name="action">Action needed</param>
    /// <param name="device">Device used</param>
    /// <returns>KeyCode of the action needed in a specific device</returns>
    public static KeyCode GetActionKeyCode(int id, InputAction action, InputDevice device)
    {
        Dictionary<InputAction, KeyCode>[] Player = id == 1 ? InputManager.Player1 : InputManager.Player2;
        return Player[(int)device][action];
    }

    /// <summary>
    /// Modify and remap input of a specific action in a specific device
    /// </summary>
    /// <param name="id">Id of the player</param>
    /// <param name="action">Action wanted</param>
    /// <param name="device">Device used</param>
    /// <param name="inputKey">Input key</param>
    public static void RemapAction(int id, InputAction action, InputDevice device, KeyCode inputKey)
    {
        if (id == 1)
            Player1[(int)device][action] = inputKey;
        else if (id == 2)
            Player2[(int)device][action] = inputKey;
    }
}

/*
   System.Array values = System.Enum.GetValues(typeof(KeyCode));
        foreach (KeyCode code in values)
        {
            if (Input.GetKeyDown(code)) { Debug.Log(System.Enum.GetName(typeof(KeyCode), code)); }
        }
*/
