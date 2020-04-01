using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuSlider : MonoBehaviour
{
    public int minValue = 0;
    public int maxValue = 10;
    [HideInInspector] public int currentValue;

    public TextMeshProUGUI sliderText;

    public void AddValueToSlider(int delta)
    {
        currentValue = Mathf.Clamp(currentValue + delta, minValue, maxValue);
        sliderText.text = currentValue.ToString();
    }
}
