using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public Button resumeButton;

    public void OpenPauseMenu()
    {
        Time.timeScale = 0;
        EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Start_G")) Resume();
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
        Time.timeScale = 1;
        SaveManager.Instance.WriteSaveFile();
        GameManager.Instance.LoadMenu("MainMenu", new LoadingMenuInfo(2));
    }

    public void Options()
    {
        // Time.timeScale = 1;
        // TODO: Menu Options
    }

    public void Quit()
    {
        Time.timeScale = 1;
        GameObject loadingScreen = (GameObject)Resources.Load("LoadingScreen");
        loadingScreen = Instantiate(loadingScreen, transform.parent);
        SaveManager.Instance.WriteSaveFile();
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }

}
