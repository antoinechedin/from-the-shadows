using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingChapterInfo
{
    private int startLevelIndex;

    public LoadingChapterInfo(int i)
    {
        startLevelIndex = i;
    }

    public int StartLevelIndex
    {
        get { return startLevelIndex; }
    }

    public override string ToString()
    {
        return "startLevelIndex: " + startLevelIndex;
    }
}
