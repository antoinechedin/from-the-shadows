using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System;

public class GameManager : Singleton<GameManager>
{
    private LoadingMenuInfo loadingMenuInfos = null;
    private LoadingChapterInfo loadingChapterInfos = null;

    private Save[] saves = new Save[3]; //store all saves of the game
    private int currentSave = -1; //l'indice de la save courante
    private int currentChapter = -1;

    //info about the state of the game
    private bool debuging = false;
    private bool debugCanvasDisplayed = false;
    private bool loading = false;

    //debug bools
    private bool displayedNoSaveFile = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.D))
        {
            debuging = !debuging;
        }

        DisplayDebugCanvas();
    }

    #region getters / setters
    /// <summary>
    /// Get all the chapters in the game. Levels are obtained by searching into Chapter attribute named 'levels'.
    /// </summary>
    /// <returns>A list containing all chapters</returns>
    public List<Chapter> GetChapters()
    {
        if (currentSave == -1)
        {
            Debug.LogWarning("GetChapters  : currentSave index not set, returning chapters of Save[0] by default");
            return saves[0].Chapters;
        }
        else
        {
            return saves[currentSave].Chapters;
        }
    }

    public int CurrentChapter
    {
        get { return currentChapter; }
        set { currentChapter = value; }
    }

    public LoadingMenuInfo LoadingMenuInfos
    {
        get { return loadingMenuInfos; }
        //WARN : le set n'est là que parce que notre jeu commence direct dans le menu
        //Dans le cas, où on a un splash screen avant, les LoadingMenuInfo seront set via le LoadMenu
        //qui sera appelé depuis le splash screen
        set { loadingMenuInfos = value; }
    }

    public LoadingChapterInfo LoadingChapterInfo
    {
        get { return loadingChapterInfos; }
        set { loadingChapterInfos = value; }
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
    #endregion

    #region Save data manipulation
    /// <summary>
    /// sets the completion of the level "lvl" of the chapter "chap" to true
    /// </summary>
    /// <param name="chap"></param>
    /// <param name="lvl"></param>
    public void SetLevelCompleted(int chap, int lvl)
    {
        try
        {
            saves[currentSave].Chapters[chap].GetLevels()[lvl].completed = true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("WARN META : No Save detected. Doing nothing>" + e.StackTrace);
        }
    }
    #endregion

    #region Save Json Load
    /// <summary>
    /// Calls the coroutine that loads the file given in parameters
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
        bool finished = false;

        while (!finished)
        {
            loading = true;
            List<Chapter> chapters = new List<Chapter>();
            Dictionary<string, float> metaFloat = new Dictionary<string, float>();
            Dictionary<string, int> metaInt = new Dictionary<string, int>();

            JObject json = JObject.Parse(File.ReadAllText(Application.persistentDataPath + "/Saves/SaveFile" + save + ".json"));

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
                chapters.Add(chapter);
            }
            FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/Saves/SaveFile" + save + ".json");
            System.DateTime lastDate = fileInfo.LastWriteTime;
            Save addedSave = new Save(chapters, nbPlayer, metaInt, metaFloat, lastDate);
            saves[save] = addedSave;
            finished = true;
        }
        yield return null;
        loading = false;
    }

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
        Debug.Log(Application.persistentDataPath);
        saves = new Save[3];

        loading = true;
        bool finished = false;

        while (!finished)
        {
            for (int i = 0; i < 3; i++) //Warn : set to 3 by default. Need to change if we had more saves
            {
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
            }
            finished = true;
            yield return null;
        }

        loading = false;
    }
    #endregion

    #region Save Json Write
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

        while (!finished)
        {
            StreamWriter stream = new StreamWriter(Application.persistentDataPath + "/Saves/SaveFile" + currentSave + ".json");
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
    #endregion

    #region Json SaveFiles management
    /// <summary>
    /// Deletes the json file at the index "save"
    /// </summary>
    /// <param name="save"></param>
    public void DeleteSaveFile(int save)
    {
        File.Delete(Application.persistentDataPath + "/Saves/SaveFile" + save + ".json");
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
        StreamWriter streamWriter = File.CreateText(Application.persistentDataPath + "/Saves/SaveFile" + save + ".json");

        //On lit le fichier de création de base duo ou solo
        string saveFileContent = "";
        if (nbPlayer == 1)
        {
            saveFileContent = File.ReadAllText(Application.persistentDataPath + "/SaveFileSolo.json");
        }
        else if (nbPlayer == 2)
        {
            saveFileContent = File.ReadAllText(Application.persistentDataPath + "/SaveFileDuo.json");
        }

        //On rempli le nouveau SaveFile
        streamWriter.Write(saveFileContent);
        streamWriter.Close();

        LoadSaveFile(save);
    }
    #endregion

    #region Load scenes
    /// <summary>
    /// Set the LoadingMenuInfos before loading the scene.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="loadingInfo"></param>
    public void LoadMenu(string sceneName, LoadingMenuInfo loadingInfo)
    {
        loadingMenuInfos = loadingInfo;
        StartCoroutine(LoadAsyncScene(sceneName));
    }

    /// <summary>
    /// Set the LoadingChapterInfos before loading the scene
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="loadingInfo"></param>
    public void LoadChapter(string sceneName, LoadingChapterInfo loadingInfo)
    {
        loadingChapterInfos = loadingInfo;
        StartCoroutine(LoadAsyncScene(sceneName));
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
    /// Coroutine that load a scene fading the screen to black
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
    #endregion

    #region Metadata

    /// <summary>
    /// returns the metadata n of the currentSave or the sabe nbSave if it's passed in parameters
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public int GetMetaInt(string n, int saveNb = -1)
    {
        try
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
        catch (Exception e)
        {
            Debug.LogWarning("WARN META : No Save detected. Returning -1 by default" + e.StackTrace);
            return -1;
        }

    }


    /// <summary>
    /// returns the metadata n of the currentSave or the save nbSave if it's passed in parameters
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public float GetMetaFloat(string n, int saveNb = -1)
    {
        try
        {
            if (saveNb == -1) //if no save number is specified, returns the meta of the current save
            {
                return saves[currentSave].MetaFloat[n];
            }
            else //else return the meta of the asked save
            {
                return saves[saveNb].MetaFloat[n];
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("WARN META : :No Save detected. Returning -1 by default" + e.StackTrace);
            return -1;
        }
    }

    /// <summary>
    /// sets the value of the metadata of name n with the value val
    /// </summary>
    /// <param name="n"></param>
    /// <param name="val"></param>
    public void SetMetaInt(string n, int val)
    {
        try
        {
            saves[currentSave].MetaInt[n] = val;
        }
        catch (Exception e)
        {
            if (!displayedNoSaveFile)
            {
                Debug.LogWarning("WARN META : No Save detected" + e.StackTrace);
                displayedNoSaveFile = true;
            }
        }
    }

    /// <summary>
    /// sets the value of the metadata of name n with the value val
    /// </summary>
    /// <param name="n"></param>
    /// <param name="val"></param>
    public void SetMetaFloat(string n, float val)
    {
        try
        {
            saves[currentSave].MetaFloat[n] = val;
        }
        catch (Exception e)
        {
            if (!displayedNoSaveFile)
            {
                Debug.LogWarning("WARN META : No Save detected" + e.StackTrace);
                displayedNoSaveFile = true;
            }
        }
    }

    /// <summary>
    /// Adds val the the metadata of name n
    /// </summary>
    /// <param name="n"></param>
    /// <param name="val"></param>
    public void AddMetaInt(string n, int val)
    {
        try
        {
            saves[currentSave].MetaInt[n] = saves[currentSave].MetaInt[n] + val;
        }
        catch (Exception e)
        {
            if (!displayedNoSaveFile)
            {
                Debug.LogWarning("WARN META : No Save detected" + e.StackTrace);
                displayedNoSaveFile = true;
            }
        }
    }

    /// <summary>
    /// Adds val the the metadata of name n
    /// </summary>
    /// <param name="n"></param>
    /// <param name="val"></param>
    public void AddMetaFloat(string n, float val)
    {
        try
        {
            saves[currentSave].MetaFloat[n] = saves[currentSave].MetaFloat[n] + val;
        }
        catch (Exception e)
        {
            if (!displayedNoSaveFile)
            {
                Debug.LogWarning("WARN META : No Save detected" + e.StackTrace);
                displayedNoSaveFile = true;
            }
        }
    }
    #endregion


    private void DisplayDebugCanvas()
    {
        if (debuging)//display
        {
            if (!debugCanvasDisplayed)
            {
                string savesLength = "saves length : ",
                    save0 = "save 0 : ",
                    save1 = "save 1 : ",
                    save2 = "save 2 : ",
                    curSave = "Current save : ",
                    load = "Loading : ",
                    nbChapter = "nb chapter : ",
                    currentChap = "current chapter : ",
                    nbLvl = "nb levels : ",
                    loadingMenInfo = "LoadingMenuInfo : ",
                    loadingChapInfo = "LoadingChapterInfo : ";

                GameObject debugCanvas = Instantiate((GameObject)Resources.Load("GMDebugCanvas"), Vector3.zero, Quaternion.identity);
                if (saves != null)
                {
                    savesLength += saves.Length.ToString();
                    save0 += saves[0] != null ? saves[0].Print() : "null";
                    save1 += saves[1] != null ? saves[1].Print() : "null";
                    save2 += saves[2] != null ? saves[2].Print() : "null";
                    if (currentSave != -1 && saves[currentSave] != null)
                    {
                        nbChapter += saves[currentSave].Chapters.Count.ToString();
                        if (CurrentChapter != -1 && saves[currentSave].Chapters[currentChapter] != null)
                        {
                            nbLvl += saves[CurrentSave].Chapters[currentChapter].GetNbLevels();
                        }
                        else
                        {
                            nbLvl += "null";
                        }
                    }
                    else
                    {
                        nbChapter += "null";
                        nbLvl += "null";
                    }
                }
                else
                {
                    savesLength += "null";
                    nbChapter += "null";
                    save0 += "null";
                    save1 += "null";
                    save2 += "null";
                    nbLvl += "null";
                }


                curSave += currentSave;
                load += loading.ToString();

                loadingMenInfo += loadingMenuInfos != null ? loadingMenuInfos.Print() : "null";
                loadingChapInfo += loadingChapterInfos != null ? loadingChapterInfos.Print() : "null";

                debugCanvas.transform.Find("Saves length").GetComponent<Text>().text = savesLength;
                debugCanvas.transform.Find("Save0").GetComponent<Text>().text = save0;
                debugCanvas.transform.Find("Save1").GetComponent<Text>().text = save1;
                debugCanvas.transform.Find("Save2").GetComponent<Text>().text = save2;
                debugCanvas.transform.Find("Current Save").GetComponent<Text>().text = curSave;
                debugCanvas.transform.Find("loading").GetComponent<Text>().text = load;
                debugCanvas.transform.Find("nbChapterCurrentSave").GetComponent<Text>().text = nbChapter;
                debugCanvas.transform.Find("nbLevels").GetComponent<Text>().text = nbLvl;
                debugCanvas.transform.Find("currentChapter").GetComponent<Text>().text = currentChap;
                debugCanvas.transform.Find("loadingMenuInfo").GetComponent<Text>().text = loadingMenInfo;
                debugCanvas.transform.Find("loadingChapterInfo").GetComponent<Text>().text = loadingChapInfo;
   
                debugCanvasDisplayed = true;
            }
        }
        else //destroy
        {
            Destroy(GameObject.Find("GMDebugCanvas(Clone)"));
            debugCanvasDisplayed = false;
        }

    }

    public void TestLoadSave(int save)
    {

        JObject json = JObject.Parse(File.ReadAllText("C:/Users/lveyssiere/Desktop/SaveFileDuo.json"));


        //chargement des chapitres
        List<Chapter> chaps = JsonConvert.DeserializeObject<List<Chapter>>(json["chapters"].ToString());
        Debug.Log(json["chapters"].ToString());
        Chapter chaptest = JsonConvert.DeserializeObject<Chapter>(json["chapters"][0]["chapter"].ToString());
        chaptest.PrintChapter();
    }

    /// <summary>
    /// This function does nothing except allowing to call the GM therefore make it spawn
    /// </summary>
    public void SpawnGameManager()
    {
        Debug.Log("GameManager spawned");
    }
}