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
    private bool[] shadowCollectibles;
    [SerializeField]
    private bool[] lightCollectibles;
    [SerializeField]
    private bool isCheckpoint;

    public Level(bool completed, bool[] lightCollect, bool[] shadowCollect, bool isCheckpoint)
    {
        this.completed = completed;
        shadowCollectibles = shadowCollect;
        lightCollectibles = lightCollect;
        this.isCheckpoint = isCheckpoint;
    }

    public bool Completed
    {
        get { return completed; }
        set { completed = value; }
    }

    public bool[] LightCollectibles
    {
        get { return lightCollectibles; }
        set { lightCollectibles = value; }
    }

    public bool[] ShadowCollectibles
    {
        get { return shadowCollectibles; }
        set { shadowCollectibles = value; }
    }

    public bool IsCheckpoint
    {
        get { return isCheckpoint; }
        set { isCheckpoint = value; }
    }

    public string PrintLevel()
    {
        string res = "level: " + completed +", collectibles : ";
        foreach (bool b in lightCollectibles)
        {
            res += b.ToString() + " ";
        }

        foreach (bool b in shadowCollectibles)
        {
            res += b.ToString() + " ";
        }

        return res;
    }
}
