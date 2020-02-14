using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MenuOptions : MonoBehaviour
{
    public Selectable volumeGO;

    Dictionary<int, int> resolutions;
    List<int> keysResolution;
    public int cursorResolution;

    private void Awake()
    {
        keysResolution = new List<int>();
        keysResolution.Add(1280);
        keysResolution.Add(1920);

        resolutions = new Dictionary<int, int>();
        resolutions[1280] = 720;
        resolutions[1920] = 1080;
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
        cursorResolution = (cursorResolution + (int)value) % (keysResolution.Count - 1);
        int width = keysResolution[cursorResolution];
        int height = resolutions[keysResolution[cursorResolution]];

        Screen.SetResolution(width, height, Screen.fullScreen);
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
