using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
public class GameManager : Singleton<GameManager>
{
    public enum GameStates { MainMenu, ChoosingLevel, Playing, Paused };

    public GameStates gameState;
    private Save[] saves; //store all saves of the game
    private int currentSave = -1; //l'indice de la save courante

    //info about the state of the game
    private bool debuging = false;
    private bool loading = false;
    //-------------------------------------------------------------


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            debuging = !debuging;
            Debug.Log(debuging);
        }
    }
    //-------------------------------------------------------------

    /// <summary>
    /// Get all the chapters in the game. Levels are obtained by searching into Chapter attribute named 'levels'.
    /// </summary>
    /// <returns>A list containing all chapters</returns>
    public List<Chapter> GetChapters() {
        if (currentSave == -1)
        {
            Debug.LogError("GetChapters  : currentSave index not set, returning chapters of Save[0] by default");
            return saves[0].Chapters;
        }
        else
        {
            return saves[currentSave].Chapters;
        }
    }

    public Save[] Saves
    {
        get { return saves; }
    }

    public bool Debuging
    {
        get { return debuging; }
    }

    public bool Loading
    {
        get { return loading; }
    }

    public int CurrentSave
    {
        get { return currentSave; }
        set { currentSave = value; }
    }

    /// <summary>
    /// sets the completions of the level "lvl" of the chapter "chap" to true
    /// </summary>
    /// <param name="chap"></param>
    /// <param name="lvl"></param>
    public void SetLevelCompleted(int chap, int lvl)
    {
        if (currentSave == -1)
        {
            Debug.LogError("SetLevelCompleted  : currentSave index not set. The level hasn't been set to completed");
        }
        else
        {
            saves[currentSave].Chapters[chap].GetLevels()[lvl].completed = true;
        }
    }

    public void LoadAllSaveFiles()
    {
        StartCoroutine(LoadAllsaveFilesAsync());
    }

    private IEnumerator LoadAllsaveFilesAsync()
    {
        saves = new Save[3];

        loading = true;
        bool finished = false;

        while (!finished)
        {
            for (int i = 0; i < 3; i++) //Warn : set to 3 by default. Need to change if we had more saves
            {
                List<Chapter> chapters = new List<Chapter>();
                Dictionary<string, float> metaFloat = new Dictionary<string, float>();
                Dictionary<string, int> metaInt = new Dictionary<string, int>();

                JObject json = JObject.Parse(File.ReadAllText("Assets/Resources/Saves/SaveFile" + i + ".json"));

                //chargement des metadonnées
                int nbPlayer = (int)json["nbPlayer"];

                metaFloat.Add("totalTimePlayed", (float)json["totalTimePlayed"]);
                metaInt.Add("playerDeath1", (int)json["playerDeath1"]);
                metaInt.Add("jumpNumber1", (int)json["jumpNumber1"]);
                metaFloat.Add("distance1", (float)json["distance1"]);

                if (nbPlayer == 2)
                {
                    metaInt.Add("playerDeath2", (int)json["playerDeath2"]);
                    metaInt.Add("jumpNumber2", (int)json["jumpNumber2"]);
                    metaFloat.Add("distance2", (float)json["distance2"]);
                }


                //chargement des chapitres
                JArray allChapters = (JArray)json["chapters"];
                foreach (JObject chap in allChapters)
                {
                    JArray chapLevels = (JArray)chap["chapter"]["levels"];

                    List<Level> levels = new List<Level>();
                    foreach (JObject level in chapLevels)
                    {
                        int nbCollectible = (int)level["nbCollectible"];
                        int[] collectibles = new int[nbCollectible];

                        for (int k = 0; k < nbCollectible; k++)
                        {
                            collectibles[k] = (int)level["collectibles"][k];
                        }

                        Level lvl = new Level((bool)level["completed"], nbCollectible, collectibles);
                        levels.Add(lvl);
                    }

                    Chapter chapter = new Chapter(levels);
                    chapter.PrintChapter();
                    chapters.Add(chapter);
                }
                FileInfo fileInfo = new FileInfo("Assets/Resources/Saves/SaveFile0.json");
                System.DateTime lastDate = fileInfo.LastWriteTime;
                Save addedSave = new Save(chapters, nbPlayer, metaInt, metaFloat, lastDate);
                saves[i] = addedSave;
            }
            finished = true;
            yield return null;
        }

        loading = false;
    }

    /// <summary>
    /// Loads the SaveFile specified in parameters
    /// </summary>
    /// <param name="save"></param>
    public void LoadSaveFile(int save)
    {
        StartCoroutine(LoadSaveFileAsync(save));
    }

    /// <summary>
    /// Coroutine Loads the SaveFile specified in parameters
    /// </summary>
    /// <param name="save"></param>
    private IEnumerator LoadSaveFileAsync(int save)
    {
        loading = true;
        bool finished = false;

        while (!finished)
        {
            List<Chapter> chapters = new List<Chapter>();
            Dictionary<string, float> metaFloat = new Dictionary<string, float>();
            Dictionary<string, int> metaInt = new Dictionary<string, int>();

            JObject json = JObject.Parse(File.ReadAllText("Assets/Resources/Saves/SaveFile" + save + ".json"));

            //chargement des metadonnées
            int nbPlayer = (int)json["nbPlayer"];

            metaFloat.Add("totalTimePlayed", (float)json["totalTimePlayed"]);
            metaInt.Add("playerDeath1", (int)json["playerDeath1"]);
            metaInt.Add("jumpNumber1", (int)json["jumpNumber1"]);
            metaFloat.Add("distance1", (float)json["distance1"]);

            if (nbPlayer == 2)
            {
                metaInt.Add("playerDeath2", (int)json["playerDeath2"]);
                metaInt.Add("jumpNumber2", (int)json["jumpNumber2"]);
                metaFloat.Add("distance2", (float)json["distance2"]);
            }


            //chargement des chapitres
            JArray allChapters = (JArray)json["chapters"];
            foreach (JObject chap in allChapters)
            {
                JArray chapLevels = (JArray)chap["chapter"]["levels"];

                List<Level> levels = new List<Level>();
                foreach (JObject level in chapLevels)
                {
                    int nbCollectible = (int)level["nbCollectible"];
                    int[] collectibles = new int[nbCollectible];

                    for (int k = 0; k < nbCollectible; k++)
                    {
                        collectibles[k] = (int)level["collectibles"][k];
                    }

                    Level lvl = new Level((bool)level["completed"], nbCollectible, collectibles);
                    levels.Add(lvl);
                }

                Chapter chapter = new Chapter(levels);
                chapter.PrintChapter();
                chapters.Add(chapter);
            }
            FileInfo fileInfo = new FileInfo("Assets/Resources/Saves/SaveFile0.json");
            System.DateTime lastDate = fileInfo.LastWriteTime;
            Save addedSave = new Save(chapters, nbPlayer, metaInt, metaFloat, lastDate);
            saves[save] = addedSave;
            finished = true;
        }
        yield return null;
        loading = false;
    }

    /// <summary>
    /// Write the SaveFile based on the new data in GameManager and writes it on the save file of index "currentSave"
    /// </summary>
    public void WriteSaveFile()
    {
        StartCoroutine(WriteSaveFileAsync());
    }

    private IEnumerator WriteSaveFileAsync()
    {
        bool finished = false;

        while (!finished)
        {
            StreamWriter stream = new StreamWriter("Assets/Resources/Saves/SaveFile" + currentSave + ".json");
            //Save des Metadonnées
            string jsonString = "{\n\t\"nbPlayer\": " + saves[currentSave].NbPlayer + ",\n\t";
            foreach (string key in saves[currentSave].MetaInt.Keys)
            {
                jsonString += "\"" + key + "\": " + saves[currentSave].MetaInt[key] + ",\n\t";
            }

            foreach (string key in saves[currentSave].MetaFloat.Keys)
            {
                string value = saves[currentSave].MetaFloat[key].ToString().Replace(",", ".");
                jsonString += "\"" + key + "\": " + value + ",\n\t";
            }


            //Save des chapitres
            jsonString += "\"chapters\":[";
            int i = 1;
            foreach (Chapter chap in saves[currentSave].Chapters)
            {
                jsonString += "\n\t\t{\n\t\t\t\"chapter\": {\"levels\" :\n\t\t\t[";
                foreach (Level lvl in chap.GetLevels())
                {
                    jsonString += "\n\t\t\t\t" + JsonUtility.ToJson(lvl) + ",";
                }
                jsonString = jsonString.Substring(0, jsonString.Length - 1);
                i++;

                jsonString += "\n\t\t\t]}\n\n\t\t},";
            }
            jsonString = jsonString.Substring(0, jsonString.Length - 1);
            jsonString += "\n\t]\n}";
            stream.Write(jsonString);
            stream.Close();

            finished = true;
            yield return null;
        }
    }

    /// <summary>
    /// Deletes the json file at the index "save"
    /// </summary>
    /// <param name="save"></param>
    public void DeleteSaveFile(int save)
    {
        File.Delete("Assets/Resources/Saves/SaveFile" + save + ".json");
        saves[save] = null;
    }

    /// <summary>
    /// Creates a Json file representing the empty save at the index "save"
    /// </summary>
    /// <param name="save"></param>
    /// <param name="nbPlayer"></param>
    public void CreateSaveFile(int save, int nbPlayer)
    {
        //création du file
        StreamWriter streamWriter = File.CreateText("Assets/Resources/Saves/SaveFile" + save + ".json");

        //On lit le fichier de création de base duo ou solo
        string saveFileContent = "";
        if (nbPlayer == 1)
        {
            saveFileContent = File.ReadAllText("Assets/Resources/SaveFileSolo.json");
        }
        else if (nbPlayer == 2)
        {
            saveFileContent = File.ReadAllText("Assets/Resources/SaveFileDuo.json");
        }

        //On rempli le nouveau SaveFile
        streamWriter.Write(saveFileContent);
        streamWriter.Close();

        LoadSaveFile(save);
    }


    /// <summary>
    /// Use a coroutine to load a scene in the background
    /// </summary>
    /// <param name="sceneName"> The name of the scene to load </param>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsyncScene(sceneName));
    }

    /// <summary>
    /// Coroutine that load the next scene
    /// </summary>
    /// <param name="sceneName"> The name of the scene to load </param>
    /// <returns></returns>
    IEnumerator LoadAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; //permet de ne pas charger la scene directos quand elle est prête

        GameObject loadingScreen = (GameObject)Resources.Load("LoadingScreen"); //load le prefab de l'écran de chargement
        loadingScreen = Instantiate(loadingScreen, gameObject.transform); //l'affiche

        while (!asyncLoad.isDone)
        {
            //on attend que le loading soit completement noir ET que le scene soit prête à être affichée
            if (loadingScreen.GetComponent<LoadingScreen>().finishedFadingIn && asyncLoad.progress == 0.9f)
            {
                //affichage de la scene
                asyncLoad.allowSceneActivation = true;
                loadingScreen.GetComponent<Animator>().SetBool("finishedFadingIn", true); //on fade out le loading screen
            }
            yield return null;
        }
    }

    //---------METADATA MANIPULATION-----------------

    /// <summary>
    /// returns the metadata n of the currentSave or the sabe nbSave if it's passed in parameters
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public int GetMetaInt(string n, int saveNb = -1)
    {
        if (saveNb == -1)
        {
            return saves[currentSave].MetaInt[n];
        }
        else
        {
            return saves[saveNb].MetaInt[n];
        }

    }

    /// <summary>
    /// sets the value of the metadata of name n with the value val
    /// </summary>
    /// <param name="n"></param>
    /// <param name="val"></param>
    public void SetMetaInt(string n, int val)
    {
        saves[currentSave].MetaInt[n] = val;
    }


    /// <summary>
    /// returns the metadata n of the currentSave or the sabe nbSave if it's passed in parameters
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public float GetMetaFloat(string n, int saveNb = -1)
    {
        if (saveNb == -1)
        {
            return saves[currentSave].MetaFloat[n];
        }
        else
        {
            return saves[saveNb].MetaFloat[n];
        }
    }

    /// <summary>
    /// sets the value of the metadata of name n with the value val
    /// </summary>
    /// <param name="n"></param>
    /// <param name="val"></param>
    public void SetMetaFloat(string n, float val)
    {
        saves[currentSave].MetaFloat[n] = val;
    }

    /// <summary>
    /// Adds val the the metadata of name n
    /// </summary>
    /// <param name="n"></param>
    /// <param name="val"></param>
    public void AddMetaInt(string n, int val)
    {
        saves[currentSave].MetaInt[n] = saves[currentSave].MetaInt[n] + val;
    }

    /// <summary>
    /// Adds val the the metadata of name n
    /// </summary>
    /// <param name="n"></param>
    /// <param name="val"></param>
    public void AddMetaFloat(string n, float val)
    {
        saves[currentSave].MetaFloat[n] = saves[currentSave].MetaFloat[n] + val;
    }
}