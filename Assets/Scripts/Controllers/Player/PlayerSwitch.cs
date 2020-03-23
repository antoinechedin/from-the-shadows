using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSwitch : MonoBehaviour
{
    public enum CurrentPlayer { Light, Shadow };

    public Material shadowMat;
    public Material lightMat;

    public SkinnedMeshRenderer mesh;

    private CurrentPlayer currentPlayer;

    private void Awake()
    {
        currentPlayer = CurrentPlayer.Shadow;
        mesh.material = shadowMat;
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
            currentPlayer = CurrentPlayer.Light;
            mesh.material = lightMat;
        }
        else
        {
            currentPlayer = CurrentPlayer.Shadow;
            mesh.material = shadowMat;
        }
    }
}
