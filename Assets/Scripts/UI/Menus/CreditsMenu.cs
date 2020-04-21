using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreditsMenu : MonoBehaviour, IDissolveMenu
{
    [HideInInspector] public MenuManager menuManager;
    public Button backButton;

    private void Update()
    {
        if (EventSystem.current.sendNavigationEvents)
        {
            if (InputManager.GetActionPressed(0, InputAction.Return)
                || Input.GetKeyDown(KeyCode.Escape)
                || Input.GetKeyDown(KeyCode.Backspace))
            {
                menuManager.DissolveFromMenuToMenu(this, menuManager.mainMenu);
            }
        }
    }

    public IEnumerator DissolveInCoroutine()
    {
        menuManager.menuCamera.SetReturnToStartMenu(true);
        gameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(backButton.gameObject);
        backButton.onClick.AddListener(delegate { menuManager.DissolveFromMenuToMenu(this, menuManager.mainMenu); });

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
        menuManager.menuCamera.SetReturnToStartMenu(false);
    }
}
