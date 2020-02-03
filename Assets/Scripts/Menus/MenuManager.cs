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

    public void OpenChaptersMenu()
    {
        chaptersMenu.gameObject.SetActive(true);
        saveMenu.gameObject.SetActive(false);
        menuCamera.SetReturnToMainMenu(false);
        EventSystem.current.SetSelectedGameObject(firstChapter.gameObject);
    }

}
