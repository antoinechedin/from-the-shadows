using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    void Awake()
    {
        GameManager.Instance.optionsUpdateDelegate += SetSoundsVolume;
        SetSoundsVolume();
    }

    public void SetSoundsVolume()
    {
        if (PlayerPrefs.HasKey("SoundsVolume"))
        {
            AudioListener.volume = (float)PlayerPrefs.GetInt("SoundsVolume") / 10f;
        }
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.optionsUpdateDelegate -= SetSoundsVolume;
    }
}
