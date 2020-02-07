using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public Canvas startMenu;
    public Canvas saveMenu;
    public Canvas chaptersMenu;

    public MenuChapter menuChapter;
    public MenuCamera menuCamera;

    public Button newGame;
    public Button firstSave;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.LoadAllSaveFiles();

        newGame.onClick.AddListener(delegate { OpenSaveMenu(); });

        if (GameManager.Instance.LoadingMenuInfos == null)
        {
            GameManager.Instance.LoadingMenuInfos = new LoadingMenuInfo(0);
        }

        int sceneIndex = GameManager.Instance.LoadingMenuInfos.StartingMenuScene;
        switch (sceneIndex)
        {
            case 0: // Start menu
                OpenStartMenu();
                break;
            case 1: // Saves menu
                OpenSaveMenu();
                break;
            case 2: // Chapters menu
                OpenChaptersMenu(GameManager.Instance.CurrentChapter);
                break;
            default:
                Debug.LogWarning("Menu index " + sceneIndex + " doesn't exist");
                break;
        }
    }

    public void OpenStartMenu()
    {
        startMenu.gameObject.SetActive(true);
        saveMenu.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(newGame.gameObject);
    }

    public void OpenSaveMenu()
    {
        saveMenu.gameObject.SetActive(true);
        startMenu.gameObject.SetActive(false);
        chaptersMenu.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstSave.gameObject);
    }

    public void OpenChaptersMenu(int chapterIndex)
    {
        chaptersMenu.gameObject.SetActive(true);
        saveMenu.gameObject.SetActive(false);
        startMenu.gameObject.SetActive(false);

        menuCamera.SetReturnToMainMenu(false);
        menuChapter.ResetInteractablesChaptersButtons();

        EventSystem.current.SetSelectedGameObject(menuChapter.chapterButtons[chapterIndex].gameObject);
    }

    public void Quit()
    {
        GameObject loadingScreen = (GameObject)Resources.Load("LoadingScreen");
        loadingScreen = Instantiate(loadingScreen, gameObject.transform);
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }

}
