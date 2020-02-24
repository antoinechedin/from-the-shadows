using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class SaveManager : Singleton<SaveManager>
{
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

            string json = File.ReadAllText(Application.persistentDataPath + "/Saves/SaveFile" + saveIndex + ".json");

            Save createdSave = JsonUtility.FromJson<Save>(json);
            FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/Saves/SaveFile" + saveIndex + ".json");
            createdSave.LastOpenDate = new SerializableDate(fileInfo.LastWriteTime);
            GameManager.Instance.Saves[saveIndex] = createdSave;
            Debug.Log("Save " + saveIndex + " loaded : " + createdSave);

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
        GameManager.Instance.Loading = true;

        if (GameManager.Instance.CurrentSave != -1)
        {
            while (!finished)
            {
                int currentSave = GameManager.Instance.CurrentSave;

                StreamWriter stream = new StreamWriter(Application.persistentDataPath + "/Saves/SaveFile" + GameManager.Instance.CurrentSave + ".json");
                //string jsonString = JsonConvert.SerializeObject(GameManager.Instance.Saves[currentSave]);
                string jsonString = JsonUtility.ToJson(GameManager.Instance.Saves[currentSave], true);
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

        GameManager.Instance.Loading = false;
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
        StreamWriter streamWriter = File.CreateText(Application.persistentDataPath + "/Saves/SaveFile" + save + ".json");

        //création des dictionnaires de metadonnées
        StringIntDictionary mInt = new StringIntDictionary();
        mInt.Add("jumpNumber1", 0);
        mInt.Add("jumpNumber2", 0);
        mInt.Add("playerDeath1", 0);
        mInt.Add("playerDeath2", 0);

        StringFloatDictionary mFloat = new StringFloatDictionary();
        mFloat.Add("distance1", 0);
        mFloat.Add("distance2", 0);
        mFloat.Add("totalTimePlayed", 0);

        //On créer les chapitres et les tableaux, puis on l'écrit sur le nouveau fichier.
        List<Level> lvlChap0 = new List<Level>();
        for (int i = 0; i < 34; i++)
        {
            lvlChap0.Add(new Level(false, new bool[] { false, false, false }));
        }
        // List<Level> lvlChap1 = new List<Level>();
        // lvlChap1.Add(new Level(false, new bool[] { false }));


        List<Chapter> chapters = new List<Chapter>();
        chapters.Add(new Chapter(lvlChap0));
        // chapters.Add(new Chapter(lvlChap1));
        // chapters.Add(new Chapter(lvlChap1));
        // chapters.Add(new Chapter(lvlChap1));
        // chapters.Add(new Chapter(lvlChap1));

        //enfin, on créer la save qui contient toutes les informations
        Save createdSave = new Save(chapters, nbPlayer, mInt, mFloat, new SerializableDate(DateTime.Now));


        string saveFileContent = JsonUtility.ToJson(createdSave, true);

        //On rempli le nouveau SaveFile
        streamWriter.Write(saveFileContent);
        streamWriter.Close();

        LoadSaveFile(save);
    }
    #endregion

    public void TestLoad()
    {
        List<Level> levels = new List<Level>();
        Level lvl = new Level(true, new bool[] { false, false });
        levels.Add(lvl);
        levels.Add(lvl);

        List<Chapter> chaps = new List<Chapter>();
        Chapter chap = new Chapter(levels);
        chaps.Add(chap);
        chaps.Add(chap);

        StringIntDictionary mInt = new StringIntDictionary();
        mInt.Add("1", 1);
        mInt.Add("2", 2);

        StringFloatDictionary mFloat = new StringFloatDictionary();
        mFloat.Add("1", 1.0f);
        mFloat.Add("2", 2.0f);

        Save save = new Save(chaps, 1, mInt, mFloat, new SerializableDate(DateTime.Now));

        string json = JsonUtility.ToJson(save, true);
        Debug.Log(json);
        save = null;
        save = JsonUtility.FromJson<Save>(json);
        Debug.Log(save.Chapters[0].GetLevels()[0].Completed);
    }


}
