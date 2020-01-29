using System;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class InputManager : MonoBehaviour
{
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
