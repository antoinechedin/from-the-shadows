using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuChapter : MonoBehaviour
{
    public MenuCamera menuCamera;
    public MenuLevels menuLevels;
    public Button[] chapterButtons;
    public Text levelLabel;
    public GameObject chapterButtonsPanel;
    public RectTransform thisPanel;
    public RectTransform saveMenu;
    public MenuManager menuManager;
    public Image metaDataPanel;
    public Image metaDataIcon;
    public Image rightArrow;
    public Image leftArrow;

    private List<Chapter> chapters;
    private Animator menuChapterAnimator;
    private Animator metaDataPanelAnimator;
    private bool chapterMenuIsOpen = false;
    private bool statsOpen = false;

    private List<string> chaptersName;

    void Awake()
    {
        chaptersName = new List<string>(new string[] {
            "CHAPTER 0",
            "CHAPTER 1",
            "CHAPTER 2"
        });
    }

    void Start()
    {
        menuChapterAnimator = gameObject.GetComponent<Animator>();
        metaDataPanelAnimator = metaDataPanel.gameObject.GetComponent<Animator>();
        // levelLabel.text = chaptersName[localIndexCurrentChapter].ToUpper();
    }

    void Update()
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
                menuLevels.ResetScreenshots();
                menuLevels.enabled = false;
                chapterMenuIsOpen = false;
                chapterButtonsPanel.SetActive(true);
                metaDataIcon.gameObject.SetActive(true);
                EventSystem.current.SetSelectedGameObject(chapterButtons[GameManager.Instance.CurrentChapter].gameObject);
                if (menuChapterAnimator != null)
                {
                    menuChapterAnimator.SetBool("open", false);
                }
                menuCamera.SetZoom(false);
            }
            else if (!chapterMenuIsOpen)
            {
                menuCamera.SetReturnToSavesMenu(true);
                menuManager.OpenSaveMenu();
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

    public void DisplayStatistics()
    {
        statsOpen = !statsOpen;
        EventSystem.current.SetSelectedGameObject(null);
        chapterButtonsPanel.SetActive(!chapterButtonsPanel.activeSelf);
        menuCamera.cursor.gameObject.SetActive(!menuCamera.cursor.gameObject.activeSelf);
        metaDataIcon.gameObject.SetActive(!metaDataIcon.gameObject.activeSelf);
        EventSystem.current.SetSelectedGameObject(chapterButtons[GameManager.Instance.CurrentChapter].gameObject);
        metaDataPanelAnimator.SetBool("open", !metaDataPanelAnimator.GetBool("open"));
    }

    public void OpenChapterMenu(int chapterId)
    {
        GameManager.Instance.CurrentChapter = chapterId;
        Debug.Log("Open chapter: " + chapterId);
        Debug.Log("Current chapter: " + GameManager.Instance.CurrentChapter);
        menuCamera.SetChapterSelected(chapterId);

        if (!chapterMenuIsOpen)
        {
            menuLevels.enabled = true;
            chapterMenuIsOpen = true;
            //chapterButtonsPanel.SetActive(false);
            metaDataIcon.gameObject.SetActive(false);
            if (menuChapterAnimator != null)
            {
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
                menuChapterAnimator.SetBool("open", true);
                menuCamera.SetZoom(true);
                menuLevels.SetMenuLevels(GameManager.Instance.CurrentChapter);
            }
        }
    }

    public void UpdateChapterName(int chapterNumber)
    {
        levelLabel.text = chaptersName[chapterNumber].ToUpper();
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
        while (i < Mathf.Min(chapters.Count, chapterButtons.Length) && chapters[i - 1].isCompleted())
        {
            chapterButtons[i].interactable = true;
            lastUnlockedChapterId = i;
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
        UpdateChapterName(chapterUnlocked);
        yield return new WaitForSeconds(0.25f);
        menuCamera.UnlockAnimation(true);
        yield return new WaitForSeconds(3f);
        EventSystem.current.SetSelectedGameObject(chapterButtons[chapterUnlocked].gameObject);
    }

}
