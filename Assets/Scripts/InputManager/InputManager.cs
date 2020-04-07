using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Actions of the player
/// </summary>
public enum InputAction
{
    MoveLeft,
    MoveRight,
    MoveUp,
    MoveDown,
    Jump,
    Attack,
    Interact,
    Switch,
    Pause,
    Restart,
    Select,
    Return
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

    public static Dictionary<InputAction, KeyCode>[] Player1 = new Dictionary<InputAction, KeyCode>[]
    {
        new Dictionary<InputAction, KeyCode>
        {
            { InputAction.MoveDown, KeyCode.S },
            { InputAction.MoveUp, KeyCode.Z },
            { InputAction.MoveLeft, KeyCode.Q },
            { InputAction.MoveRight, KeyCode.D },
            { InputAction.Jump, KeyCode.Space },
            { InputAction.Attack, KeyCode.A },
            { InputAction.Interact, KeyCode.E },
            { InputAction.Switch, KeyCode.Tab },
            { InputAction.Pause, KeyCode.Escape },
            { InputAction.Restart, KeyCode.Delete },
            { InputAction.Select, KeyCode.Return },
            { InputAction.Return, KeyCode.Escape }
        },
        new Dictionary<InputAction, KeyCode>
        {
            { InputAction.Jump, KeyCode.Joystick1Button0 },
            { InputAction.Attack, KeyCode.Joystick1Button1 },
            { InputAction.Interact, KeyCode.Joystick1Button2 },
            { InputAction.Switch, KeyCode.Joystick1Button4 },
            { InputAction.Select, KeyCode.Joystick1Button0 },
            { InputAction.Return, KeyCode.Joystick1Button1 },
            { InputAction.Pause, KeyCode.Joystick1Button7 },
            { InputAction.Restart, KeyCode.Joystick1Button6 }
        }
    };

    public static Dictionary<InputAction, KeyCode>[] Player2 = new Dictionary<InputAction, KeyCode>[]
    {
        new Dictionary<InputAction, KeyCode>
        {
            { InputAction.MoveDown, KeyCode.DownArrow },
            { InputAction.MoveUp, KeyCode.UpArrow },
            { InputAction.MoveLeft, KeyCode.LeftArrow },
            { InputAction.MoveRight, KeyCode.RightArrow },
            { InputAction.Switch, KeyCode.Tab },
            { InputAction.Pause, KeyCode.Escape },
            { InputAction.Restart, KeyCode.Delete },
            { InputAction.Select, KeyCode.Return },
            { InputAction.Return, KeyCode.Escape },
            { InputAction.Jump, KeyCode.Keypad0 },
            { InputAction.Attack, KeyCode.Keypad8 },
            { InputAction.Interact, KeyCode.Keypad4 }
        },
        new Dictionary<InputAction, KeyCode>
        {
            { InputAction.Jump, KeyCode.Joystick2Button0 },
            { InputAction.Attack, KeyCode.Joystick2Button1 },
            { InputAction.Interact, KeyCode.Joystick2Button2 },
            { InputAction.Switch, KeyCode.Joystick1Button4 },
            { InputAction.Select, KeyCode.Joystick2Button0 },
            { InputAction.Return, KeyCode.Joystick2Button1 },
            { InputAction.Pause, KeyCode.Joystick2Button7 },
            { InputAction.Restart, KeyCode.Joystick2Button6 }
        }
    };

    private static bool GetKey(int id, InputAction action, InputDevice device)
    {
        Dictionary<InputAction, KeyCode>[] Player = WhichPlayer(id);

        return Input.GetKey(Player[(int)device][action]);
    }

    private static Dictionary<InputAction, KeyCode>[] WhichPlayer(int id)
    {
        if (id != 1 && id != 2)
        {
            Debug.LogError("Player Selection : Error ID Player, player 1 selected");
            return null;
        }

        if (id == 1)
            return Player1;

        return Player2;
    }

    /// <summary>
    /// Get horizontal axis.
    /// </summary>
    /// <param name="id">ID of the player</param>
    /// <returns>float X Axis</returns>
    public static float GetHorizontalAxis(int id)
    {
        float xAxis = 0;

        if (GetKey(id, InputAction.MoveRight, (int)InputDevice.Keyboard))
            xAxis += 1;
        if (GetKey(id, InputAction.MoveLeft, (int)InputDevice.Keyboard))
            xAxis -= 1;

        // Temporary (Controller)
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal_" + id)) > 0)
            xAxis = Input.GetAxisRaw("Horizontal_" + id);

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

        if (GetKey(id, InputAction.MoveUp, (int)InputDevice.Keyboard))
            yAxis += 1;
        if (GetKey(id, InputAction.MoveDown, (int)InputDevice.Keyboard))
            yAxis -= 1;

        // Temporary (Controller)
        if (Mathf.Abs(Input.GetAxisRaw("Vertical_" + id)) > 0)
            yAxis = Input.GetAxisRaw("Vertical_" + id);

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
            Dictionary<InputAction, KeyCode>[] Player1;
            Dictionary<InputAction, KeyCode>[] Player2;
            Player1 = WhichPlayer(1);
            Player2 = WhichPlayer(2);

            IsPressed = Input.GetKeyDown(Player1[(int)InputDevice.Keyboard][action]);
            IsPressed = IsPressed || Input.GetKeyDown(Player1[(int)InputDevice.Controller][action]);
            IsPressed = IsPressed || Input.GetKeyDown(Player2[(int)InputDevice.Keyboard][action]);
            IsPressed = IsPressed || Input.GetKeyDown(Player2[(int)InputDevice.Controller][action]);
        }
        else
        {
            Dictionary<InputAction, KeyCode>[] Player;
            Player = WhichPlayer(id);

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
        Dictionary<InputAction, KeyCode>[] Player;

        Player = WhichPlayer(id);

        IsReleased = Input.GetKeyUp(Player[(int)InputDevice.Keyboard][action]);

        IsReleased = IsReleased || Input.GetKeyUp(Player[(int)InputDevice.Controller][action]);

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
        Dictionary<InputAction, KeyCode>[] Player;

        Player = WhichPlayer(id);

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
