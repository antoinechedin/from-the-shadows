using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public RectTransform startMenu;
    public RectTransform savesMenu;
    public RectTransform optionsMenu;
    public RectTransform chaptersMenu;

    public MenuChapter menuChapter;
    public MenuCamera menuCamera;

    public Button newGame;
    public Button firstSave;

    // Start is called before the first frame update
    void Start()
    {
        SaveManager.Instance.LoadAllSaveFiles();

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
                if (GameManager.Instance.CurrentChapter != -1)
                {
                    OpenChaptersMenu(GameManager.Instance.CurrentChapter);
                }
                else
                {
                    Debug.LogWarning("WARN MenuManager.Start: CurrentSave not set. Opening at chapter");
                    OpenChaptersMenu(0);
                }

                break;
            default:
                Debug.LogWarning("Menu index " + sceneIndex + " doesn't exist");
                break;
        }
    }

    public void OpenStartMenu()
    {
        startMenu.gameObject.SetActive(true);
        savesMenu.gameObject.SetActive(false);
        chaptersMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(false);

        EventSystem.current.SetSelectedGameObject(newGame.gameObject);
    }

    public void OpenSaveMenu()
    {
        savesMenu.gameObject.SetActive(true);
        startMenu.gameObject.SetActive(false);
        chaptersMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(false);

        int lastSaveSelected = savesMenu.gameObject.GetComponent<SaveMenu>().LastSelected;
        Button lastButtonSelected = savesMenu.gameObject.GetComponent<SaveMenu>().buttons[lastSaveSelected];
        EventSystem.current.SetSelectedGameObject(lastButtonSelected.gameObject);
    }

    public void OpenChaptersMenu(int chapterIndex)
    {
        chaptersMenu.gameObject.SetActive(true);
        savesMenu.gameObject.SetActive(false);
        startMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(false);

        menuCamera.SetReturnToMainMenu(false);
        menuChapter.ResetInteractablesChaptersButtons();

        EventSystem.current.SetSelectedGameObject(menuChapter.chapterButtons[chapterIndex].gameObject);
    }

    public void OpenOptionsMenu()
    {
        optionsMenu.gameObject.SetActive(true);
        chaptersMenu.gameObject.SetActive(false);
        savesMenu.gameObject.SetActive(false);
        startMenu.gameObject.SetActive(false);

        optionsMenu.GetComponent<MenuOptions>().OpenOptionsMenu();
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
