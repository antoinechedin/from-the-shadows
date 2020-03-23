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

    private CurrentPlayer currentPlayer;

    private void Awake()
    {
        PlayShadow();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
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
    }

    private void PlayShadow()
    {
        currentPlayer = CurrentPlayer.Shadow;
        mesh.material = shadowMat;
        lightSourceGO.SetActive(false);
    }
}
