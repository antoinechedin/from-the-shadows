using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChapterButton : MonoBehaviour, ISelectHandler
{
    public int chapterNumber;
    public MenuCamera menuCamera;
    public MenuChapter menuChapter;

    public void OnSelect(BaseEventData eventData)
    {
        menuCamera.SetChapterSelected(chapterNumber);
        menuChapter.SetCurrentChapter(chapterNumber);
    }
}
