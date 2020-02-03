using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save
{
    private List<Chapter> chapters; //structure qui stock toutes les infos sur les chapitres (et par parentée, sur tous les level)
    private int nbPlayer; //stock si c'est une partie en solo ou en duo

    //METADATA
    private Dictionary<string, int> metaInt;
    private Dictionary<string, float> metaFloat;

    public Save(List<Chapter> chaps, int nb, Dictionary<string, int> mInt, Dictionary<string, float> mFloat)
    {
        chapters = chaps;
        nbPlayer = nb;
        metaInt = mInt;
        metaFloat = mFloat;
    }

    public List<Chapter> Chapters
    {
        get { return chapters; }
    }

    public int NbPlayer
    {
        get { return nbPlayer; }
    }

    public Dictionary<string, int> MetaInt
    {
        get {return metaInt;}
    }

    public Dictionary<string, float> MetaFloat
    {
        get { return metaFloat; }
    }
}
