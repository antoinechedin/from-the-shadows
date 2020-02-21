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

    public GameObject CurrentSpawns
    {
        get { return currentSpawns; }
        set { currentSpawns = value; }
    }

    // Update is called once per frame
    void Start()
    {
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

        if(GameManager.Instance.CurrentChapter != -1)
            levels[currentLevel].SetCollectibles(GameManager.Instance.GetCurrentChapter().GetLevels()[currentLevel].Collectibles);

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
        if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.N))
        {
            NextLevel();
        }

        if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.K))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Die();
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
                player.transform.position = lightSpawnPos;
            else if (player.GetComponent<PlayerInput>().id == 2)//shadow
                player.transform.position = shadowSpawnPos;
        }
    }

    /// <summary>
    /// decrease the current level index and teleports the cameara to the position avec the previous level
    /// </summary>
    public void PreviousLevel(int newCurrentLevel = -1)
    {
        //on désactive la room actuelle et ses voisins
        levels[currentLevel].DisableLevel();

        //on décrémente l'indice de la room actuelle
        if (newCurrentLevel != -1)
            currentLevel = newCurrentLevel;
        else
            currentLevel--;


        //On active la nouvelle room et ses voisins
        levels[currentLevel].EnableLevel();

        if (currentLevel >= 0) //on bouge la cam dans le tableau précédent
        {
            CreateEmptyCameraPoints();
            levelCamera.SetLimit(levels[currentLevel].cameraLimitLB, levels[currentLevel].cameraLimitRT);
        }
    }

    /// <summary>
    /// increase the current level index and teleports the cameara to the position avec the next level
    /// </summary>
    public void NextLevel(int newCurrentLevel = -1)
    {
        //Mise àjour des infos concernant le niveau courant
        if(GameManager.Instance.CurrentChapter != -1)
            ValidateCollectibles();
        GameManager.Instance.SetLevelCompleted(GameManager.Instance.CurrentChapter, currentLevel);

        //on désactive le nouveau actuel et ses voisins
        levels[currentLevel].DisableLevel();

        if (newCurrentLevel != -1)
            currentLevel = newCurrentLevel;
        else
            currentLevel++;


        //On active la nouvelle room et ses voisins
        if (newCurrentLevel != 1000)
            levels[currentLevel].EnableLevel();


        if (currentLevel == levels.Count || newCurrentLevel == 1000) //Le chapitre est terminé
        {
            //Save the metaData
            CollectMetaData();
            SaveManager.Instance.WriteSaveFile();
            GameManager.Instance.LoadMenu("MainMenu", new LoadingMenuInfo(2));
        }
        else //on transfert le joueur dans le tableau suivant
        {
            CollectMetaData();
            SaveManager.Instance.WriteSaveFile();
            CreateEmptyCameraPoints();
            levelCamera.SetLimit(levels[currentLevel].cameraLimitLB, levels[currentLevel].cameraLimitRT);
            if (GameManager.Instance.CurrentChapter != -1)
                levels[currentLevel].SetCollectibles(GameManager.Instance.GetCurrentChapter().GetLevels()[currentLevel].Collectibles);
        }

        SaveManager.Instance.WriteSaveFile();
    }

    public void ValidateCollectibles()
    {
        foreach (GameObject go in levels[currentLevel].collectibles)
        {
            Collectible collectible = go.GetComponent<Collectible>();
            if (collectible.isPickedUp)
            {
                collectible.isValidated = true;
                GameManager.Instance.SaveCollectibleTaken(GameManager.Instance.CurrentChapter, currentLevel, go.transform.GetSiblingIndex());
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
        StartCoroutine(ResetLevelAsync(playerId));
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
    }
}
