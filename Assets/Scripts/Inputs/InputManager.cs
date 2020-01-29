using System;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class InputManager : MonoBehaviour, IPlayerActions
{
    public InputMaster inputs;

    private void Awake()
    {
        inputs.Player.Enable();
        inputs.Player.SetCallbacks(this);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        throw new NotImplementedException();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnAccept(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnRefuse(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void OnDisable()
    {
        inputs.Player.Disable();
    }

}
