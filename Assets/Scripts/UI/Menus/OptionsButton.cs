using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsButton : MonoBehaviour, ISelectHandler
{
    [HideInInspector] public OptionsMenu optionsMenu;

    public void Init(OptionsMenu optionsMenu)
    {
        this.optionsMenu = optionsMenu;
    }

    public void OnSelect(BaseEventData eventData)
    {
        optionsMenu.HandleSelectTrigger(GetComponent<Selectable>());
    }
}
