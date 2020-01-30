using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This class' only purpose is to store info about a chapter. It does not heritate from MonoBehaviour.
/// </summary>
[Serializable]
public class Chapter
{
    private List<Level> levels;

    public Chapter(List<Level> lvl)
    {
        this.levels = lvl;
    }

    public List<Level> GetLevels()
    {
        return levels;
    }

    public void PrintChapter()
    {
        Debug.Log("Chapter avec " +levels.Count +" level");
        foreach (Level l in levels)
        {
            l.PrintLevel();
        }
    }
}
