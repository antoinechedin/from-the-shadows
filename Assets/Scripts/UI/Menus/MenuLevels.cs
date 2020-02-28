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
    public GameObject levelScreenshotsParent;

    [Header("Carousel")]
    [SerializeField]
    private List<LevelScreenshot> screenshots = new List<LevelScreenshot>();
    public float distanceBetweenScreenshots;
    [Range(0.0f, 3.0f)]
    [Tooltip("The minimum size when a level is not selected")]
    public float minSize;
    [Range(0.0f, 3.0f)]
    [Tooltip("The size multiplier of the selected level")]
    public float maxSize;
    [Tooltip("The speed of the carousel")]
    public float speed;
    [SerializeField]
    private int currentSelectedLevel = 0;

    public void SetMenuLevels(int chapterNumber, Chapter chapter)
    {
        ResetScreenshots();
        
        int nbCompleted = 0;
        int totalLevels = 0;

        List<Level> levels = chapter.GetLevels();
        foreach (Level l in levels)
        {

            if (l.Completed) nbCompleted++;
            totalLevels++;
        }

        SetMenuLevelInfo(0);

        int nbLevelSpawned = 0;
        for (int i = 0; i < totalLevels; i++) // Create the levels buttons
        {
            if (levels[i].IsCheckpoint)
            {
                //TODO : aller chercher le bon srceenshot
                GameObject levelScreenshot = Resources.Load("LevelScreenshots/Level0Card") as GameObject;
                LevelScreenshot spawnedScreenshot = Instantiate(levelScreenshot, levelScreenshotsParent.transform).GetComponent<LevelScreenshot>();
                spawnedScreenshot.LevelIndex = i;
                spawnedScreenshot.GetComponent<RectTransform>().localPosition = new Vector3(nbLevelSpawned * distanceBetweenScreenshots, 0, 0);
                screenshots.Add(spawnedScreenshot.GetComponent<LevelScreenshot>());

                nbLevelSpawned++;
            }
        }
    }

    public void SelectNextLevel()
    {
        if (currentSelectedLevel < screenshots.Count - 1)
        {
            foreach (LevelScreenshot go in screenshots)
            {
                go.SetNewDestination(new Vector3(-1000, 0, 0));
            }
            currentSelectedLevel++;
        }

        SetMenuLevelInfo(screenshots[currentSelectedLevel].LevelIndex);
    }

    public void SelectPreviousLevel()
    {
        if (currentSelectedLevel > 0)
        {
            foreach (LevelScreenshot go in screenshots)
            {
                go.SetNewDestination(new Vector3(1000, 0, 0));
            }
            currentSelectedLevel--;
        }

        SetMenuLevelInfo(screenshots[currentSelectedLevel].LevelIndex);
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
    /// Returns a List<KeyValuePair<string, bool>> containing all the collectibles between the current checkpoint and the next check point
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
        } while ( level < ChapterLevels.Count && !ChapterLevels[level].IsCheckpoint);

        return collectibles;
    }

    public void ResetScreenshots()
    {
        foreach (LevelScreenshot child in screenshots)
        {
            GameObject.Destroy(child.gameObject);
        }
        screenshots = new List<LevelScreenshot>();
        currentSelectedLevel = 0;
    }

    private static void LevelButtonClicked(LoadingChapterInfo loadingChapterInfo)
    {
        GameManager.Instance.LoadChapter("Chapter" + GameManager.Instance.CurrentChapter, loadingChapterInfo);
    }
}
