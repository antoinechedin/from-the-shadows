using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class MenuSlider : MonoBehaviour, ISelectHandler
{
    public int minValue = 0;
    public int maxValue = 10;
    [HideInInspector] public int currentValue;

    public TextMeshProUGUI sliderText;
    public Image leftArrowImage;
    public Image rightArrowImage;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void AddValueToSlider(int delta)
    {
        if (delta < 0 && currentValue > minValue)
        {
            animator.SetTrigger("SliderLeft");
            currentValue += delta;
        }
        else if (delta > 0 && currentValue < maxValue)
        {
            animator.SetTrigger("SliderRight");
            currentValue += delta;
        }

        if(currentValue == minValue) leftArrowImage.color = Color.gray;
        else leftArrowImage.color = Color.white;
        
        if(currentValue == maxValue) rightArrowImage.color = Color.gray;
        else rightArrowImage.color = Color.white;


        sliderText.text = currentValue.ToString();
    }

    public void OnSelect(BaseEventData eventData)
    {
        OptionsMenu optionMenu = GetComponentInParent<OptionsMenu>();
        if (optionMenu != null)
        {
            optionMenu.HandleSelectTrigger(GetComponent<Selectable>());
        }
    }
}
