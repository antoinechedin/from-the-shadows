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
    public MenuCamera menuCamera;
    public Button newGame;
    public Button firstSave;
    public Button firstChapter;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.LoadAllSaveFiles();
        newGame.onClick.AddListener(delegate { OpenSaveMenu(); });
        switch (GameManager.Instance.StartMenuScene)
        {
            case 0: // start menu
                OpenStartMenu();
                break;
            case 1: // save menu
                OpenSaveMenu();
                break;
            case 2: // chapter menu
                OpenChaptersMenu(GameManager.Instance.StartChapterIndex);
                break;
            default:
                Debug.Log("Menu index doesn't exist");
                break;
        }
        EventSystem.current.SetSelectedGameObject(newGame.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(EventSystem.current.currentSelectedGameObject);
        // if (EventSystem.current.currentSelectedGameObject ==)
        // {
        //     EventSystem.current.SetSelectedGameObject(newGame.gameObject);
        // }
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
        chaptersMenu.gameObject.GetComponent<MenuChapter>().SetCurrentChapter(chapterIndex);
        saveMenu.gameObject.SetActive(false);
        menuCamera.SetReturnToMainMenu(false);
        EventSystem.current.SetSelectedGameObject(firstChapter.gameObject);
    }

}
