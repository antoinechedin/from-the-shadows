using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Coffee.UIExtensions;
using UnityEngine.Video;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public VideoPlayer introCinematic;
    public CinematicMenu cinematicMenu;
    public GameObject startMenu;
    public MainMenu mainMenu;
    public SavesMenu savesMenu;
    public OptionsMenu optionsMenu;
    public CreditsMenu creditsMenu;
    public ChaptersMenu chaptersMenu;
    public GameObject footer;
    public MusicManager musicManager;

    public MenuCamera menuCamera;

    public Button play;
    // public Button options;
    // public Button quit;
    public Button firstSave;

    public Image background;
    public Image startMenuBackground;
    public TextMeshProUGUI version;

    public AudioClip uiPress;
    public AudioClip uiSelect;

    public const float dissolveDuration = 0.5f;
    public const float dissolveOffset = 0.1f;

    private Animator backgroundAnimator;
    // private Animator startMenuBackgroundAnimator;

    // private Dissolve titleDissolve;
    // private Dissolve playDissolve;
    // private Dissolve optionsDissolve;
    // private Dissolve quitDissolve;

    private Coroutine introCinematicCoroutine;

    private void Awake()
    {
        mainMenu.menuManager = this;
        savesMenu.menuManager = this;
        optionsMenu.menuManager = this;
        creditsMenu.menuManager = this;
        chaptersMenu.menuManager = this;

        version.text = Application.version + "\n2020 © Gamagora";
        Time.timeScale = 1;
    }

    void Start()
    {
        // titleDissolve = startMenu.Find("Menu").Find("Image").GetComponent<Dissolve>();
        // playDissolve = play.GetComponentInChildren<Dissolve>();
        // optionsDissolve = options.GetComponentInChildren<Dissolve>();
        // quitDissolve = quit.GetComponentInChildren<Dissolve>();

        SaveManager.Instance.LoadAllSaveFiles();
        DiscordController.Instance.Init();
        cinematicMenu.gameObject.SetActive(true);
        cinematicMenu.Init();

        // play.onClick.AddListener(delegate { StartCoroutine(OpenSaveMenuCoroutine()); });
        // options.onClick.AddListener(delegate { StartCoroutine(OpenOptionsMenuCoroutine()); });
        // quit.onClick.AddListener(delegate { StartCoroutine(QuitCoroutine()); });

        if (GameManager.Instance.LoadingMenuInfos == null)
        {
            Debug.Log("MenuManager.Start: no loading menu infos in game manager. Start intro cinematic");
            introCinematicCoroutine = StartCoroutine(StartIntroCinematic());
            startMenu.SetActive(false);
        }

        // backgroundAnimator = background.gameObject.GetComponent<Animator>();
        // startMenuBackgroundAnimator = startMenuBackground.gameObject.GetComponent<Animator>();
        else
        {
            DisplayMenu();
        }
    }

    private IEnumerator StartIntroCinematic()
    {
        GameManager.Instance.LoadingMenuInfos = new LoadingMenuInfo(0);

        footer.SetActive(false);
        introCinematic.gameObject.SetActive(true);

        introCinematic.Prepare();
        while (!introCinematic.isPrepared) yield return null;
        introCinematic.Play();
        yield return new WaitForSeconds(0.1f);
        cinematicMenu.SetForegroundAlpha(0);
        yield return new WaitForSeconds((float)introCinematic.length);

        cinematicMenu.SetForegroundAlpha(1);
        introCinematicCoroutine = null;
        introCinematic.Stop();
        DisplayMenu();
    }

    private void DisplayMenu()
    {
        footer.SetActive(true);
        introCinematic.gameObject.SetActive(false);
        startMenu.SetActive(true);
        StartCoroutine(cinematicMenu.FadeOutCinematicMenuCoroutine());
        musicManager.StartTheme(musicManager.mainTheme);

        int sceneIndex = GameManager.Instance.LoadingMenuInfos.StartingMenuScene;
        int finishChapterForFirstTime = GameManager.Instance.LoadingMenuInfos.FinishChapterForFirstTime;
        switch (sceneIndex)
        {
            case 0: // Start menu
                DissolveFromMenuToMenu(null, mainMenu);
                break;
            case 1: // Saves menu
                DissolveFromMenuToMenu(null, savesMenu);
                break;
            case 2: // Chapters menu
                if (GameManager.Instance.CurrentChapter != -1)
                {
                    menuCamera.SmoothTransition = false;
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
        if (introCinematicCoroutine != null && Input.anyKeyDown)
        {
            if (!cinematicMenu.canSkip)
            {
                cinematicMenu.ShowSkipText();
            }
            else
            {
                Debug.Log("Stop cinematic");
                StopCoroutine(introCinematicCoroutine);
                introCinematicCoroutine = null;
                cinematicMenu.SetForegroundAlpha(1);
                introCinematic.Stop();
                DisplayMenu();
            }
        }
    }

    public void DissolveFromMenuToMenu(IDissolveMenu from, IDissolveMenu to)
    {
        StartCoroutine(DissolveFromMenuToMenuCoroutine(from, to));
    }

    public IEnumerator DissolveFromMenuToMenuCoroutine(IDissolveMenu from, IDissolveMenu to)
    {
        if (from != null) yield return StartCoroutine(from.DissolveOutCoroutine());
        else yield return new WaitForSecondsRealtime(0.3f);
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