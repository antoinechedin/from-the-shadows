using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuLevels : MonoBehaviour
{
    public GridLayoutGroup buttonsGroup;
    public HorizontalLayoutGroup collectiblesPanel;
    public LevelButton levelButtonPrefab;
    public GameObject collectibleTaken, collectibleMissing; // Prefabs

    private Chapter currentChapter;

    public void SetMenuLevels(int chapterNumber, Chapter chapter)
    {
        currentChapter = chapter;

        int nbCompleted = 0;
        int totalLevels = 0;

        List<Level> levels = chapter.GetLevels();
        foreach (Level l in levels)
        {

            if (l.completed) nbCompleted++;
            totalLevels++;
        }

        SetMenuLevelInfo(0);

        DestroyPreviousButtons();

        for (int i = 0; i < totalLevels; i++)
        {
            int levelNumber = i;
            GameObject button = Instantiate(levelButtonPrefab.gameObject, buttonsGroup.transform);
            button.transform.Find("Text").GetComponent<Text>().text = "" + (i + 1);
            button.GetComponent<Button>().onClick.AddListener(delegate { LevelButtonClicked(chapterNumber, levelNumber); });
            button.GetComponent<LevelButton>().menuLevels = this;
            button.GetComponent<LevelButton>().levelNumber = levelNumber;
            if (levelNumber > 0 && !chapter.GetLevels()[levelNumber - 1].completed)
            {
                button.GetComponent<Button>().interactable = false;
            }
            if (i == 0) EventSystem.current.SetSelectedGameObject(button.gameObject);
        }
    }

    public void SetMenuLevelInfo(int level)
    {
        foreach (Transform child in collectiblesPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (int c in currentChapter.GetLevels()[level].collectibles)
        {
            if (c == 1)
            {
                Instantiate(collectibleTaken, collectiblesPanel.transform);
            }
            else
            {
                Instantiate(collectibleMissing, collectiblesPanel.transform);
            }
        }
    }

    public void DestroyPreviousButtons()
    {
        foreach (Transform child in buttonsGroup.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private static void LevelButtonClicked(int chapterNumber, int levelNumber)
    {
        GameManager.Instance.LoadScene("Leo", levelNumber);
        GameManager.Instance.StartChapterIndex = chapterNumber;
        GameManager.Instance.StartMenuScene = 2;
    }

}
