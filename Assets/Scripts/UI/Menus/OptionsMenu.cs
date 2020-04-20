using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class OptionsMenu : MonoBehaviour, IDissolveMenu
{
    [HideInInspector] public int currentIndex;

    private bool listeningKey = false;
    private MenuControlsButton currentControlsButton;


    [Header("Component refs")]
    [HideInInspector] public MenuManager menuManager;
    public MenuSlider musicSlider;
    public MenuSlider soundsSlider;
    public MenuControlsButton[] controlsButtons;
    public OptionsButton saveButton;
    public OptionsButton resetButton;
    [HideInInspector] public Selectable[] selectables;
    public CanvasGroup PressAKeyCanvasGroup;

    private void Awake()
    {
        currentIndex = -1;
        selectables = new Selectable[4 + controlsButtons.Length];

        selectables[0] = musicSlider.GetComponent<Selectable>();
        selectables[1] = soundsSlider.GetComponent<Selectable>();

        for (int i = 0; i < controlsButtons.Length; i++)
        {
            selectables[2 + i] = controlsButtons[i].GetComponent<Selectable>();
        }

        selectables[selectables.Length - 2] = saveButton.GetComponent<Selectable>();
        selectables[selectables.Length - 1] = resetButton.GetComponent<Selectable>();

        Init();
    }

    public void Init()
    {
        int musicVolume = PlayerPrefs.GetInt("MusicVolume", 10);
        int soundsVolume = PlayerPrefs.GetInt("SoundsVolume", 10);

        musicSlider.Init(musicVolume, this);
        soundsSlider.Init(soundsVolume, this);
        foreach (MenuControlsButton controlsButton in controlsButtons)
        {
            controlsButton.Init(this);
        }
        saveButton.Init(this);
        resetButton.Init(this);
    }

    private void Update()
    {
        if (EventSystem.current.sendNavigationEvents && menuManager != null)
        {
            if (InputManager.GetActionPressed(0, InputAction.Return)
                || Input.GetKeyDown(KeyCode.Escape)
                || Input.GetKeyDown(KeyCode.Backspace))
            {
                menuManager.DissolveFromMenuToMenu(this, menuManager.mainMenu);
            }
        }

        if (listeningKey)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    Debug.Log("OptionsMenu: " + keyCode.ToString() + " pressed");
                    PlayerPrefs.SetInt(currentControlsButton.playerPrefsId, (int)keyCode);

                    InputManager.UpdateKeyMapping();
                    currentControlsButton.UpdateButton();

                    listeningKey = false;
                    currentControlsButton = null;
                    PressAKeyCanvasGroup.alpha = 0f;
                    StartCoroutine(StopListeningKeyCoroutine());
                }
            }
        }
    }

    public void ResetControls()
    {
        foreach (InputAction action in Enum.GetValues(typeof(InputAction)))
        {
            PlayerPrefs.DeleteKey("P1_" + action.ToString());
            PlayerPrefs.DeleteKey("P2_" + action.ToString());
        }
        InputManager.UpdateKeyMapping();
        foreach (MenuControlsButton button in controlsButtons)
        {
            button.UpdateButton();
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

    public void StartListeningKey(MenuControlsButton controlsButton)
    {
        EventSystem.current.sendNavigationEvents = false;
        currentControlsButton = controlsButton;
        PressAKeyCanvasGroup.alpha = 0.9f;
        StartCoroutine(StartListeningKeyCoroutine());
    }

    private IEnumerator StartListeningKeyCoroutine()
    {
        yield return null;
        listeningKey = true;
    }

    private IEnumerator StopListeningKeyCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        EventSystem.current.sendNavigationEvents = true;
    }

    public IEnumerator DissolveInCoroutine()
    {
        currentIndex = -1;
        if(menuManager != null) menuManager.menuCamera.SetReturnToStartMenu(true);
        gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(selectables[0].gameObject);
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
        if(menuManager != null) menuManager.menuCamera.SetReturnToStartMenu(false);
    }
}
