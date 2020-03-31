using System;
using UnityEngine;

[Serializable]
public class LevelButtonInfosArray
{
    public LevelButtonInfos[] infos;
}

[Serializable]
public class LevelButtonInfos
{
    public string name;
    public Sprite image;
}