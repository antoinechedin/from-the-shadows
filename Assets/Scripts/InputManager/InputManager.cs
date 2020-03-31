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
    Attack
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
    

    //public static Dictionary<InputAction, KeyCode> Player1_Keyboard = new Dictionary<InputAction, KeyCode>();
    //public static Dictionary<InputAction, KeyCode> Player1_Controller = new Dictionary<InputAction, KeyCode>();

    public static Dictionary<InputAction, KeyCode>[] Player1 = new Dictionary<InputAction, KeyCode>[]
    {
        new Dictionary<InputAction, KeyCode>(),
        new Dictionary<InputAction, KeyCode>()
    };

    public static Dictionary<InputAction, KeyCode>[] Player2 = new Dictionary<InputAction, KeyCode>[]
    {
        new Dictionary<InputAction, KeyCode>(),
        new Dictionary<InputAction, KeyCode>()
    };

    //public static Dictionary<InputAction, KeyCode> Player2_Keyboard = new Dictionary<InputAction, KeyCode>();
    //public static Dictionary<InputAction, KeyCode> Player2_Controller = new Dictionary<InputAction, KeyCode>();

    /// <summary>
    /// Initialize inputs of players.
    /// </summary>
    public static void Init()
    {

        Player1[(int)InputDevice.Keyboard].Clear();
        Player1[(int)InputDevice.Controller].Clear();
        Player2[(int)InputDevice.Keyboard].Clear();
        Player2[(int)InputDevice.Controller].Clear();

        Player1[(int)InputDevice.Keyboard].Add(InputAction.MoveDown, KeyCode.S);
        Player1[(int)InputDevice.Keyboard].Add(InputAction.MoveUp, KeyCode.Z);
        Player1[(int)InputDevice.Keyboard].Add(InputAction.MoveLeft, KeyCode.Q);
        Player1[(int)InputDevice.Keyboard].Add(InputAction.MoveRight, KeyCode.D);
        Player1[(int)InputDevice.Keyboard].Add(InputAction.Jump, KeyCode.Space);
        Player1[(int)InputDevice.Keyboard].Add(InputAction.Attack, KeyCode.F);

        Player1[(int)InputDevice.Controller].Add(InputAction.Jump, KeyCode.Joystick1Button0);
        Player1[(int)InputDevice.Controller].Add(InputAction.Attack, KeyCode.Joystick1Button1);

        Player2[(int)InputDevice.Keyboard].Add(InputAction.MoveDown, KeyCode.DownArrow);
        Player2[(int)InputDevice.Keyboard].Add(InputAction.MoveUp, KeyCode.UpArrow);
        Player2[(int)InputDevice.Keyboard].Add(InputAction.MoveLeft, KeyCode.LeftArrow);
        Player2[(int)InputDevice.Keyboard].Add(InputAction.MoveRight, KeyCode.RightArrow);
        Player2[(int)InputDevice.Keyboard].Add(InputAction.Jump, KeyCode.Return);
        Player2[(int)InputDevice.Keyboard].Add(InputAction.Attack, KeyCode.RightShift);

        Player2[(int)InputDevice.Controller].Add(InputAction.Jump, KeyCode.Joystick2Button0);
        Player2[(int)InputDevice.Controller].Add(InputAction.Attack, KeyCode.Joystick2Button1);
    }

    private static bool GetKey(int id, InputAction action, InputDevice device)
    {
        Dictionary<InputAction, KeyCode>[] Player = WhichPlayer(id);

        return Input.GetKey(Player[(int) device][action]);
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
        Dictionary<InputAction, KeyCode>[] Player;

        Player = WhichPlayer(id);

        if (GetKey(id, InputAction.MoveRight, (int)InputDevice.Keyboard))
            xAxis = 1;
        if (GetKey(id, InputAction.MoveLeft, (int)InputDevice.Keyboard))
            xAxis = -1;

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
        Dictionary<InputAction, KeyCode>[] Player;

        Player = WhichPlayer(id);

        if (GetKey(id, InputAction.MoveUp, (int)InputDevice.Keyboard))
            yAxis = 1;
        if (GetKey(id, InputAction.MoveDown, (int)InputDevice.Keyboard))
            yAxis = -1;

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
        Dictionary<InputAction, KeyCode>[] Player;

        Player = WhichPlayer(id);

        IsPressed = Input.GetKeyDown(Player[(int) InputDevice.Keyboard][action]);

        IsPressed = IsPressed || Input.GetKeyDown(Player[(int)InputDevice.Controller][action]);
        
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

        // Temporarly (Controller)
        if (action == InputAction.Jump)
            IsReleased = (IsReleased || Input.GetButtonUp("A_" + id));
        else if (action == InputAction.Attack)
            IsReleased = (IsReleased || Input.GetButtonUp("Y_" + id));

        //return IsReleased = (Input.GetKeyUp(Player[(int)InputDevice.Keyboard][action]) || Input.GetButtonUp("A_" + id));
        return IsReleased;
    }

    /// <summary>
    /// Display the key code for a precise action
    /// </summary>
    /// <param name="id">ID of the player</param>
    /// <param name="action">Action needed</param>
    /// <param name="device">Device used</param>
    /// <returns>KeyCode of the action needed in a specific device</returns>
    public static KeyCode DisplayKeyCode (int id, InputAction action, InputDevice device)
    {
        Dictionary<InputAction, KeyCode>[] Player;

        Player = WhichPlayer(id);

        return Player[(int)device][action];
    }

    /// <summary>
    /// Modify input of a specific action in a specific device
    /// </summary>
    /// <param name="id">Id of the player</param>
    /// <param name="action">Action wanted</param>
    /// <param name="device">Device used</param>
    /// <param name="inputKey">Input key</param>
    public static void SetKeyCode (int id, InputAction action, InputDevice device, KeyCode inputKey)
    {
        if (id == 1)
            Player1[(int)device][action] = inputKey;
        else if (id == 2)
            Player2[(int)device][action] = inputKey;
    }
}



/*
        update

        System.Array values = System.Enum.GetValues(typeof(KeyCode));
        foreach (KeyCode code in values)
        {
            if (Input.GetKeyDown(code)) { Debug.Log(System.Enum.GetName(typeof(KeyCode), code)); }
        }

 */
