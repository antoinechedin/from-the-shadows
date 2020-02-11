using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionVolumeDisplay : MonoBehaviour
{
    public Text text; 

    void Update()
    {
        text.text = Mathf.RoundToInt(AudioListener.volume * 100).ToString(); // TODO : marche pas, trouver une autre méthode
    }
}