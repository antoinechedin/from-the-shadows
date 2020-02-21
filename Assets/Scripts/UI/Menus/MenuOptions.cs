using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MenuOptions : MonoBehaviour
{
    public MenuManager menuManager;

    public Selectable volumeGO;

    List<Resolution> resolutions;
    int cursorResolution;

    private void Awake()
    {
        resolutions = new List<Resolution>();
        resolutions.AddRange(Screen.resolutions);
        cursorResolution = resolutions.FindIndex(p => p.Equals(Screen.currentResolution));
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

    public void ChangeVolume(float value)
    {
        AudioListener.volume = Mathf.Clamp(AudioListener.volume + value, 0, 1);
    }

    public void ChangeResolution(float value)
    {
        cursorResolution = Mathf.Clamp(cursorResolution + (int)value, 0, resolutions.Count - 1);       

        Screen.SetResolution(resolutions[cursorResolution].width, resolutions[cursorResolution].height, Screen.fullScreen);
        
        Debug.Log("---------------------------------" + cursorResolution + "---------------------------------");
        Debug.Log(Screen.currentResolution);
        Debug.Log(resolutions[cursorResolution]); ;
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void Return()
    {
        if (menuManager != null)
            menuManager.OpenStartMenu();
        else
            gameObject.SetActive(false);
    }
}
