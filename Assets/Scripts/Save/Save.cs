using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

[Serializable]
public class Save
{
    [SerializeField]
    private List<Chapter> chapters; //structure qui stock toutes les infos sur les chapitres (et par parentée, sur tous les level)
    [SerializeField]
    private int nbPlayer; //stock si c'est une partie en solo ou en duo

    //METADATA
    [SerializeField]
    private StringIntDictionary metaInt;
    [SerializeField]
    private StringFloatDictionary metaFloat;
    [SerializeField]
    private SerializableDate lastOpenDate;

    public Save(List<Chapter> chaps, int nbPlay, StringIntDictionary mInt, StringFloatDictionary mFloat, SerializableDate dt)
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

    public SerializableDate LastOpenDate
    {
        get { return lastOpenDate; }
        set { lastOpenDate = value; }
    }

    public int NbPlayer
    {
        get { return nbPlayer; }
        set { nbPlayer = value; }
    }

    public StringIntDictionary MetaInt
    {
        get {return metaInt;}
        set { metaInt = value; }
    }

    public StringFloatDictionary MetaFloat
    {
        get { return metaFloat; }
        set { metaFloat = value; }
    }

    public override string ToString()
    {
        return (nbPlayer == 1 ? "solo " : "duo ") + chapters.Count + " chap";
    }


}
