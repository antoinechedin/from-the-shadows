using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuChapter : MonoBehaviour
{
    public MenuCamera menuCamera;
    public MenuLevels menuLevels;
    public List<Button> chapterButtons;
    public Text levelLabel;
    public Text collectiblesNumber;
    public Text completedNumber;
    public GameObject chapterButtonsPanel;
    public Canvas canvas;
    public Canvas saveMenu;
    public MenuManager menuManager;

    private List<Chapter> chapters;
    private int currentChapter;
    private Animator menuChapterAnimator;
    private Animator menuLevelAnimator;
    private bool chapterMenuIsOpen = false;
    private bool levelMenuIsOpen = false;

    private List<string> chaptersName;

    void Awake()
    {
        chaptersName = new List<string>(new string[] {
            "Chapter 1",
            "Chapter 2",
            "Chapter 3",
            "Chapter 4",
            "Chapter 5"
        });
    }

    void Start()
    {
        chapters = GameManager.Instance.GetChapters();
        currentChapter = GameManager.Instance.StartChapterIndex;
        menuChapterAnimator = gameObject.GetComponent<Animator>();
        menuLevelAnimator = menuLevels.GetComponent<Animator>();
        for (int i = 0; i < chapters.Count; i++)
        {
            if (!chapters[i].GetLevels()[chapters[i].GetNbLevels() - 1].completed)
            {
                for (int j = i + 1; j < chapterButtons.Count; j++)
                {
                    chapterButtons[j].interactable = false;
                }
                break;
            }
        }
    }

    void Update()
    {
        // Open the chapter
        if (Input.GetButtonDown("A_G"))
        {
            if (!chapterMenuIsOpen && !levelMenuIsOpen)
            {
                chapterMenuIsOpen = true;
                chapterButtonsPanel.SetActive(false);
                if (menuChapterAnimator != null)
                {
                    int nbCollectibleTaken = 0;
                    int totalNbCollectible = 0;
                    int nbCompleted = 0;
                    int totalLevel = 0;

                    List<Level> levels = chapters[currentChapter].GetLevels();
                    foreach (Level l in levels)
                    {
                        foreach (int collectible in l.collectibles)
                        {
                            if (collectible == 1) nbCollectibleTaken++;
                        }
                        totalNbCollectible += l.nbCollectible;
                        if (l.completed) nbCompleted++;
                        totalLevel++;
                    }

                    levelLabel.text = chaptersName[currentChapter];
                    collectiblesNumber.text = nbCollectibleTaken + "/" + totalNbCollectible;
                    completedNumber.text = nbCompleted + "/" + totalLevel;
                    menuChapterAnimator.SetBool("open", true);
                    menuCamera.SetZoom(true);
                }
            }
            // Open the level and close the chapter
            else if (chapterMenuIsOpen && !levelMenuIsOpen)
            {
                chapterMenuIsOpen = false;
                menuChapterAnimator.SetBool("open", false);
                levelMenuIsOpen = true;
                if (menuChapterAnimator != null)
                {
                    menuLevels.SetMenuLevels(currentChapter, chapters[currentChapter]);
                    menuLevelAnimator.SetBool("open", true);
                }
            }
        }

        // Cancel
        if (Input.GetButtonDown("B_G"))
        {
            // Close the chapter
            if (chapterMenuIsOpen && !levelMenuIsOpen)
            {
                chapterMenuIsOpen = false;
                chapterButtonsPanel.SetActive(true);
                EventSystem.current.SetSelectedGameObject(chapterButtons[currentChapter].gameObject);
                if (menuChapterAnimator != null)
                {
                    menuChapterAnimator.SetBool("open", false);
                }
                menuCamera.SetZoom(false);
            }
            // Close the level and open the chapter
            else if (!chapterMenuIsOpen && levelMenuIsOpen)
            {
                chapterMenuIsOpen = true;
                levelMenuIsOpen = false;
                menuLevels.DestroyPreviousButtons();
                if (menuLevelAnimator != null)
                {
                    menuLevelAnimator.SetBool("open", false);
                }
                if (menuChapterAnimator != null)
                {
                    menuChapterAnimator.SetBool("open", true);
                }
            }
            else if (!chapterMenuIsOpen && !levelMenuIsOpen)
            {
                menuCamera.SetReturnToMainMenu(true);
                gameObject.transform.position += new Vector3(455, 0, 0);
                menuLevels.gameObject.transform.position += new Vector3(455, 0, 0);
                menuManager.OpenSaveMenu();
            }
        }
    }

    /// <summary>
    /// Set the number of the current chapter
    /// </summary>
    /// <param name="number">The new current chapter</param>
    public void SetCurrentChapter(int number)
    {
        currentChapter = number;
    }
}
