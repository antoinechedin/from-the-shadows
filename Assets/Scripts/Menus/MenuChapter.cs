using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuChapter : MonoBehaviour
{
    public MenuLevels menuLevels;
    public List<UnityEngine.UI.Button> chapterButtons;
    public UnityEngine.UI.Text levelLabel;
    public UnityEngine.UI.Text collectiblesNumber;
    public UnityEngine.UI.Text completedNumber;
    public GameObject chapterButtonsPanel;

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

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.LoadSaveFile();
        chapters = GameManager.Instance.GetChapters();
        currentChapter = 0; // GameManager.Instance.GetCurrentChapter();
        EventSystem.current.SetSelectedGameObject(chapterButtons[0].gameObject);
        menuChapterAnimator = gameObject.GetComponent<Animator>();
        menuLevelAnimator = menuLevels.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Open the chapter
        if (Input.GetButtonDown("Jump"))
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
                        nbCollectibleTaken += l.nbCollectibleTaken;
                        totalNbCollectible += l.totalNbCollectible;
                        if (l.completed) nbCompleted++;
                        totalLevel++;
                    }

                    levelLabel.text = chaptersName[currentChapter];
                    collectiblesNumber.text = nbCollectibleTaken + "/" + totalNbCollectible;
                    completedNumber.text = nbCompleted + "/" + totalLevel;
                    menuChapterAnimator.SetBool("open", true);
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
                    int nbCompleted = 0;
                    int totalLevel = 0;

                    List<Level> levels = chapters[currentChapter].GetLevels();
                    foreach (Level l in levels)
                    {
                        if (l.completed) nbCompleted++;
                        totalLevel++;
                    }

                    menuLevels.SetMenuLevels(nbCompleted, totalLevel);
                    menuLevelAnimator.SetBool("open", true);
                }
            }
        }

        // Cancel
        if (Input.GetButtonDown("Return"))
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
            }
            // Close the level and open the chapter
            else if (!chapterMenuIsOpen && levelMenuIsOpen)
            {
                chapterMenuIsOpen = true;
                levelMenuIsOpen = false;
                if (menuLevelAnimator != null)
                {
                    menuLevelAnimator.SetBool("open", false);
                }
                if (menuChapterAnimator != null)
                {
                    menuChapterAnimator.SetBool("open", true);
                }
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
