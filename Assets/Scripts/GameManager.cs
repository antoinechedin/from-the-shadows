using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
public class GameManager : Singleton<GameManager>
{
    public enum GameStates {MainMenu, ChoosingLevel, Playing, Paused};

    public GameStates gameState;
    private List<Chapter> chapters; //structure qui stock toutes les infos sur les chapitres (et par parentée, sur tous les level)
    private bool debuging = false;
    private int nbPlayer; //stock si c'est une partie en solo ou en duo

    //METADATA
    private Dictionary<string, int> metaInt;
    private Dictionary<string, float> metaFloat;



    //-------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
    }

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
        return chapters;
    }

    public bool Debuging
    {
        get { return debuging; }
    }

    /// <summary>
    /// Read the SaveFile and store levels data in memory (GameManager.chapters)
    /// </summary>
    public void LoadSaveFile()
    {
        chapters = new List<Chapter>();
        metaFloat = new Dictionary<string, float>();
        metaInt = new Dictionary<string, int>();

        JObject json = JObject.Parse(File.ReadAllText("Assets/Resources/SaveFile.json"));

        //chargement des metadonnées
        nbPlayer = (int)json["nbPlayer"];

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
            JArray chapLevels = (JArray) chap["chapter"]["levels"];

            List<Level> levels = new List<Level>();
            foreach (JObject level in chapLevels)
            {
                int nbCollectible = (int)level["nbCollectible"];
                int[] collectibles = new int[nbCollectible];

                for (int i = 0; i < nbCollectible; i++)
                {
                    collectibles[i] = (int) level["collectibles"][i];
                }
               
                Level lvl = new Level((bool) level["completed"], nbCollectible, collectibles);
                levels.Add(lvl);
            }

            Chapter chapter = new Chapter(levels);
            chapter.PrintChapter();
            chapters.Add(chapter);
        }
    }

    /// <summary>
    /// Write the SaveFile based on the new data in GameManager.Chapters
    /// </summary>
    public void WriteSaveFile()
    {
        StreamWriter stream = new StreamWriter("Assets/Resources/SaveFile.json");
        //Save des Metadonnées
        string jsonString = "{\n\t\"nbPlayer\": "+nbPlayer+",\n\t";
        foreach (string key in metaInt.Keys)
        {
            jsonString += "\""+key+"\": "+metaInt[key]+",\n\t";
        }

        foreach (string key in metaFloat.Keys)
        {
            string value = metaFloat[key].ToString().Replace(",", ".");
            jsonString += "\"" + key + "\": " +value + ",\n\t";
        }


        //Save des chapitres
        jsonString += "\"chapters\":[";
        int i = 1;
        foreach (Chapter chap in chapters)
        {
            jsonString += "\n\t\t{\n\t\t\t\"chapter\": {\"levels\" :\n\t\t\t[";
            foreach (Level lvl in chap.GetLevels())
            {
                jsonString += "\n\t\t\t\t" +JsonUtility.ToJson(lvl)+",";
            }
            jsonString = jsonString.Substring(0, jsonString.Length - 1);
            i++;

            jsonString += "\n\t\t\t]}\n\n\t\t},";
    }
        jsonString = jsonString.Substring(0, jsonString.Length - 1);
        jsonString += "\n\t]\n}";
        stream.Write(jsonString);
        stream.Close();
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
    /// returns the value of the metadata of name n
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public int GetMetaInt(string n)
    {
        return metaInt[n];
    }

    /// <summary>
    /// sets the value of the metadata of name n with the value val
    /// </summary>
    /// <param name="n"></param>
    /// <param name="val"></param>
    public void SetMetaInt(string n, int val)
    {
         metaInt[n] = val;
    }


    /// <summary>
    /// returns the value of the metadata of name n
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public float GetMetaFloat(string n)
    {
         return metaFloat[n];
    }

    /// <summary>
    /// sets the value of the metadata of name n with the value val
    /// </summary>
    /// <param name="n"></param>
    /// <param name="val"></param>
    public void SetMetaFloat(string n, float val)
    {
         metaFloat[n] = val;
    }

    /// <summary>
    /// Adds val the the metadata of name n
    /// </summary>
    /// <param name="n"></param>
    /// <param name="val"></param>
    public void AddMetaInt(string n, int val)
    {
        metaInt[n] += val;
    }

    /// <summary>
    /// Adds val the the metadata of name n
    /// </summary>
    /// <param name="n"></param>
    /// <param name="val"></param>
    public void AddMetaFloat(string n, float val)
    {
        metaFloat[n] = metaFloat[n] + val;
    }
}