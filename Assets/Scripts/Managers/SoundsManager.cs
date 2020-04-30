using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    void Awake()
    {
        SetSoundsVolume();
    }

    private void OnEnable()
    {
        GameManager.Instance.optionsUpdateDelegate += SetSoundsVolume;
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
