using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelButton : MonoBehaviour, ISelectHandler
{
    public int levelNumber;
    public MenuLevels menuLevels;

    public void OnSelect(BaseEventData eventData)
    {
        //menuLevels.SetMenuLevelInfo(levelNumber);
    }
}
