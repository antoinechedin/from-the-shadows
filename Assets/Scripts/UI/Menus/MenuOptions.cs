using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MenuOptions : MonoBehaviour
{
    public Selectable volumeGO;

    private void Awake()
    {
        
    }

    public void OpenOptionsMenu()
    {
        EventSystem.current.SetSelectedGameObject(volumeGO.gameObject);
    }

    private void Update()
    {
        if (Input.GetButtonDown("B_G"))
        {
            Return();
        }
    }

    public void ChangeVolume()
    {
        //AudioListener.volume = ;
    }

    public void ChangeResolution()
    {
        //Screen.SetResolution();
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    public void Return()
    {
        throw new NotImplementedException();
    }
}
