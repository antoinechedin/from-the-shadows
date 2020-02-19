using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

    public Image background;
    public Image startMenuBackground;
    public TextMeshProUGUI version;

    private Animator backgroundAnimator;
    private Animator startMenuBackgroundAnimator;

    // Start is called before the first frame update
    void Start()
    {
        SaveManager.Instance.LoadAllSaveFiles();

        newGame.onClick.AddListener(delegate { OpenSaveMenu(); });

        if (GameManager.Instance.LoadingMenuInfos == null)
        {
            GameManager.Instance.LoadingMenuInfos = new LoadingMenuInfo(0);
        }

        backgroundAnimator = background.gameObject.GetComponent<Animator>();
        startMenuBackgroundAnimator = startMenuBackground.gameObject.GetComponent<Animator>();

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

        startMenuBackgroundAnimator.SetBool("fade", true);
        backgroundAnimator.SetBool("fade", false);
        version.text = "v" + Application.version + "\n2020 © Spooky Team";

        menuCamera.SetReturnToStartMenu(true);

        EventSystem.current.SetSelectedGameObject(newGame.gameObject);
    }

    public void OpenSaveMenu()
    {
        savesMenu.gameObject.SetActive(true);
        startMenu.gameObject.SetActive(false);
        chaptersMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(false);

        startMenuBackgroundAnimator.SetBool("fade", false);
        backgroundAnimator.SetBool("fade", true);

        menuCamera.SetReturnToStartMenu(false);
        menuCamera.SetReturnToSavesMenu(true);

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

        backgroundAnimator.SetBool("fade", false);
        startMenuBackgroundAnimator.SetBool("fade", false);

        menuCamera.SetReturnToSavesMenu(false);
        menuChapter.ResetInteractablesChaptersButtons();

        EventSystem.current.SetSelectedGameObject(menuChapter.chapterButtons[chapterIndex].gameObject);
    }

    public void OpenOptionsMenu()
    {
        optionsMenu.gameObject.SetActive(true);
        chaptersMenu.gameObject.SetActive(false);
        savesMenu.gameObject.SetActive(false);
        startMenu.gameObject.SetActive(false);

        backgroundAnimator.SetBool("fade", true);
        startMenuBackgroundAnimator.SetBool("fade", false);

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
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

}
