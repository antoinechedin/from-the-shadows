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
    public MainMenu mainMenu;
    public SavesMenu savesMenu;
    public OptionsMenu optionsMenu;
    public CreditsMenu creditsMenu;
    public ChaptersMenu chaptersMenu;

    public MenuCamera menuCamera;

    public Button play;
    // public Button options;
    // public Button quit;
    public Button firstSave;

    public Image background;
    public Image startMenuBackground;
    public TextMeshProUGUI version;

    public const float dissolveDuration = 0.5f;
    public const float dissolveOffset = 0.1f;

    private Animator backgroundAnimator;
    private Animator startMenuBackgroundAnimator;

    // private Dissolve titleDissolve;
    // private Dissolve playDissolve;
    // private Dissolve optionsDissolve;
    // private Dissolve quitDissolve;

    private void Awake()
    {
        mainMenu.menuManager = this;
        savesMenu.menuManager = this;
        optionsMenu.menuManager = this;
        creditsMenu.menuManager = this;
        chaptersMenu.menuManager = this;
    }

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

        DiscordController.Instance.Init();

        // backgroundAnimator = background.gameObject.GetComponent<Animator>();
        startMenuBackgroundAnimator = startMenuBackground.gameObject.GetComponent<Animator>();

        int sceneIndex = GameManager.Instance.LoadingMenuInfos.StartingMenuScene;
        int finishChapterForFirstTime = GameManager.Instance.LoadingMenuInfos.FinishChapterForFirstTime;
        switch (sceneIndex)
        {
            case 0: // Start menu
                    // StartCoroutine(OpenStartMenuCoroutine());
                DissolveFromMenuToMenu(null, mainMenu);

                break;
            case 1: // Saves menu
                    // StartCoroutine(OpenSaveMenuCoroutine());
                DissolveFromMenuToMenu(null, savesMenu);
                break;
            case 2: // Chapters menu
                if (GameManager.Instance.CurrentChapter != -1)
                {
                    DissolveFromMenuToMenu(null, chaptersMenu);
                    // OpenChaptersMenu(GameManager.Instance.CurrentChapter, finishChapterForFirstTime);
                }
                else
                {
                    Debug.LogWarning("WARN MenuManager.Start: CurrentSave not set. Opening at chapter");
                    DissolveFromMenuToMenu(null, chaptersMenu);
                    // OpenChaptersMenu(0, -1);
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

    public void DissolveFromMenuToMenu(IDissolveMenu from, IDissolveMenu to)
    {
        StartCoroutine(DissolveFromMenuToMenuCoroutine(from, to));
    }

    public IEnumerator DissolveFromMenuToMenuCoroutine(IDissolveMenu from, IDissolveMenu to)
    {
        if (from != null) yield return StartCoroutine(from.DissolveOutCoroutine());
        if (to != null) yield return StartCoroutine(to.DissolveInCoroutine());
    }

    public void SaveOptions()
    {
        DissolveFromMenuToMenu(optionsMenu, mainMenu);
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