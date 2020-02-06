using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingMenuInfo
{
    private int startingMenuScene; // Menu to open when going in the mainManu scene (mainMenu, SaveFile, chapterSelection)

    public LoadingMenuInfo(int startMenuScene)
    {
        startingMenuScene = startMenuScene;
    }

    public int StartingMenuScene
    {
        get { return startingMenuScene; }
    }
}