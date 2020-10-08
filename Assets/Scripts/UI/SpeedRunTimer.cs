using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedRunTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI background;
    [SerializeField] private TextMeshProUGUI mainText;

    public void SetText(string value)
    {
        background.text = value;
        mainText.text = value;
    }
}
