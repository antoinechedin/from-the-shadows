using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, IDissolveMenu
{
    [HideInInspector] public MenuManager menuManager;
    public Button playButton;
    public Button optionsButton;
    public Button creditsButton;
    public Button quitButton;

    public GameObject lastSelectedGameObject;

    private void Awake()
    {
        lastSelectedGameObject = playButton.gameObject;
        playButton.onClick.AddListener(delegate
        {
            lastSelectedGameObject = playButton.gameObject;
            menuManager.DissolveFromMenuToMenu(this, menuManager.savesMenu);
        });
        optionsButton.onClick.AddListener(delegate
        {
            lastSelectedGameObject = optionsButton.gameObject;
            menuManager.DissolveFromMenuToMenu(this, menuManager.optionsMenu);
        });
        creditsButton.onClick.AddListener(delegate
        {
            lastSelectedGameObject = creditsButton.gameObject;
            menuManager.DissolveFromMenuToMenu(this, menuManager.creditsMenu);
        });
    }

    public IEnumerator DissolveInCoroutine()
    {
        gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(lastSelectedGameObject);
        DissolveController[] dissolves = GetComponentsInChildren<DissolveController>();
        for (int i = 0; i < dissolves.Length - 1; i++)
        {
            StartCoroutine(dissolves[i].DissolveInCoroutine(MenuManager.dissolveDuration));
            yield return new WaitForSecondsRealtime(MenuManager.dissolveOffset);
        }

        EventSystem.current.sendNavigationEvents = true;

        yield return StartCoroutine(dissolves[dissolves.Length - 1].DissolveInCoroutine(MenuManager.dissolveDuration));
    }

    public IEnumerator DissolveOutCoroutine()
    {
        EventSystem.current.sendNavigationEvents = false;

        DissolveController[] dissolves = GetComponentsInChildren<DissolveController>();
        for (int i = 0; i < dissolves.Length - 1; i++)
        {
            StartCoroutine(dissolves[i].DissolveOutCoroutine(MenuManager.dissolveDuration));
            yield return new WaitForSecondsRealtime(MenuManager.dissolveOffset);
        }

        yield return StartCoroutine(dissolves[dissolves.Length - 1].DissolveOutCoroutine(MenuManager.dissolveDuration));
        gameObject.SetActive(false);
    }
}
