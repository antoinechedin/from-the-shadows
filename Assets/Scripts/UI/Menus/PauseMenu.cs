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

    public AudioClip uiPress;
    public AudioClip uiSelect;

    public Image foreground;

    private bool optionsOpened;

    private void Awake()
    {
        optionsOpened = false;
    }

    public void OpenPauseMenu()
    {
        EventSystem.current.SetSelectedGameObject(mainPauseMenu.resumeButton.gameObject);
        Time.timeScale = 0;
    }

    private void Update()
    {
        {
            if (EventSystem.current.sendNavigationEvents)
                if (optionsOpened)
                {
                    if (InputManager.GetActionPressed(0, InputAction.Return)
                    || Input.GetKeyDown(KeyCode.Escape)
                    || Input.GetKeyDown(KeyCode.Backspace))
                        CloseOptions();
                }
                else
                {
                    if (InputManager.GetActionPressed(0, InputAction.Pause)
                    || Input.GetKeyDown(KeyCode.Escape))
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
        GetComponentInParent<Canvas>().GetComponent<AudioSource>().PlayOneShot(uiPress);
    }

    public void Home()
    {
        EventSystem.current.sendNavigationEvents = false;
        GameObject.FindObjectOfType<ChapterManager>().CollectMetaData();
        GameObject.Find("MusicManager").GetComponent<MusicManager>().StopTheme();
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
        GetComponentInParent<Canvas>().GetComponent<AudioSource>().PlayOneShot(uiPress);
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
        EventSystem.current.sendNavigationEvents = false;
        //GameObject.Find("MusicManager").GetComponent<MusicManager>().StopTheme();
        GameObject.FindObjectOfType<ChapterManager>().CollectMetaData();
        SaveManager.Instance.WriteSaveFile();
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        float timer = 0;
        float DURATION = 3f;

        while (timer < DURATION)
        {
            timer += Time.unscaledDeltaTime;
            if (timer > DURATION) timer = DURATION;

            float alpha = timer / DURATION;
            foreground.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

}
