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
    public bool completed = false; //indicates wether the level as been completed or not
    public int nbCollectible;
    public int[] collectibles;

    public Level(bool completed, int nbCollect, int[] collect)
    {
        this.completed = completed;
        nbCollectible = nbCollect;
        collectibles = collect;
    }

    public void PrintLevel()
    {
        string res = "level: " + completed +" nbCollectible : " +nbCollectible +", collectibles ";
        foreach (int i in collectibles)
        {
            res += i + " ";
        }

        Debug.Log(res);
    }
}
