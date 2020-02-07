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
    }

    public System.DateTime LastOpenDate
    {
        get { return lastOpenDate; }
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

    public string Print()
    {
        return "nb player : " + nbPlayer + ". nb chapters : " + chapters.Count;
    }




    public static void TestWriteSaveFile(Save save)
    {

        //Save save = GameManager.Instance.Saves[GameManager.Instance.CurrentSave];

        string json = JsonConvert.SerializeObject(save);
        Debug.Log(json);
    }

    public static void TestLoadSaveFile()
    {
        JObject json = JObject.Parse(File.ReadAllText("C:/Users/Leo/Desktop/SaveFileDuo.json"));

        Save save = JsonConvert.DeserializeObject<Save>(json.ToString());
        Debug.Log(save.nbPlayer);
    }



    /// <summary>
    /// Create an empty save file of the solo campaign
    /// </summary>
    public static void CreateSoloFile(int saveIndex)
    {
    }

    /// <summary>
    /// Create an empty save file of the duo campaign
    /// </summary>
    public static void CreateDuoFile()
    {

    }
}
