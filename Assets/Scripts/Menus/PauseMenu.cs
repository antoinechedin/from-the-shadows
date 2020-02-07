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
        // TODO: Freeze the game
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
        // TODO: Unfreeze the game
    }

    public void Home()
    {
        GameManager.Instance.LoadMenu("MainMenu", new LoadingMenuInfo(2));
    }

    public void Options()
    {
        // TODO: Menu Options
    }

    public void Quit()
    {
        Application.Quit();
    }

}
