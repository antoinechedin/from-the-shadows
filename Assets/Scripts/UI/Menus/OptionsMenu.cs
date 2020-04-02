using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour
{
    public MenuSlider musicSlider;
    public MenuSlider soundsSlider;

    [HideInInspector] public int currentIndex;
    [HideInInspector] public Selectable[] selectables;

    private void Awake()
    {
        currentIndex = -1;
        selectables = new Selectable[]
        {
            musicSlider.GetComponent<Selectable>(),
            soundsSlider.GetComponent<Selectable>()
        };

        Init();

    }

    public void Init()
    {
        int musicVolume = PlayerPrefs.GetInt("MusicVolume", 10);
        int soundsVolume = PlayerPrefs.GetInt("SoundsVolume", 10);

        musicSlider.Init(musicVolume);
        soundsSlider.Init(soundsVolume);
    }

    public void HandleSelectTrigger(Selectable selectable)
    {
        int index = 0;
        for (int i = 0; i < selectables.Length; i++)
        {
            if (selectables[i] == selectable)
            {
                index = i;
                break;
            }
        }

        if (index != currentIndex)
        {
            if (currentIndex >= 0) selectables[currentIndex].animator.SetTrigger("Normal");
            currentIndex = index;
            selectable.animator.SetTrigger("Selected");
        }
    }
}
