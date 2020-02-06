using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChapterButton : MonoBehaviour, ISelectHandler
{
    public int chapterNumber;
    public MenuCamera menuCamera;

    public void OnSelect(BaseEventData eventData)
    {
        GameManager.Instance.CurrentChapter = chapterNumber;
        menuCamera.SetChapterSelected(chapterNumber);
    }
    
}
