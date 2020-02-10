using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MenuOptions : MonoBehaviour
{
    public Slider volumeSlider;
    public Text resolutionText;
    public Toggle toggleFullscreen;

    private void Awake()
    {
        volumeSlider.value = AudioListener.volume;
        resolutionText.text = Screen.currentResolution.width + "x" + Screen.currentResolution.height;
        toggleFullscreen.isOn = Screen.fullScreen;
    }

    public void OpenOptionsMenu()
    {
        EventSystem.current.SetSelectedGameObject(volumeSlider.gameObject);
    }

    private void Update()
    {
        if (Input.GetButtonDown("B_G"))
        {
            Back();
        }
    }

    private void Back()
    {
        throw new NotImplementedException();
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
