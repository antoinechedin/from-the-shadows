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
    }

    public IEnumerator DissolveInCoroutine()
    {
        gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(lastSelectedGameObject);
        DissolveController[] dissolves = GetComponentsInChildren<DissolveController>();
        for (int i = 0; i < dissolves.Length - 1; i++)
        {
            StartCoroutine(dissolves[i].DissolveInCoroutine(menuManager.dissolveDuration));
            yield return new WaitForSeconds(menuManager.dissolveOffset);
        }

        yield return StartCoroutine(dissolves[dissolves.Length - 1].DissolveInCoroutine(menuManager.dissolveDuration));
        EventSystem.current.sendNavigationEvents = true;
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
