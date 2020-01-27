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
    public int nbCollectibleTaken; //The number of collectible the player took
    public int totalNbCollectible; //The total number of collectible there are in the level

    public Level(bool completed, int nbCollectibleTaken, int totalNbCollectible)
    {
        this.completed = completed;
        this.nbCollectibleTaken = nbCollectibleTaken;
        this.totalNbCollectible = totalNbCollectible;
    }

    public void PrintLevel()
    {
        Debug.Log("level : " + completed +", "+nbCollectibleTaken +"/ " +totalNbCollectible);
    }
}
