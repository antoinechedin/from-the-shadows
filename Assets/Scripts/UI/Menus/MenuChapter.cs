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
    private Animator menuChapterAnimator;
    private bool chapterMenuIsOpen = false;

    private List<string> chaptersName;

    void Awake()
    {
        chaptersName = new List<string>(new string[] {
            "Chapter 0",
            "Chapter 1",
            "Chapter 2",
            "Chapter 3",
            "Chapter 4"
        });
    }

    void Start()
    {
        menuChapterAnimator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        // Cancel
        if (Input.GetButtonDown("B_G"))
        {
            int localIndexCurrentChapter = GameManager.Instance.CurrentChapter;
            // Close the chapter
            if (chapterMenuIsOpen)
            {
                chapterMenuIsOpen = false;
                chapterButtonsPanel.SetActive(true);
                EventSystem.current.SetSelectedGameObject(chapterButtons[localIndexCurrentChapter].gameObject);
                if (menuChapterAnimator != null)
                {
                    menuChapterAnimator.SetBool("open", false);
                }
                menuCamera.SetZoom(false);
                menuLevels.DestroyPreviousButtons();
            }
            else if (!chapterMenuIsOpen)
            {
                menuCamera.SetReturnToMainMenu(true);
                gameObject.transform.position += new Vector3(605, 0, 0);
                menuManager.OpenSaveMenu();
            }
        }
    }

    public void OpenChapterMenu()
    {
        int localIndexCurrentChapter = GameManager.Instance.CurrentChapter;
        if (!chapterMenuIsOpen)
        {
            chapterMenuIsOpen = true;
            chapterButtonsPanel.SetActive(false);
            if (menuChapterAnimator != null)
            {
                int nbCollectibleTaken = 0;
                int totalNbCollectible = 0;
                int nbCompleted = 0;
                int totalLevel = 0;

                List<Level> levels = chapters[localIndexCurrentChapter].GetLevels();
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

                levelLabel.text = chaptersName[localIndexCurrentChapter];
                collectiblesNumber.text = nbCollectibleTaken + "/" + totalNbCollectible;
                completedNumber.text = nbCompleted + "/" + totalLevel;
                menuChapterAnimator.SetBool("open", true);
                menuCamera.SetZoom(true);
                GameManager.Instance.CurrentChapter = localIndexCurrentChapter;
                menuLevels.SetMenuLevels(localIndexCurrentChapter, chapters[localIndexCurrentChapter]);
            }
        }
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

}
