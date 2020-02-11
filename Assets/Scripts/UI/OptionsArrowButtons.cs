using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsArrowButtons : MonoBehaviour, ISelectHandler
{
    public Selectable origin;
    public float value;

    bool done;

    public void OnSelect(BaseEventData eventData)
    {
        done = false;
        StartCoroutine(ChangeVolume());
        done = true;
    }

    private IEnumerator ChangeVolume()
    {
        while (!done) 
        {
            yield return null;
        }
        AudioListener.volume += value;
        EventSystem.current.SetSelectedGameObject(origin.gameObject);        
    }
}
