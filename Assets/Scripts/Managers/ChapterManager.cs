using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    public List<LevelManager> levels;
    public PauseMenu pauseMenu;
    private int currentLevel = 0; //indice du niveau actuel

    private float timeSinceBegin = 0;
    private LevelCamera levelCamera;
    private GameObject currentSpawns;

    private bool resetingLevel = false;

    public GameObject CurrentSpawns
    {
        get { return currentSpawns; }
        set { currentSpawns = value; }
    }

    // Update is called once per frame
    void Start()
    {
        SetLevelsId();

        if (GameManager.Instance.LoadingChapterInfo != null)
        {
            currentLevel = GameManager.Instance.LoadingChapterInfo.StartLevelIndex;
        }

        levelCamera = Camera.main.GetComponent<LevelCamera>();
        CreateEmptyCameraPoints();
        levelCamera.SetLimit(levels[currentLevel].cameraLimitLB, levels[currentLevel].cameraLimitRT);
        levelCamera.MoveTo((levels[currentLevel].cameraLimitRT.position + levels[currentLevel].cameraLimitLB.position) / 2, false);

        currentSpawns = levels[currentLevel].playerSpawns[0];
        SpawnPlayers();

        //collectibles
        if(GameManager.Instance.CurrentChapter != -1)
        {
            Level currentLvl = GameManager.Instance.GetCurrentChapter().GetLevels()[currentLevel];
            levels[currentLevel].SetCollectibles(currentLvl.LightCollectibles, currentLvl.ShadowCollectibles);
        }

        //UpdateEnabledLevels();
        foreach (LevelManager level in levels)
        {
            level.DisableLevel();
        }
        levels[currentLevel].EnableLevel();
    }

    private void Update()
    {
        timeSinceBegin += Time.deltaTime; //Compter de temps pour la collecte de metadonnées

        // Position moyenne des deux joueurs

        if (levelCamera.StayInLimits)
        {
            Vector3 meanPosition = new Vector3();
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            meanPosition = (players[0].transform.position + players[1].transform.position) / 2;
            Camera.main.GetComponent<LevelCamera>().MoveTo(meanPosition);
        }

        if (Input.GetButtonDown("Start_G"))
        {
            pauseMenu.gameObject.SetActive(true);
            pauseMenu.OpenPauseMenu();
        }

        if (Input.GetButtonDown("Select_G"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Die();
        }

        #region CheatCodes
        //next level
        if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.N))
        {
            if (currentLevel + 1 < levels.Count)
            {
                ChangeLevel(currentLevel + 1);
                currentSpawns = levels[currentLevel].playerSpawns[0];
                SpawnPlayers();
            }
        }
        //previous level
        if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.B))
        {
            if (currentLevel - 1 >= 0)
            {
                ChangeLevel(currentLevel - 1);
                currentSpawns = levels[currentLevel].playerSpawns[0];
                SpawnPlayers();
            }
        }
        //kill players
        if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.K))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Die();
        }
        #endregion
    }

    /// <summary>
    /// Sets the LevelManager.id value according to the position of the level in the ChapterManager.levels list.
    /// </summary>
    public void SetLevelsId()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].id = i;
        }
    }

    /// <summary>
    /// Tps the players to the currentSpawn position. The spawn position is set in Next et PreviousLevel() so it changes at run time.
    /// </summary>
    /// <param name="spawns"></param>
    public void SpawnPlayers()
    {
        //Find the spawns positions in the children
        Vector3 lightSpawnPos = currentSpawns.transform.GetChild(0).transform.position;
        Vector3 shadowSpawnPos = currentSpawns.transform.GetChild(1).transform.position;

        //Find the players
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<PlayerInput>().id == 1) //Light
                player.GetComponent<PlayerController>().SpawnAt(lightSpawnPos);
            else if (player.GetComponent<PlayerInput>().id == 2)//shadow
                player.GetComponent<PlayerController>().SpawnAt(shadowSpawnPos);
        }
    }

    /// <summary>
    /// Saves the current level and sets the new current level to "newCurrentLevel" in parameters
    /// </summary>
    public void ChangeLevel(int newCurrentLevel)
    {
        //Mise àjour des infos concernant le niveau courant
        if(GameManager.Instance.CurrentChapter != -1)
            ValidateCollectibles();
        GameManager.Instance.SetLevelCompleted(GameManager.Instance.CurrentChapter, currentLevel);

        //on désactive le nouveau actuel et ses voisins
        levels[currentLevel].DisableLevel();

        currentLevel = newCurrentLevel;

        //On active la nouvelle room et ses voisins
        levels[currentLevel].EnableLevel();

        CollectMetaData();
        CreateEmptyCameraPoints();
        levelCamera.SetLimit(levels[currentLevel].cameraLimitLB, levels[currentLevel].cameraLimitRT);

        //collectibles
        if (GameManager.Instance.CurrentChapter != -1)
        {
            Level currentLvl = GameManager.Instance.GetCurrentChapter().GetLevels()[currentLevel];
            levels[currentLevel].SetCollectibles(currentLvl.LightCollectibles, currentLvl.ShadowCollectibles);
        }


        SaveManager.Instance.WriteSaveFile();
    }

    public void FinishChapter()
    {
        //Save the metaData
        CollectMetaData();
        SaveManager.Instance.WriteSaveFile();
        GameManager.Instance.LoadMenu("MainMenu", new LoadingMenuInfo(2));
    }

    public void ValidateCollectibles()
    {
        //Validate light collectibles
        foreach (GameObject go in levels[currentLevel].lightCollectibles)
        {
            Collectible collectible = go.GetComponent<Collectible>();
            if (collectible.isPickedUp)
            {
                collectible.isValidated = true;
                GameManager.Instance.SaveCollectibleTaken(GameManager.Instance.CurrentChapter, currentLevel,Collectible.Type.Light, go.transform.GetSiblingIndex());
            }
        }

        //Validate shadow collectibles
        foreach (GameObject go in levels[currentLevel].shadowCollectibles)
        {
            Collectible collectible = go.GetComponent<Collectible>();
            Debug.Log("collectible d'ombre : " + collectible.isPickedUp);
            if (collectible.isPickedUp)
            {
                collectible.isValidated = true;
                GameManager.Instance.SaveCollectibleTaken(GameManager.Instance.CurrentChapter, currentLevel, Collectible.Type.Shadow, go.transform.GetSiblingIndex());
            }
        }
    }

    public void CreateEmptyCameraPoints()
    {
        if (levels[currentLevel].cameraLimitLB == null)
        {
            GameObject emptyPoint = new GameObject();
            emptyPoint.transform.position = new Vector3(-5f, 5f, -10f);
            levels[currentLevel].cameraLimitLB = emptyPoint.transform;
        }
        if (levels[currentLevel].cameraLimitRT == null)
        {
            GameObject emptyPoint = new GameObject();
            emptyPoint.transform.position = new Vector3(-5f, 5f, -10f);
            levels[currentLevel].cameraLimitRT = emptyPoint.transform;
        }
    }

    public void CollectMetaData()
    {
        GameManager.Instance.AddMetaFloat(MetaTag.TOTAL_TIME_PLAYED, timeSinceBegin); //collecte du temps de jeu
        timeSinceBegin = 0;
    }

    /// <summary>
    /// The player died : displays all deaths animations (player, screen, etc...) and reset all Resetable Objects
    /// </summary>
    /// <param name="playerId"></param>
    public void ResetLevel(int playerId)
    {
        if (!resetingLevel)
        {
            resetingLevel = true;
            StartCoroutine(ResetLevelAsync(playerId));
        }
    }

    /// <summary>
    /// The player died : displays all deaths animations (player, screen, etc...) and reset all Resetable Objects
    /// </summary>
    /// <param name="playerId"></param>
    IEnumerator ResetLevelAsync(int playerId)
    {
        //on récupère le joueur concerné
        PlayerController[] players = GameObject.FindObjectsOfType<PlayerController>();
        PlayerController player = null;
        foreach (PlayerController p in players)
        {
            if (p.input.id == playerId)
            {
                player = p;
            }
        }

        //on attend que le joueur soit mort. ie : que l'animation soit terminée
        while (!player.GetComponent<PlayerController>().dead)
        {
            yield return null;
        }

        //on affiche l'écran de transition de mort
        GameObject transitionScreen = (GameObject)Resources.Load("SwipeTransition"); //load le prefab
        transitionScreen = Instantiate(transitionScreen, gameObject.transform); //l'affiche

        //tant que l'ecran n'a pas fini de fade au noir
        while (!transitionScreen.GetComponent<TransitionScreen>().finishedFadingIn)
        {
            yield return null;
        }
        //On fait ci dessous tout ce qui intervient pendant que l'écran est noir

        //Teleporte les joueurs au début du jeu
        SpawnPlayers();
        //téléporte la camera à sa position de départ
        //Incrémente la meta donnée du joueur mort
        GameManager.Instance.AddMetaInt(playerId == 1 ? MetaTag.PLAYER_1_DEATH : MetaTag.PLAYER_2_DEATH, 1);
        //Reset tous les objets Resetables
        levels[currentLevel].ResetAllResetables();
        //on remet Player.dead à false
        player.dead = false;
        player.dying = false;

        //tant que l'ecran n'a pas fini de fade au noir
        while (!transitionScreen.GetComponent<TransitionScreen>().finished)
        {
            yield return null;
        }

        player.input.active = true;

        resetingLevel = false;
    }
}
