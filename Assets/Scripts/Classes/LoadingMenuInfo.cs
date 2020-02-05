using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingMenuInfo
{
    private int startingMenuScene { get;} // Menu to open when going in the mainManu scene (mainMenu, SaveFile, chapterSelection)
    private int startingChapterIndex { get; } // index of chapter position for the cursor in the chapterSelection

    public LoadingMenuInfo(int startMenuScene)
    {
        startingMenuScene = startMenuScene;
        startingChapterIndex = -1;
    }

    public LoadingMenuInfo(int startMenuScene, int startingChapter)
    {
        startingMenuScene = startMenuScene;
        startingChapterIndex = startingChapter;
    }
}
