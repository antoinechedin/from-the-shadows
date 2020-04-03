using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreditsMenu : MonoBehaviour, IDissolveMenu
{
    [HideInInspector] public MenuManager menuManager;

    private void Update()
    {
        if(EventSystem.current.sendNavigationEvents)
        {
            if(Input.GetButtonDown("B_G"))
            {
                menuManager.DissolveFromMenuToMenu(this, menuManager.mainMenu);
            }
        }
    }

    public IEnumerator DissolveInCoroutine()
    {
        gameObject.SetActive(true);
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
