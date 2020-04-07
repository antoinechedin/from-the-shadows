using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{
    public MainPauseMenu mainPauseMenu;
    public OptionsMenu optionsMenu;

    public DissolveController foregroundDissolveController;

    private bool optionsOpened;

    private void Awake()
    {
        optionsOpened = false;
        gameObject.SetActive(false);
    }

    public void OpenPauseMenu()
    {
        Time.timeScale = 0;
        EventSystem.current.SetSelectedGameObject(mainPauseMenu.resumeButton.gameObject);
    }

    private void Update()
    {
        {
        if (EventSystem.current.sendNavigationEvents)
            if (optionsOpened)
            {
                if (InputManager.GetActionPressed(1, InputAction.Pause) || InputManager.GetActionPressed(2, InputAction.Pause)) 
                    CloseOptions();
            }
            else
            {
                if (InputManager.GetActionPressed(1, InputAction.Pause) || InputManager.GetActionPressed(2, InputAction.Pause)) 
                    Resume();
            }
        }
        
    }

    public void Resume()
    {
        EventSystem.current.SetSelectedGameObject(null);
        gameObject.SetActive(false);
        Input.ResetInputAxes();
        Time.timeScale = 1;
    }

    public void Home()
    {
        Input.ResetInputAxes();
        Time.timeScale = 1;
        GameObject.FindObjectOfType<ChapterManager>().CollectMetaData();
        SaveManager.Instance.WriteSaveFile();
        GameManager.Instance.LoadMenu("MainMenu", new LoadingMenuInfo(2));
    }

    public void OpenOptions()
    {
        StartCoroutine(OpenOptionsCoroutine());
    }

    private IEnumerator OpenOptionsCoroutine()
    {
        yield return StartCoroutine(mainPauseMenu.DissolveOutCoroutine());
        optionsOpened = true;
        yield return StartCoroutine(optionsMenu.DissolveInCoroutine());
    }

    public void CloseOptions()
    {
        StartCoroutine(CloseOptionsCoroutine());
    }

    private IEnumerator CloseOptionsCoroutine()
    {
        yield return StartCoroutine(optionsMenu.DissolveOutCoroutine());
        optionsOpened = false;
        yield return StartCoroutine(mainPauseMenu.DissolveInCoroutine());
    }

    public void Quit()
    {
        GameObject.FindObjectOfType<ChapterManager>().CollectMetaData();
        SaveManager.Instance.WriteSaveFile();
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        yield return StartCoroutine(foregroundDissolveController.DissolveInCoroutine(3f));
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

}
