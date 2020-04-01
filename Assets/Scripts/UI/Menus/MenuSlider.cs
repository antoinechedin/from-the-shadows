using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MenuSlider : MonoBehaviour
{
    public int minValue = 0;
    public int maxValue = 10;
    [HideInInspector] public int currentValue;

    public TextMeshProUGUI sliderText;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void AddValueToSlider(int delta)
    {
        currentValue = Mathf.Clamp(currentValue + delta, minValue, maxValue);
        sliderText.text = currentValue.ToString();
        if(delta < 0) animator.SetTrigger("SliderLeft");
        if(delta > 0) animator.SetTrigger("SliderRight");
    }
}
