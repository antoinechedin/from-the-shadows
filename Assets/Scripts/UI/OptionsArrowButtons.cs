using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsArrowButtons : MonoBehaviour, ISelectHandler
{
    public MenuOptions menuOptions;

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

        menuOptions.ChangeVolume(value);

        EventSystem.current.SetSelectedGameObject(origin.gameObject);        
    }

    private IEnumerator ChangeResolution()
    {
        while (!done)
        {
            yield return null;
        }

        menuOptions.ChangeResolution(value);

        EventSystem.current.SetSelectedGameObject(origin.gameObject);
    }
}
