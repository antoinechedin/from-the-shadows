using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{

    public static void TestWriteSaveFile(Save save)
    {

        //Save save = GameManager.Instance.Saves[GameManager.Instance.CurrentSave];

        string json = JsonConvert.SerializeObject(save);
        Debug.Log(json);
    }


    #region Load Save File

    /// <summary>
    /// Calls the coroutine that load all the saves at once.
    /// </summary>
    public void LoadAllSaveFiles()
    {
        StartCoroutine(LoadAllsaveFilesAsync());
    }

    /// <summary>
    /// Asynchromously loads all the saves
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadAllsaveFilesAsync()
    {
        GameManager.Instance.Loading = true;
        bool finished = false;

        while (!finished)
        {
            if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "/Saves/");
            FileInfo[] filesInfo = directoryInfo.GetFiles();
            foreach (FileInfo f in filesInfo)
            {
                switch (f.Name)
                {
                    case "SaveFile0.json":
                        LoadSaveFile(0);
                        break;
                    case "SaveFile1.json":
                        LoadSaveFile(1);
                        break;
                    case "SaveFile2.json":
                        LoadSaveFile(2);
                        break;
                }
            }
            finished = true;
            yield return null;
        }

        GameManager.Instance.Loading = false;
    }

    public void LoadSaveFile(int save)
    {
        StartCoroutine(LoadSaveFileAsync(save));
    }

    static IEnumerator LoadSaveFileAsync(int saveIndex)
    {
        bool finished = false;

        while (!finished)
        {
            GameManager.Instance.Loading = true;

            //JObject json = JObject.Parse(File.ReadAllText(Application.persistentDataPath + "/Saves/SaveFile" + saveIndex + ".json"));
            JObject json = JObject.Parse(File.ReadAllText(Application.persistentDataPath + "/Saves/SaveFile"+saveIndex+".json"));

            Save createdSave = JsonConvert.DeserializeObject<Save>(json.ToString());
            FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/Saves/SaveFile" + saveIndex + ".json");
            createdSave.LastOpenDate = fileInfo.LastWriteTime;
            GameManager.Instance.Saves[saveIndex] = createdSave;
            Debug.Log("Save " + saveIndex + " loaded : " + createdSave.Print());

            finished = true;
        }
        yield return null;
        GameManager.Instance.Loading = false;
    }
    #endregion

    #region Write Save File
    /// <summary>
    /// Calls the coroutine that saves the data present in GM on the current loaded saveFile
    /// </summary>
    public void WriteSaveFile()
    {
        StartCoroutine(WriteSaveFileAsync());
    }

    /// <summary>
    /// Write the SaveFile based on the new data in GameManager and writes it on the save file of index "currentSave"
    /// </summary>
    /// <returns></returns>
    private IEnumerator WriteSaveFileAsync()
    {
        bool finished = false;

        if (GameManager.Instance.CurrentSave != -1)
        {
            while (!finished)
            {
                int currentSave = GameManager.Instance.CurrentSave;

                StreamWriter stream = new StreamWriter(Application.persistentDataPath + "/Saves/SaveFile" + GameManager.Instance.CurrentSave + ".json");
                string jsonString = JsonConvert.SerializeObject(GameManager.Instance.Saves[currentSave]); ;
                stream.Write(jsonString);
                stream.Close();

                finished = true;
                yield return null;
            }
        }
        else
        {
            Debug.LogWarning("WARN : SaveManager.WriteSaveFile: GameManager.CurrentSave is not set. Not saving.");
        }
    }
    #endregion

    #region Json SaveFiles management
    /// <summary>
    /// Deletes the json file at the index "save"
    /// </summary>
    /// <param name="save"></param>
    public void DeleteSaveFile(int save)
    {
        File.Delete(Application.persistentDataPath + "/Saves/SaveFile" + save + ".json");
        GameManager.Instance.Saves[save] = null;
    }

    /// <summary>
    /// Creates a Json file representing the empty save at the index "save"
    /// </summary>
    /// <param name="save"></param>
    /// <param name="nbPlayer"></param>
    public void CreateSaveFile(int save, int nbPlayer)
    {
        //création du file
        StreamWriter streamWriter = File.CreateText(Application.persistentDataPath + "/Saves/SaveFile"+save+".json");

        //création des dictionnaires de metadonnées
        Dictionary<string, int> mInt = new Dictionary<string, int>();
        mInt.Add("jumpNumber1", 0);
        mInt.Add("jumpNumber2", 0);
        mInt.Add("playerDeath1", 0);
        mInt.Add("playerDeath2", 0);

        Dictionary<string, float> mFloat = new Dictionary<string, float>();
        mFloat.Add("distance1", 0);
        mFloat.Add("distance2", 0);
        mFloat.Add("totalTimePlayed", 0);

        //On créer une structure de données vide, puis on l'écrit sur le nouveau fichier (en gros on créer une save vide)
        List<Level> lvlChap1 = new List<Level>();
        lvlChap1.Add(new Level(false, new bool[] { false }));
        lvlChap1.Add(new Level(false, new bool[] { false }));
        lvlChap1.Add(new Level(false, new bool[] { false }));

        List<Chapter> chapters = new List<Chapter>();
        chapters.Add(new Chapter(lvlChap1));

        List<Level> lvlChap2 = new List<Level>();
        lvlChap2.Add(new Level(false, new bool[] { false }));

        chapters.Add(new Chapter(lvlChap2));

        Save createdSave = new Save(chapters, nbPlayer, mInt, mFloat, System.DateTime.Now);


        string saveFileContent = JsonConvert.SerializeObject(createdSave);

        //On rempli le nouveau SaveFile
        streamWriter.Write(saveFileContent);
        streamWriter.Close();

        LoadSaveFile(save);
    }
    #endregion
}
