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
        InitOptions();

        resolutions = new List<Resolution>();
        resolutions.AddRange(Screen.resolutions);
        cursorResolution = resolutions.FindIndex(p => p.Equals(Screen.currentResolution));
    }

    private void InitOptions()
    {
        if (PlayerPrefs.HasKey("Volume"))
            AudioListener.volume = PlayerPrefs.GetFloat("Volume");

        if (PlayerPrefs.HasKey("ScreenWidth") && PlayerPrefs.HasKey("ScreenHeight"))        
            Screen.SetResolution(PlayerPrefs.GetInt("ScreenWidth"), PlayerPrefs.GetInt("ScreenHeight"), true);

        if (PlayerPrefs.HasKey("Fullscreen"))
            Screen.fullScreen = PlayerPrefs.GetInt("Fullscreen") == 0 ? false : true;
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
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);
    }

    public void ChangeResolution(float value)
    {
        cursorResolution = Mathf.Clamp(cursorResolution + (int)value, 0, resolutions.Count - 1);       
        Screen.SetResolution(resolutions[cursorResolution].width, resolutions[cursorResolution].height, Screen.fullScreen);
        PlayerPrefs.SetInt("ScreenWidth", Screen.currentResolution.width);
        PlayerPrefs.SetInt("ScreenHeight", Screen.currentResolution.height);
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        PlayerPrefs.SetInt("Fullscreen", Screen.fullScreen ? 1 : 0);
    }

    public void Return()
    {
        if (menuManager != null)
           menuManager.OpenStartMenu();
        else
            gameObject.SetActive(false);
    }
}
