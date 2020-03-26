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
        mInt.Add("jumpNumber2", 0); // if
        mInt.Add("playerDeath1", 0);
        mInt.Add("playerDeath2", 0); // if

        StringFloatDictionary mFloat = new StringFloatDictionary();
        mFloat.Add("distance1", 0);
        mFloat.Add("distance2", 0); // if
        mFloat.Add("totalTimePlayed", 0);

        //On créer les chapitres et les tableaux, puis on l'écrit sur le nouveau fichier.
        List<Level> lvlChap0 = new List<Level>();
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, true)); //0   - The entrance - 2 Collectibles
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { false }, false)); //1
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //2
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //3
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //4
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //5 
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { false }, false)); //6
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, true)); //7   - The room of a thousand stairs - 4 Collectibles
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); // GhostRoom
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //8
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { false }, false)); //9
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //10
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //11
        lvlChap0.Add(new Level(false, new bool[] { false }, new bool[] { false }, false)); //12
        lvlChap0.Add(new Level(false, new bool[] { false }, new bool[] { }, false)); //13
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, true)); //14 - The second encounter - 3 Collectibles
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //15
        lvlChap0.Add(new Level(false, new bool[] { false }, new bool[] { }, false)); //16
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //17
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //18
        lvlChap0.Add(new Level(false, new bool[] { false }, new bool[] { }, false)); //19
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { false }, false)); //20
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //21 
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //22
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, true)); //  - Fresh air - 5 Collectibles
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //23
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //24
        lvlChap0.Add(new Level(false, new bool[] { false }, new bool[] { }, false)); //25
        lvlChap0.Add(new Level(false, new bool[] { false }, new bool[] { false }, false)); //26
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //27
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //28
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //29
        lvlChap0.Add(new Level(false, new bool[] { false }, new bool[] { false }, false)); //30
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, true)); //31 - The third encounter - 4 Collectibles
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //32
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { false }, false)); //33
        lvlChap0.Add(new Level(false, new bool[] { false }, new bool[] { }, false)); //34
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //35
        lvlChap0.Add(new Level(false, new bool[] { false }, new bool[] { false }, false)); //36
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, true)); //37 - The show - 0 Collectibles
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //38
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //39
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //40
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //41
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, false)); //42
        lvlChap0.Add(new Level(false, new bool[] { }, new bool[] { }, true)); //43 - The confrontation - 0 Collectibles
        // List<Level> lvlChap1 = new List<Level>();
        // lvlChap1.Add(new Level(false, new bool[] { false }));


        List<Chapter> chapters = new List<Chapter>();
        chapters.Add(new Chapter(lvlChap0)); // if avec variable
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


}
