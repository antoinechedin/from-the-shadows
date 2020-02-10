using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This class' only purpose is to store info about a Level. It does not heritate from MonoBehaviour.
/// </summary>
[Serializable]
public class Level
{
    private bool completed = false; //indicates wether the level as been completed or not
    private bool[] collectibles;

    public Level(bool completed, bool[] collect)
    {
        this.completed = completed;
        collectibles = collect;
    }

    public bool Completed
    {
        get { return completed; }
        set { completed = value; }
    }

    public bool[] Collectibles
    {
        get { return collectibles; }
        set { collectibles = value; }
    }

    public string PrintLevel()
    {
        string res = "level: " + completed +", collectibles : ";
        foreach (bool b in collectibles)
        {
            res += b.ToString() + " ";
        }

        return res;
    }
}
