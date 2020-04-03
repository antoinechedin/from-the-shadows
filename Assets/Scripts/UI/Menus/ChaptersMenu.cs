using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ChaptersMenu : MonoBehaviour, IDissolveMenu
{
    [HideInInspector] public MenuManager menuManager;


    public MenuCamera menuCamera;
    public Carousel carousel;
    public Button[] chapterButtons;
    public GameObject chapterButtonsPanel;
    public RectTransform thisPanel;
    public RectTransform saveMenu;
    public Image metaDataPanel;
    public Image metaDataIcon;

    private List<Chapter> chapters;
    private bool chapterMenuIsOpen = false;
    private bool statsOpen = false;

    private List<string> chaptersName;

    void Awake()
    {
        chaptersName = new List<string>(new string[] {
            "Prologue",
            "Chapter 1",
            "Chapter 2"
        });
    }

    void Start()
    {
        // metaDataPanelAnimator = metaDataPanel.gameObject.GetComponent<Animator>();
        // levelLabel.text = chaptersName[localIndexCurrentChapter].ToUpper();
    }

    void Update()
    {
        if (EventSystem.current.sendNavigationEvents)
        {
            // Cancel
            if (Input.GetButtonDown("B_G"))
            {
                if (statsOpen)
                {
                    DisplayStatistics();
                }
                // Close the chapter
                else if (chapterMenuIsOpen)
                {
                    carousel.ResetScreenshots();
                    carousel.enabled = false;
                    chapterMenuIsOpen = false;
                    chapterButtonsPanel.SetActive(true);
                    metaDataIcon.gameObject.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(chapterButtons[GameManager.Instance.CurrentChapter].gameObject);
                    carousel.animator.SetBool("open", false);
                    menuCamera.SetZoom(false);
                }
                else if (!chapterMenuIsOpen)
                {
                    menuCamera.SetReturnToSavesMenu(true);
                    // menuManager.OpenSaveMenu();
                    Debug.Log("chapterMenu closed");
                    menuManager.DissolveFromMenuToMenu(menuManager.chaptersMenu, menuManager.savesMenu);
                }
            }

            if (Input.GetButtonDown("Start_G") && !chapterMenuIsOpen)
            {
                DisplayStatistics();
            }

            // leftArrow.GetComponent<Image>().color = new Color(255, 255, 255, 1);
            // rightArrow.GetComponent<Image>().color = new Color(255, 255, 255, 1);

            // if (localIndexCurrentChapter == 0)
            // {
            //     leftArrow.GetComponent<Image>().color = new Color(255, 255, 255, 0);
            // }
            // if (localIndexCurrentChapter >= chaptersName.Count - 1)
            // {
            //     rightArrow.GetComponent<Image>().color = new Color(255, 255, 255, 0);
            // }
            // else if (!chapterButtons[localIndexCurrentChapter + 1].interactable)
            // {
            //     rightArrow.GetComponent<Image>().color = new Color(255, 255, 255, 0);
            // }
        }
    }

    public void DisplayStatistics()
    {
        statsOpen = !statsOpen;
        EventSystem.current.SetSelectedGameObject(null);
        chapterButtonsPanel.SetActive(!chapterButtonsPanel.activeSelf);
        menuCamera.cursor.gameObject.SetActive(!menuCamera.cursor.gameObject.activeSelf);
        metaDataIcon.gameObject.SetActive(!metaDataIcon.gameObject.activeSelf);
        EventSystem.current.SetSelectedGameObject(chapterButtons[GameManager.Instance.CurrentChapter].gameObject);
        // metaDataPanelAnimator.SetBool("open", !metaDataPanelAnimator.GetBool("open"));
    }

    public void OpenChapterMenu(int chapterId)
    {
        GameManager.Instance.CurrentChapter = chapterId;
        Debug.Log("Open chapter: " + chapterId);
        Debug.Log("Current chapter: " + GameManager.Instance.CurrentChapter);
        menuCamera.SetChapterSelected(chapterId);

        if (!chapterMenuIsOpen)
        {
            carousel.enabled = true;
            chapterMenuIsOpen = true;
            //chapterButtonsPanel.SetActive(false);
            metaDataIcon.gameObject.SetActive(false);

            int nbLightCollectibleTaken = 0;
            int nbShadowCollectibleTaken = 0;

            int totalNbLightCollectible = 0;
            int totalNbShadowCollectible = 0;

            int nbCompleted = 0;
            int totalLevel = 0;

            List<Level> levels = chapters[GameManager.Instance.CurrentChapter].GetLevels();
            foreach (Level l in levels)
            {
                //Light collectibles
                foreach (bool collectible in l.LightCollectibles)
                {
                    if (collectible == true) nbLightCollectibleTaken++;
                }
                totalNbLightCollectible += l.LightCollectibles.Length;

                //shadow collectibles
                foreach (bool collectible in l.ShadowCollectibles)
                {
                    if (collectible == true) nbShadowCollectibleTaken++;
                }
                totalNbShadowCollectible += l.ShadowCollectibles.Length;
                if (l.Completed) nbCompleted++;
                totalLevel++;

            }

            //                levelLabel.text = chaptersName[localIndexCurrentChapter].ToUpper();
            Debug.Log(carousel.animator);
            carousel.animator.SetBool("open", true);
            menuCamera.SetZoom(true);
            carousel.SetMenuLevels(GameManager.Instance.CurrentChapter);

        }
    }

    public void ResetInteractablesChaptersButtons()
    {
        if (chapterButtons.Length == 0)
        {
            Debug.LogError("ERROR MenuChaputer.ResetInteractablesChaptersButtons(): There's no chapter buttons");
            return;
        }


        int lastUnlockedChapterId = 0;
        chapterButtons[0].interactable = true;
        chapters = GameManager.Instance.GetChapters();

        int i = 1;
        while (i < Mathf.Min(chapters.Count, chapterButtons.Length))
        {
            if (chapters[i - 1].isCompleted())
            {
                chapterButtons[i].interactable = true;
                chapterButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = chaptersName[i];
                lastUnlockedChapterId = i;
            }
            else
            {
                chapterButtons[i].interactable = false;
                chapterButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "...";
            }
            i++;
        }

        if (lastUnlockedChapterId == 0)
        {
            Navigation explicitNav = new Navigation();
            explicitNav.mode = Navigation.Mode.Explicit;
            chapterButtons[0].navigation = explicitNav;
        }
        else
        {
            for (i = 0; i < Mathf.Min(chapters.Count, chapterButtons.Length); i++)
            {
                Navigation explicitNav = new Navigation();
                explicitNav.mode = Navigation.Mode.Explicit;
                if (i <= lastUnlockedChapterId)
                {
                    explicitNav.selectOnUp = chapterButtons[Utils.Modulo(i - 1, lastUnlockedChapterId + 1)];
                    explicitNav.selectOnDown = chapterButtons[Utils.Modulo(i + 1, lastUnlockedChapterId + 1)];

                }
                chapterButtons[i].navigation = explicitNav;
            }
        }
    }

    public void UnlockChapter(int chapterUnlocked)
    {
        IEnumerator coroutine = UnlockChapterCoroutine(chapterUnlocked);
        StartCoroutine(coroutine);
    }

    private IEnumerator UnlockChapterCoroutine(int chapterUnlocked)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForSeconds(0.5f);
        menuCamera.SetChapterSelected(chapterUnlocked);
        GameManager.Instance.CurrentChapter = chapterUnlocked;
        // UpdateChapterName(chapterUnlocked);
        yield return new WaitForSeconds(0.25f);
        menuCamera.UnlockAnimation(true);
        yield return new WaitForSeconds(3f);
        EventSystem.current.SetSelectedGameObject(chapterButtons[chapterUnlocked].gameObject);
    }

    public IEnumerator DissolveInCoroutine()
    {
        gameObject.SetActive(true);
        ResetInteractablesChaptersButtons();

        EventSystem.current.SetSelectedGameObject(chapterButtons[0].gameObject);
        DissolveController[] dissolves = GetComponentsInChildren<DissolveController>();
        for (int i = 0; i < dissolves.Length - 1; i++)
        {
            StartCoroutine(dissolves[i].DissolveInCoroutine(menuManager.dissolveDuration));
            yield return new WaitForSeconds(menuManager.dissolveOffset);
        }

        EventSystem.current.sendNavigationEvents = true;

        yield return StartCoroutine(dissolves[dissolves.Length - 1].DissolveInCoroutine(menuManager.dissolveDuration));
    }

    public IEnumerator DissolveOutCoroutine()
    {
        EventSystem.current.sendNavigationEvents = false;

        DissolveController[] dissolves = GetComponentsInChildren<DissolveController>();
        for (int i = 0; i < dissolves.Length - 1; i++)
        {
            StartCoroutine(dissolves[i].DissolveOutCoroutine(menuManager.dissolveDuration));
            yield return new WaitForSeconds(menuManager.dissolveOffset);
        }

        yield return StartCoroutine(dissolves[dissolves.Length - 1].DissolveOutCoroutine(menuManager.dissolveDuration));
        gameObject.SetActive(false);
    }
}
