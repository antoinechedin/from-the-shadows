using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionDisplay : MonoBehaviour
{
    public Text text;

    public string display;

    void Update()
    {
        StartCoroutine(display);
    }

    private void DisplayVolume()
    {
        text.text = Mathf.RoundToInt(AudioListener.volume * 100).ToString();
    }

    private void DisplayResolution()
    {
        text.text = Screen.width + "x" + Screen.height + " @ " + Screen.currentResolution.refreshRate;
    }

    private void DisplayFullscreen()
    {
        if (Screen.fullScreen)        
            text.text = "Fullscreen : on";
        else
            text.text = "Fullscreen : off";
    }
}