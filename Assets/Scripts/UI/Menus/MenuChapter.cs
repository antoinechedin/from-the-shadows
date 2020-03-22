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
    public List<Button> chapterButtons;
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
    private int localIndexCurrentChapter;

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
        localIndexCurrentChapter = GameManager.Instance.CurrentChapter;
        levelLabel.text = chaptersName[localIndexCurrentChapter].ToUpper();
    }

    void Update()
    {
        localIndexCurrentChapter = GameManager.Instance.CurrentChapter;
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
                EventSystem.current.SetSelectedGameObject(chapterButtons[localIndexCurrentChapter].gameObject);
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

        leftArrow.GetComponent<Image>().color = new Color(255, 255, 255, 1);
        rightArrow.GetComponent<Image>().color = new Color(255, 255, 255, 1);

        if (localIndexCurrentChapter == 0)
        {
            leftArrow.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
        if (localIndexCurrentChapter >= chaptersName.Count - 1)
        {
            rightArrow.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
        else if (!chapterButtons[localIndexCurrentChapter + 1].interactable)
        {
            rightArrow.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
    }

    public void DisplayStatistics()
    {
        statsOpen = !statsOpen;
        EventSystem.current.SetSelectedGameObject(null);
        chapterButtonsPanel.SetActive(!chapterButtonsPanel.activeSelf);
        menuCamera.cursor.gameObject.SetActive(!menuCamera.cursor.gameObject.activeSelf);
        metaDataIcon.gameObject.SetActive(!metaDataIcon.gameObject.activeSelf);
        EventSystem.current.SetSelectedGameObject(chapterButtons[localIndexCurrentChapter].gameObject);
        metaDataPanelAnimator.SetBool("open", !metaDataPanelAnimator.GetBool("open"));
    }

    public void OpenChapterMenu()
    {
        int localIndexCurrentChapter = GameManager.Instance.CurrentChapter;
        if (!chapterMenuIsOpen)
        {
            menuLevels.enabled = true;
            chapterMenuIsOpen = true;
            chapterButtonsPanel.SetActive(false);
            metaDataIcon.gameObject.SetActive(false);
            if (menuChapterAnimator != null)
            {
                int nbLightCollectibleTaken = 0;
                int nbShadowCollectibleTaken = 0;

                int totalNbLightCollectible = 0;
                int totalNbShadowCollectible = 0;

                int nbCompleted = 0;
                int totalLevel = 0;

                List<Level> levels = chapters[localIndexCurrentChapter].GetLevels();
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

                levelLabel.text = chaptersName[localIndexCurrentChapter].ToUpper();
                menuChapterAnimator.SetBool("open", true);
                menuCamera.SetZoom(true);
                GameManager.Instance.CurrentChapter = localIndexCurrentChapter;
                menuLevels.SetMenuLevels(localIndexCurrentChapter, chapters[localIndexCurrentChapter]);
            }
        }
    }

    public void UpdateChapterName(int chapterNumber)
    {
        levelLabel.text = chaptersName[chapterNumber].ToUpper();
    }

    public void ResetInteractablesChaptersButtons()
    {
        chapters = GameManager.Instance.GetChapters();
        for (int i = 1; i < chapterButtons.Count; i++)
        {
            chapterButtons[i].interactable = false;
        }
        for (int i = 0; i < chapters.Count - 1; i++)
        {
            if (chapters[i].isCompleted())
            {
                chapterButtons[i + 1].interactable = true;
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
