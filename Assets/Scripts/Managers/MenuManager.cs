using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Coffee.UIExtensions;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public RectTransform startMenu;
    public SavesMenu savesMenu;
    public RectTransform optionsMenu;
    public RectTransform chaptersMenu;

    public MenuChapter menuChapter;
    public MenuCamera menuCamera;

    public Button play;
    // public Button options;
    // public Button quit;
    public Button firstSave;

    public Image background;
    public Image startMenuBackground;
    public TextMeshProUGUI version;

    public float dissolveDuration;
    public float dissolveOffset;

    private Animator backgroundAnimator;
    private Animator startMenuBackgroundAnimator;

    // private Dissolve titleDissolve;
    // private Dissolve playDissolve;
    // private Dissolve optionsDissolve;
    // private Dissolve quitDissolve;

    void Start()
    {
        // titleDissolve = startMenu.Find("Menu").Find("Image").GetComponent<Dissolve>();
        // playDissolve = play.GetComponentInChildren<Dissolve>();
        // optionsDissolve = options.GetComponentInChildren<Dissolve>();
        // quitDissolve = quit.GetComponentInChildren<Dissolve>();

        SaveManager.Instance.LoadAllSaveFiles();

        // play.onClick.AddListener(delegate { StartCoroutine(OpenSaveMenuCoroutine()); });
        // options.onClick.AddListener(delegate { StartCoroutine(OpenOptionsMenuCoroutine()); });
        // quit.onClick.AddListener(delegate { StartCoroutine(QuitCoroutine()); });

        if (GameManager.Instance.LoadingMenuInfos == null)
        {
            GameManager.Instance.LoadingMenuInfos = new LoadingMenuInfo(0);
        }

        // backgroundAnimator = background.gameObject.GetComponent<Animator>();
        startMenuBackgroundAnimator = startMenuBackground.gameObject.GetComponent<Animator>();

        int sceneIndex = GameManager.Instance.LoadingMenuInfos.StartingMenuScene;
        int finishChapterForFirstTime = GameManager.Instance.LoadingMenuInfos.FinishChapterForFirstTime;
        switch (sceneIndex)
        {
            case 0: // Start menu
                StartCoroutine(OpenStartMenuCoroutine());
                break;
            case 1: // Saves menu
                StartCoroutine(OpenSaveMenuCoroutine());
                break;
            case 2: // Chapters menu
                if (GameManager.Instance.CurrentChapter != -1)
                {
                    OpenChaptersMenu(GameManager.Instance.CurrentChapter, finishChapterForFirstTime);
                }
                else
                {
                    Debug.LogWarning("WARN MenuManager.Start: CurrentSave not set. Opening at chapter");
                    OpenChaptersMenu(0, -1);
                }

                break;
            default:
                Debug.LogWarning("Menu index " + sceneIndex + " doesn't exist");
                break;
        }
    }

    private void Update()
    {
        // Debug
        if (Input.GetKeyDown(KeyCode.I))
        {
            // StartCoroutine(ButtonsDissolveIn());
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            // StartCoroutine(ButtonsDissolveOut());
        }
    }

    public void OpenStartMenu()
    {
        StartCoroutine(OpenStartMenuCoroutine());
    }

    private IEnumerator OpenStartMenuCoroutine()
    {
        EventSystem.current.sendNavigationEvents = false;

        DissolveController[] dissolves = savesMenu.GetComponentsInChildren<DissolveController>();
        for (int i = 0; i < dissolves.Length - 1; i++)
        {
            StartCoroutine(dissolves[i].DissolveOutCoroutine(dissolveDuration));
            yield return new WaitForSeconds(dissolveOffset);
        }

        yield return StartCoroutine(dissolves[dissolves.Length - 1].DissolveOutCoroutine(dissolveDuration));

        startMenu.gameObject.SetActive(true);
        savesMenu.gameObject.SetActive(false);
        chaptersMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(false);

        EventSystem.current.SetSelectedGameObject(play.gameObject);

        // startMenuBackground.gameObject.SetActive(true);
        // startMenuBackgroundAnimator.SetBool("fade", true);
        // backgroundAnimator.SetBool("fade", false);
        version.text = Application.version + "\n2020 © " + Application.companyName;

        dissolves = startMenu.GetComponentsInChildren<DissolveController>();
        for (int i = 0; i < dissolves.Length - 1; i++)
        {
            StartCoroutine(dissolves[i].DissolveInCoroutine(dissolveDuration));
            yield return new WaitForSeconds(dissolveOffset);
        }

        EventSystem.current.sendNavigationEvents = true;
        yield return StartCoroutine(dissolves[dissolves.Length - 1].DissolveInCoroutine(dissolveDuration));

        menuCamera.SetReturnToStartMenu(true);
        // yield return StartCoroutine(ButtonsDissolveIn());

        EventSystem.current.sendNavigationEvents = true;
    }

    public void OpenSaveMenu()
    {
        StartCoroutine(OpenSaveMenuCoroutine());
    }

    private IEnumerator OpenSaveMenuCoroutine()
    {

        EventSystem.current.sendNavigationEvents = false;
        // yield return StartCoroutine(ButtonsDissolveOut());        

        if (startMenu.gameObject.activeSelf)
        {
            // startMenuBackgroundAnimator.SetBool("fade", false);
        }
        // backgroundAnimator.SetBool("fade", true);

        DissolveController[] dissolves = startMenu.GetComponentsInChildren<DissolveController>();

        for (int i = 0; i < dissolves.Length - 1; i++)
        {
            StartCoroutine(dissolves[i].DissolveOutCoroutine(dissolveDuration));
            yield return new WaitForSeconds(dissolveOffset);
        }

        yield return StartCoroutine(dissolves[dissolves.Length - 1].DissolveOutCoroutine(dissolveDuration));

        startMenu.gameObject.SetActive(false);
        savesMenu.gameObject.SetActive(true);
        chaptersMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(false);

        int lastSaveSelected = savesMenu.gameObject.GetComponent<SavesMenu>().LastSelected;
        Button lastButtonSelected = savesMenu.gameObject.GetComponent<SavesMenu>().savesButons[lastSaveSelected];
        EventSystem.current.SetSelectedGameObject(lastButtonSelected.gameObject);

        dissolves = savesMenu.GetComponentsInChildren<DissolveController>();
        for (int i = 0; i < dissolves.Length - 1; i++)
        {
            StartCoroutine(dissolves[i].DissolveInCoroutine(dissolveDuration));
            yield return new WaitForSeconds(dissolveOffset);
        }

        EventSystem.current.sendNavigationEvents = true;
        yield return StartCoroutine(dissolves[dissolves.Length - 1].DissolveInCoroutine(dissolveDuration));

        menuCamera.SetReturnToStartMenu(false);
        menuCamera.SetReturnToSavesMenu(true);

        //yield return StartCoroutine(SavesDissolveIn());
    }

    public void OpenChaptersMenu(int chapterIndex, int chapterFirstCompleted)
    {
        StartCoroutine(OpenChaptersMenuCoroutine(chapterIndex, chapterFirstCompleted));
    }

    private IEnumerator OpenChaptersMenuCoroutine(int chapterIndex, int chapterFirstCompleted)
    {
        EventSystem.current.sendNavigationEvents = false;

        DissolveController[] dissolves = savesMenu.GetComponentsInChildren<DissolveController>();
        for (int i = 0; i < dissolves.Length - 1; i++)
        {
            StartCoroutine(dissolves[i].DissolveOutCoroutine(dissolveDuration));
            yield return new WaitForSeconds(dissolveOffset);
        }
        yield return StartCoroutine(dissolves[dissolves.Length - 1].DissolveOutCoroutine(dissolveDuration));


        startMenu.gameObject.SetActive(false);
        savesMenu.gameObject.SetActive(false);
        chaptersMenu.gameObject.SetActive(true);
        optionsMenu.gameObject.SetActive(false);

        savesMenu.actionChoiceButtons.gameObject.SetActive(false);
        savesMenu.newGameChoiceButtons.gameObject.SetActive(false);

        // backgroundAnimator.SetBool("fade", false);

        menuCamera.SetReturnToSavesMenu(false);
        menuChapter.ResetInteractablesChaptersButtons();

        EventSystem.current.SetSelectedGameObject(menuChapter.chapterButtons[chapterIndex].gameObject);

        dissolves = chaptersMenu.GetComponentsInChildren<DissolveController>();
        for (int i = 0; i < dissolves.Length - 1; i++)
        {
            StartCoroutine(dissolves[i].DissolveInCoroutine(dissolveDuration));
            yield return new WaitForSeconds(dissolveOffset);
        }
        EventSystem.current.sendNavigationEvents = true;
        yield return StartCoroutine(dissolves[dissolves.Length - 1].DissolveInCoroutine(dissolveDuration));

        // if (chapterFirstCompleted >= 0)
        // {
        //     menuChapter.UnlockChapter(chapterFirstCompleted + 1);
        // }

    }

    public void OpenOptionsMenu()
    {
        StartCoroutine(OpenOptionsMenuCoroutine());
    }

    private IEnumerator OpenOptionsMenuCoroutine()
    {
        // yield return StartCoroutine(ButtonsDissolveOut());

        optionsMenu.gameObject.SetActive(true);
        chaptersMenu.gameObject.SetActive(false);
        savesMenu.gameObject.SetActive(false);
        startMenu.gameObject.SetActive(false);

        backgroundAnimator.SetBool("fade", true);
        startMenuBackgroundAnimator.SetBool("fade", false);

        optionsMenu.GetComponent<MenuOptions>().OpenOptionsMenu();
        yield return null;
    }

    public void Quit()
    {
        StartCoroutine(QuitCoroutine());
    }

    public IEnumerator QuitCoroutine()
    {
        // yield return StartCoroutine(ButtonsDissolveOut());

        GameObject loadingScreen = (GameObject)Resources.Load("LoadingScreen");
        loadingScreen = Instantiate(loadingScreen, gameObject.transform);
        StartCoroutine(Fade());
        yield return null;
    }

    // IEnumerator ButtonsDissolveIn()
    // {
    //     StartCoroutine(titleDissolve.DissolveIn());
    //     StartCoroutine(playDissolve.DissolveIn());
    //     StartCoroutine(optionsDissolve.DissolveIn());        
    //     yield return StartCoroutine(quitDissolve.DissolveIn());
    // }

    // IEnumerator ButtonsDissolveOut()
    // {
    //     StartCoroutine(titleDissolve.DissolveOut());
    //     StartCoroutine(optionsDissolve.DissolveOut());
    //     StartCoroutine(quitDissolve.DissolveOut());        
    //     yield return StartCoroutine(playDissolve.DissolveOut());
    // }

    // IEnumerator SavesDissolveIn()
    // {
    //     Dissolve ng1 = savesMenu.Find("New Game 1").Find("Rectangle").GetComponent<Dissolve>();
    //     Dissolve ng2 = savesMenu.Find("New Game 2").Find("Rectangle").GetComponent<Dissolve>();
    //     Dissolve ng3 = savesMenu.Find("New Game 3").Find("Rectangle").GetComponent<Dissolve>();
    //     StartCoroutine(ng1.DissolveIn());
    //     StartCoroutine(ng2.DissolveIn());
    //     yield return StartCoroutine(ng3.DissolveIn());
    // }

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