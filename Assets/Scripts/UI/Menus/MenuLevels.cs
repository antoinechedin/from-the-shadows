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
    public GameObject collectibleLight, collectibleShadow, collectibleMissing; // Prefabs

    public void SetMenuLevels(int chapterNumber, Chapter chapter)
    {
        int nbCompleted = 0;
        int totalLevels = 0;

        List<Level> levels = chapter.GetLevels();
        foreach (Level l in levels)
        {

            if (l.Completed) nbCompleted++;
            totalLevels++;
        }

        //SetMenuLevelInfo(0);

        DestroyPreviousButtons();

        for (int i = 0; i < totalLevels; i++) // Create the levels buttons
        {
            int levelNumber = i;
            if (levels[i].IsCheckpoint)
            {
                GameObject button = Instantiate(levelButtonPrefab.gameObject, buttonsGroup.transform);
                button.transform.Find("Text").GetComponent<Text>().text = "" + (i + 1);
                button.GetComponent<Button>().onClick.AddListener(delegate
                {
                    LevelButtonClicked(new LoadingChapterInfo(levelNumber));
                });
                button.GetComponent<LevelButton>().menuLevels = this;
                button.GetComponent<LevelButton>().levelNumber = levelNumber;
                if (levelNumber > 0 && !chapter.GetLevels()[levelNumber - 1].Completed)
                {
                    button.GetComponent<Button>().interactable = false;
                }
                if (i == 0) EventSystem.current.SetSelectedGameObject(button.gameObject);
            }
        }
    }

    public void SetMenuLevelInfo(int level)
    {
        Chapter localCurrentChapter = GameManager.Instance.GetChapters()[GameManager.Instance.CurrentChapter];
        foreach (Transform child in collectiblesPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        List<KeyValuePair<string, bool>> collectiblesTaken = GetCollectibleToNextCheckPoint(level);

        foreach (KeyValuePair<string, bool> kv in collectiblesTaken)
        {
            if (kv.Key == "light" && kv.Value)
            {
                Instantiate(collectibleLight, collectiblesPanel.transform);
            }
            else if (kv.Key == "shadow" && kv.Value)
            {
                Instantiate(collectibleShadow, collectiblesPanel.transform);
            }
            else
            {
                Instantiate(collectibleMissing, collectiblesPanel.transform);
            }
        }
    }

    /// <summary>
    /// Returns a Dictionary<string, bool> containing all the collectibles between the current checkpoint and the next check point
    /// </summary>
    /// <returns></returns>
    public List<KeyValuePair<string, bool>> GetCollectibleToNextCheckPoint(int level)
    {
        List<KeyValuePair<string, bool>> collectibles = new List<KeyValuePair<string, bool>>();
        List<Level> ChapterLevels = GameManager.Instance.GetChapters()[GameManager.Instance.CurrentChapter].GetLevels();

        //pour chaque tableau jusqu'au prochain checkpoint
        do
        {
            //tout les colletibles de lumière du tableau
            bool[] levelLightCollectibles = ChapterLevels[level].LightCollectibles;
            foreach (bool b in levelLightCollectibles)
            {
                collectibles.Add(new KeyValuePair<string, bool>("light", b));
            }

            //tout les colletibles d'ombre du tableau
            bool[] levelShadowtCollectibles = ChapterLevels[level].ShadowCollectibles;
            foreach (bool b in levelShadowtCollectibles)
            {
                collectibles.Add(new KeyValuePair<string, bool>("shadow", b));
            }
            level++;
        } while (!ChapterLevels[level].IsCheckpoint);

        return collectibles;
    }

    public void DestroyPreviousButtons()
    {
        foreach (Transform child in buttonsGroup.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private static void LevelButtonClicked(LoadingChapterInfo loadingChapterInfo)
    {
        GameManager.Instance.LoadChapter("Chapter" + GameManager.Instance.CurrentChapter, loadingChapterInfo);
    }
}
