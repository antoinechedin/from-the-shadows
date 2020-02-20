﻿using System.Collections;
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
        text.text = Screen.currentResolution.width + "x" + Screen.currentResolution.height + " @ " + Screen.currentResolution.refreshRate;
    }
}