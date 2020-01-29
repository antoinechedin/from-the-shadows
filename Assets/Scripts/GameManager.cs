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
    private List<Chapter> chapters;
    private int currentChapter = 0;
    private int currentLevel = 0;
    private bool debuging = false;


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

    private int CurrentChapter
    {
        get { return currentChapter; }
        set { currentChapter = value; }
    }

    private int CurrentLevel
    {
        get { return currentLevel; }
        set { currentLevel = value; }
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

        JObject json = JObject.Parse(File.ReadAllText("Assets/Resources/SaveFile.json"));
        JArray allChapters = (JArray)json["chapters"];
        foreach (JObject chap in allChapters)
        {
            JArray chapLevels = (JArray) chap["chapter"]["levels"];

            List<Level> levels = new List<Level>();
            foreach (JObject level in chapLevels)
            {
                Level lvl = new Level((bool) level["completed"], (int) level["nbCollectibleTaken"], (int) level["totalNbCollectible"]);
                levels.Add(lvl);
            }

            Chapter chapter = new Chapter(levels);
            chapters.Add(chapter);
        }
    }

    /// <summary>
    /// Write the SaveFile based on the new data in GameManager.Chapters
    /// </summary>
    public void WriteSaveFile()
    {
        StreamWriter stream = new StreamWriter("Assets/Resources/SaveFile.json");
        string jsonString = "{\n\t\"chapters\":[";
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
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }






    //---------METADATA COLLECT-----------------

    /// <summary>
    ///Metadata : PlayerDeath++
    ///You have to specifiy the player id (1 or 2) in the first parameter
    /// </summary>
    public void MetaAddPlayerDeath(int playerId)
    {
        PlayerPrefs.SetInt("playerDeath"+playerId, PlayerPrefs.GetInt("playerDeath"+playerId, 0) + 1);
    }


    /// <summary>
    /// Metadata : totalDistance + addedDistance;
    /// You have to specifiy the player id (1 or 2) in the first parameter
    /// </summary>
    public void MetaAddDistance(int playerId, float addedDistance)
    {
        PlayerPrefs.SetFloat("totalDistance" + playerId, PlayerPrefs.GetFloat("totalDistance" + playerId, 0) + addedDistance);
    }

    /// <summary>
    /// Metadata : jumpNumber + 1;
    /// You have to specifiy the player id (1 or 2) in the first parameter
    /// </summary>
    public void MetaAddJump(int playerId)
    {
        PlayerPrefs.SetInt("jumpNumber" + playerId, PlayerPrefs.GetInt("jumpNumber" + playerId, 0) + 1);
    }

    /// <summary>
    /// Metadata : totalTimePlayed + timeAdded;
    /// </summary>
    /// <param name="timeAdded"></param>
    public void MetaTotalTimePlayed(float timeAdded)
    {
        PlayerPrefs.SetFloat("totalTimePlayed", PlayerPrefs.GetFloat("totalTimePlayed", 0) + timeAdded);
    }

    /// <summary>
    /// Metadata : timeInsideALevel + timeAdded;
    /// </summary>
    public void MetaAddTimeToLevel(string levelID, float timeAdded)
    {
        PlayerPrefs.SetFloat(levelID, PlayerPrefs.GetFloat(levelID, 0) + timeAdded);
    }
}