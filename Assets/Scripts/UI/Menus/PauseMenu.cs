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
        if (EventSystem.current.sendNavigationEvents)
        {
            if (optionsOpened)
            {
                if (Input.GetButtonDown("Start_G") || Input.GetButtonDown("B_G")) CloseOptions();
            }
            else
            {
                if (Input.GetButtonDown("Start_G")) Resume();
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
        Time.timeScale = 1;
        GameObject loadingScreen = (GameObject)Resources.Load("LoadingScreen");
        loadingScreen = Instantiate(loadingScreen, transform.parent);
        GameObject.FindObjectOfType<ChapterManager>().CollectMetaData();
        SaveManager.Instance.WriteSaveFile();
        StartCoroutine(Fade());
    }
    IEnumerator Fade()
    {
        yield return new WaitForSeconds(1f);
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

}
