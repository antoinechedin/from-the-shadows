using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[System.Serializable]
public class Save
{
    private List<Chapter> chapters; //structure qui stock toutes les infos sur les chapitres (et par parentée, sur tous les level)
    private int nbPlayer; //stock si c'est une partie en solo ou en duo

    //METADATA
    private Dictionary<string, int> metaInt;
    private Dictionary<string, float> metaFloat;
    private System.DateTime lastOpenDate;

    public Save(List<Chapter> chaps, int nbPlay, Dictionary<string, int> mInt, Dictionary<string, float> mFloat, System.DateTime dt)
    {
        chapters = chaps;
        nbPlayer = nbPlay;
        metaInt = mInt;
        metaFloat = mFloat;
        lastOpenDate = dt;
    }

    public List<Chapter> Chapters
    {
        get { return chapters; }
        set { chapters = value; }
    }

    public System.DateTime LastOpenDate
    {
        get { return lastOpenDate; }
        set { lastOpenDate = value; }
    }

    public int NbPlayer
    {
        get { return nbPlayer; }
        set { nbPlayer = value; }
    }

    public Dictionary<string, int> MetaInt
    {
        get {return metaInt;}
        set { metaInt = value; }
    }

    public Dictionary<string, float> MetaFloat
    {
        get { return metaFloat; }
        set { metaFloat = value; }
    }

    public string Print()
    {
        return "nb player : " + nbPlayer + ". nb chapters : " + chapters.Count;
    }


}
