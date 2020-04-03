using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class MenuSlider : MonoBehaviour, ISelectHandler
{
    public string playerPrefsId;
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
        bool changed = false;
        if (delta < 0 && currentValue > minValue)
        {
            changed = true;
            animator.SetTrigger("SliderLeft");
            currentValue += delta;
        }
        else if (delta > 0 && currentValue < maxValue)
        {
            changed = true;
            animator.SetTrigger("SliderRight");
            currentValue += delta;
        }

        if (changed)
        {
            if (currentValue == minValue) leftArrowImage.color = Color.gray;
            else leftArrowImage.color = Color.white;

            if (currentValue == maxValue) rightArrowImage.color = Color.gray;
            else rightArrowImage.color = Color.white;


            sliderText.text = currentValue.ToString();
            PlayerPrefs.SetInt(playerPrefsId, currentValue);
            GameManager.Instance.OnOptionUpdate();
        }
    }

    public void Init(int value)
    {
        currentValue = Mathf.Clamp(value, minValue, maxValue);
        sliderText.text = currentValue.ToString();

        if (currentValue == minValue) leftArrowImage.color = Color.gray;
        else leftArrowImage.color = Color.white;

        if (currentValue == maxValue) rightArrowImage.color = Color.gray;
        else rightArrowImage.color = Color.white;
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
