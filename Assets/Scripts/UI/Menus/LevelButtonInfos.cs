using System;
using UnityEngine.UI;

[Serializable]
public class LevelButtonInfosArray
{
    public LevelButtonInfos[] infos;
}

[Serializable]
public class LevelButtonInfos
{
    public string name;
    public Image image;
}