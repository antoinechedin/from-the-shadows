using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GMDebugPanel : MonoBehaviour
{
    private TextMeshProUGUI saveInfos;
    private string saveInfosTemplate;

    private void Awake()
    {
        saveInfos = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (saveInfos != null) saveInfosTemplate = saveInfos.text;
    }

    private void Update()
    {
        if (saveInfos != null)
        {
            string saveString = "";
            int currentSaveId = GameManager.Instance.CurrentSave;
            Save currentSave = currentSaveId < 0 ? null : GameManager.Instance.Saves[currentSaveId];
            int currentChapterId = GameManager.Instance.CurrentChapter;
            Chapter currentChapter = currentChapterId < 0 ? null : currentSave.Chapters[currentChapterId];

            for (int i = 0; i < GameManager.Instance.Saves.Length; i++)
            {
                saveString +=
                     "- Save" + i + ": "
                     + (GameManager.Instance.Saves[i] == null ? "null" : GameManager.Instance.Saves[i].ToString())
                     + (i != GameManager.Instance.Saves.Length - 1 ? "\n" : "");
            }

            saveInfos.text = string.Format(
                saveInfosTemplate,
                GameManager.Instance.Saves == null ? "null" : GameManager.Instance.Saves.Length.ToString(),
                saveString,
                currentSaveId,
                currentSave == null ? "null" : currentSave.Chapters.Count.ToString(),
                currentChapterId,
                currentChapter == null ? "null" : currentChapter.GetNbLevels().ToString(),
                GameManager.Instance.LoadingMenuInfos,
                GameManager.Instance.LoadingChapterInfo,
                GameManager.Instance.Loading ? "X" : " "
            );
        }
    }
}
