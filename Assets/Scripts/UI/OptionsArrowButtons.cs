﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsArrowButtons : MonoBehaviour, ISelectHandler
{
    public Selectable origin;
    public float value;

    public string onSelectFunction;

    bool done;

    public void OnSelect(BaseEventData eventData)
    {
        done = false;
        StartCoroutine(onSelectFunction);
        done = true;
    }

    private IEnumerator ChangeVolume()
    {
        while (!done) 
        {
            yield return null;
        }
        
        AudioListener.volume = Mathf.Clamp(AudioListener.volume + value, 0, 1);
        EventSystem.current.SetSelectedGameObject(origin.gameObject);        
    }
}