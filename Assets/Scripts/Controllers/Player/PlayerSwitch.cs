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
    public GameObject shadowPointLight;

    private CurrentPlayer currentPlayer;

    private void Awake()
    {
        PlayShadow();
    }

    public void SwitchPlayer()
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
        shadowPointLight.SetActive(false);
    }

    private void PlayShadow()
    {
        currentPlayer = CurrentPlayer.Shadow;
        mesh.material = shadowMat;
        lightSourceGO.SetActive(false);
        shadowPointLight.SetActive(true);
    }

    public string GetCurrentPlayer()
    {
        if (currentPlayer == CurrentPlayer.Shadow)
            return "Shadow";
        else
            return "Light";
    }
}
