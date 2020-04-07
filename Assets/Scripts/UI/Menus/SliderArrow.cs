using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderArrow : Selectable
{
    public enum Direction { Left, Right }
    public Direction direction;
    public MenuSlider menuSlider;

    public override void OnSelect(BaseEventData eventData)
    {
        if (direction == Direction.Left) menuSlider.AddValueToSlider(-1);
        else if (direction == Direction.Right) menuSlider.AddValueToSlider(1);
        StartCoroutine(SelectSlider());
    }

    private IEnumerator SelectSlider()
    {
        while (EventSystem.current.alreadySelecting)
        {
            yield return null;
        }

        EventSystem.current.SetSelectedGameObject(menuSlider.gameObject);
    }
}
