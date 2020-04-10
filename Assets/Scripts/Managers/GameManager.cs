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
    private int currentChapter = -1; //indice du chapitre courant

    //info about the state of the game
    private bool debuging = false;
    private bool loading = false;
    private bool debugCanvasExist = false;

    //Store a reference to the debug canvas when it's spawned
    private GameObject debugCanvas;

    //debug bools
    private bool displayedNoSaveFile = false;
    private bool isInCutscene = false;


    // Options update
    public delegate void OnOptionsUpdateDelegate();
    public event OnOptionsUpdateDelegate optionsUpdateDelegate;

    public void OnOptionUpdate()
    {
        Debug.Log("OnOptionUpdate() called");
        if (optionsUpdateDelegate != null)
            optionsUpdateDelegate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.D))
        {
            debuging = !debuging;
        }

        if (debuging && !debugCanvasExist)
        {
            Instantiate((GameObject)Resources.Load("DebugCanvas"), Vector3.zero, Quaternion.identity, transform);
            debugCanvasExist = true;
        }
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
            Debug.LogWarning("WARN GameManager.GetChapters: currentSave index not set, returning empty List by default");
            return new List<Chapter>();
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
        set { loading = value; }
    }

    public int CurrentSave
    {
        get { return currentSave; }
        set { currentSave = value; }
    }

    public bool IsInCutscene
    {
        get { return isInCutscene; }
        set { isInCutscene = value; }
    }

    public Chapter GetCurrentChapter()
    {
        return GetChapters()[currentChapter];
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
            saves[currentSave].Chapters[chap].GetLevels()[lvl].Completed = true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("WARN GameManager.SetCompleted : CurrentSAve not set. Doing nothing>" + e.StackTrace);
        }
    }

    public void SaveCollectibleTaken(int chap, int lvl, Collectible.Type type, int index)
    {
        if (CurrentChapter != -1)
        {
            if (type == Collectible.Type.Light)
            {
                saves[currentSave].Chapters[chap].GetLevels()[lvl].LightCollectibles[index] = true;
            }
            else if (type == Collectible.Type.Shadow)
            {
                saves[currentSave].Chapters[chap].GetLevels()[lvl].ShadowCollectibles[index] = true;
            }
        }
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
        //étape 1 : On fait un fondu au noir
        GameObject loadingScreen = (GameObject)Resources.Load("LoadingScreen"); //load le prefab de l'écran de chargement
        loadingScreen = Instantiate(loadingScreen, gameObject.transform); //l'affiche

        //On attend que le fondu au noir soit terminé
        while (!loadingScreen.GetComponent<LoadingScreen>().finishedFadingIn)
        {
            yield return null;
        }

        //étape 2 : On lance le chargement de la scène
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; //permet de ne pas charger la scene directos quand elle est prête

        //On attend la fin du chargement
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        //étape 3 : on affiche la scène. à cette étape, le scène n'est pas encore totalement prête à être révélée.
        asyncLoad.allowSceneActivation = true;

        //on attend que la scène soit complètement prète à être affichée
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //étape 4 : On enlève l'écran de chargement
        loadingScreen.GetComponent<Animator>().SetBool("finishedFadingIn", true); //on fade out le loading screen
        DiscordController.Instance.SetActivity();
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
            Debug.LogWarning("WARN GameManager.GetMetaInt: CurrentSave not set. Returning -1 by default" + e.StackTrace);
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
            Debug.LogWarning("WARN GameManager.GetMetaFloat: CurrentSave not set. Returning -1 by default" + e.StackTrace);
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
                Debug.LogWarning("WARN GameManager.SetMetaInt: CurrentSave not set" + e.StackTrace);
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
                Debug.LogWarning("WARN GameManager.SetMetaFloat: CurrentSave not set" + e.StackTrace);
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
                Debug.LogWarning("WARN GameManager.AddMetaInt: CurrentSave not set" + e.StackTrace);
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
                Debug.LogWarning("WARN GameManager.AddMetaFloat: CurrentSave not set" + e.StackTrace);
                displayedNoSaveFile = true;
            }
        }
    }
    #endregion

    #region Debug

    #endregion

    /// <summary>
    /// This function does nothing except allowing to call the GM therefore make it spawn
    /// </summary>
    public void SpawnGameManager()
    {
        Debug.Log("GameManager spawned");
    }
}