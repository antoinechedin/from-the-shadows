using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour, IDissolveMenu
{
    public MenuSlider musicSlider;
    public MenuSlider soundsSlider;

    [HideInInspector] public MenuManager menuManager;
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

    private void Update()
    {
        if (EventSystem.current.sendNavigationEvents)
        {
            if (Input.GetButtonDown("B_G"))
            {
                menuManager.DissolveFromMenuToMenu(this, menuManager.mainMenu);
            }
        }
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

    public IEnumerator DissolveInCoroutine()
    {
        gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(selectables[0].gameObject);
        DissolveController[] dissolves = GetComponentsInChildren<DissolveController>();
        for (int i = 0; i < dissolves.Length - 1; i++)
        {
            StartCoroutine(dissolves[i].DissolveInCoroutine(menuManager.dissolveDuration));
            yield return new WaitForSeconds(menuManager.dissolveOffset);
        }

        EventSystem.current.sendNavigationEvents = true;

        yield return StartCoroutine(dissolves[dissolves.Length - 1].DissolveInCoroutine(menuManager.dissolveDuration));
    }

    public IEnumerator DissolveOutCoroutine()
    {
        EventSystem.current.sendNavigationEvents = false;

        DissolveController[] dissolves = GetComponentsInChildren<DissolveController>();
        for (int i = 0; i < dissolves.Length - 1; i++)
        {
            StartCoroutine(dissolves[i].DissolveOutCoroutine(menuManager.dissolveDuration));
            yield return new WaitForSeconds(menuManager.dissolveOffset);
        }

        yield return StartCoroutine(dissolves[dissolves.Length - 1].DissolveOutCoroutine(menuManager.dissolveDuration));
        gameObject.SetActive(false);
    }
}
