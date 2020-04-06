using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remapping : MonoBehaviour
{
    [Range(1, 2)]
    public int id = 1;
    public InputAction action;
    public InputDevice device;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        System.Array values = System.Enum.GetValues(typeof(KeyCode));
        KeyCode precedentCode = InputManager.GetActionKeyCode(id, action, device);
        foreach (KeyCode code in values)
        {
            if (Input.GetKeyDown(code)) 
            { 
                if (InputManager.GetActionKeyCode(id, InputAction.Select, device) == code)
                    InputManager.RemapAction(id, action, device, precedentCode);
                else
                    precedentCode = code;
            }
        }

        //key = (KeyCode)System.Enum.Parse(typeof(KeyCode), "Wathever");
    }
}
