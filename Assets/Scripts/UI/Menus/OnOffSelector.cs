using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnOffSelector : Selectable
{
    public enum Direction { Left, Right }
    public Direction direction;
    public MenuOnOff menuOnOff;

    public override void OnSelect(BaseEventData eventData)
    {
        if (direction == Direction.Left) menuOnOff.SetValue(true);
        else if (direction == Direction.Right) menuOnOff.SetValue(false);
        StartCoroutine(SelectSlider());
    }

    private IEnumerator SelectSlider()
    {
        while (EventSystem.current.alreadySelecting)
        {
            yield return null;
        }

        EventSystem.current.SetSelectedGameObject(menuOnOff.gameObject);
    }
}
