using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingMenuInfo
{
    private int startingMenuScene; // Menu to open when going in the mainMenu scene (mainMenu, SaveFile, chapterSelection)
    private int chapterFirstFinish; // if > 0 : le chapitre fini. if = -1 pas de chapitre fini

    public LoadingMenuInfo(int startMenuScene, int chapterFinish = -1)
    {
        startingMenuScene = startMenuScene;
        chapterFirstFinish = chapterFinish;
    }

    public int StartingMenuScene
    {
        get { return startingMenuScene; }
    }

    public int FinishChapterForFirstTime
    {
        get { return chapterFirstFinish; }
    }

    public override string ToString()
    {
        return "startingMenuScene: " +startingMenuScene;
    }
}