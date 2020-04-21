using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSwitch : MonoBehaviour
{
    private enum CurrentPlayer { Light, Shadow };

    public Material shadowMat;
    public Material lightMat;
    public SkinnedMeshRenderer mesh;
    public GameObject lightSourceGO;
    public GameObject shadowCaster;

    private CurrentPlayer currentPlayer;

    private void Awake()
    {
        PlayShadow();
    }

    void Update()
    {
        //if (Input.GetButtonDown("RB_G") || Input.GetKeyDown(KeyCode.LeftShift))
        if (InputManager.GetActionPressed(0, InputAction.Switch))
        {
            TogglePlayer();
        }
    }

    private void TogglePlayer()
    {
        if(currentPlayer == CurrentPlayer.Shadow)
        {
            PlayLight();
        }
        else
        {
            PlayShadow();
        }
    }

    private void PlayLight()
    {
        currentPlayer = CurrentPlayer.Light;
        mesh.material = lightMat;
        lightSourceGO.SetActive(true);
        shadowCaster.transform.GetChild(0).gameObject.SetActive(false);
        shadowCaster.transform.GetChild(1).gameObject.SetActive(true);
    }

    private void PlayShadow()
    {
        currentPlayer = CurrentPlayer.Shadow;
        mesh.material = shadowMat;
        lightSourceGO.SetActive(false);
        shadowCaster.transform.GetChild(1).gameObject.SetActive(false);
        shadowCaster.transform.GetChild(0).gameObject.SetActive(true);
    }

    public string GetCurrentPlayer()
    {
        if (currentPlayer == CurrentPlayer.Shadow)
            return "Shadow";
        else
            return "Light";
    }
}
