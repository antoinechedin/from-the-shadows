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
    [SerializeField]
    private bool completed = false; //indicates wether the level as been completed or not
    [SerializeField]
    private bool[] collectibles;
    [SerializeField]
    private bool isCheckpoint;

    public Level(bool completed, bool[] collect, bool isCheckpoint)
    {
        this.completed = completed;
        collectibles = collect;
        this.isCheckpoint = isCheckpoint;
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

    public bool IsCheckpoint
    {
        get { return isCheckpoint; }
        set { isCheckpoint = value; }
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
