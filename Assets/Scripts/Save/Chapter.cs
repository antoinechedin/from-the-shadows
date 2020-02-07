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
    public List<Level> levels;

    public Chapter(List<Level> lvl)
    {
        this.levels = lvl;
    }

    public List<Level> GetLevels()
    {
        return levels;
    }

    public int GetNbLevels()
    {
        return levels.Count;
    }

    public bool isCompleted()
    {
        bool completed = true;
        int i = 0;
        while (completed && i < levels.Count)
        {
            completed = levels[i].completed;
            i++;
        }
        return completed;
    }

    public string PrintChapter()
    {
        string res = "";
        Debug.Log("Chapter with " + levels.Count + " levels.");
        foreach (Level l in levels)
        {
            l.PrintLevel();
        }
        return res;
    }
}
