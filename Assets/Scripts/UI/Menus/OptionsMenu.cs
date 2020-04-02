using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour
{
    [HideInInspector] public int currentIndex;
    public Selectable[] selectables;

    private void Awake()
    {
        currentIndex = -1;
    }

    public void Init()
    {
        
    }

    public void HandleSelectTrigger(Selectable selectable)
    {
        int index = 0;
        for (int i = 0; i < selectables.Length; i++)
        {
            if (selectables[i] == selectable)
            {
                index = i;
                break;
            }
        }

        if (index != currentIndex)
        {
            if (currentIndex >= 0) selectables[currentIndex].animator.SetTrigger("Normal");
            currentIndex = index;
            selectable.animator.SetTrigger("Selected");
        }
    }
}
