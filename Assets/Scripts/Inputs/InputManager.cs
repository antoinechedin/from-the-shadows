using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void OnDeviceLost()
    {
        Debug.Log("Device Lost");
    }

    private void OnDeviceRegained()
    {
        Debug.Log("Device Regained");
        
    }

    private void OnJump()
    {
        Debug.Log("Jump");
    }

    private void OnInteract()
    {
        Debug.Log("Interact");
    }

    private void OnMove()
    {
        Debug.Log("Move");
    }

    private void OnOpenMenu()
    {
        Debug.Log("OpenMenu");
    }

    private void OnAccept()
    {
        Debug.Log("Accept");
    }

    private void OnRefuse()
    {
        Debug.Log("Refuse");
    }

}
