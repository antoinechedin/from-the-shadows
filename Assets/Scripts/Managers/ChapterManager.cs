using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Diagnostics;
using TMPro;
using System;

public class ChapterManager : MonoBehaviour
{
    public List<LevelManager> levels;
    public PauseMenu pauseMenu;
    private int currentLevel = 0; // indice du niveau actuel

    private MusicManager musicManager;

    private float timeSinceBegin = 0;
    private GameObject currentSpawns;

    private bool resetingLevel = false;

    private float totalTimePlayed = 0;

    public GameObject CurrentSpawns
    {
        get { return currentSpawns; }
        set { currentSpawns = value; }
    }

    private void Awake()
    {
        SetLevelsId();
    }
    // Update is called once per frame
    void Start()
    {
        totalTimePlayed = GameManager.Instance.GetMetaFloat(MetaTag.TOTAL_TIME_PLAYED);

        if (GameObject.Find("MusicManager") != null)
            musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();

        if (GameManager.Instance.LoadingChapterInfo != null)
        {
            currentLevel = GameManager.Instance.LoadingChapterInfo.StartLevelIndex;

            if (musicManager != null)
                musicManager.SetMusicAccordingToLevel(GameManager.Instance.LoadingChapterInfo.StartLevelIndex, levels);
        }

        levels[currentLevel].virtualCamera.gameObject.SetActive(true);
        Camera.main.GetComponent<CameraManager>().cameraTarget.GetComponent<CameraTarget>().Offset = levels[currentLevel].cameraOffset;


        currentSpawns = levels[currentLevel].playerSpawns[0];
        SpawnPlayers();

        //collectibles
        if (GameManager.Instance.CurrentChapter != -1)
        {
            Level currentLvl = GameManager.Instance.GetCurrentChapter().GetLevels()[currentLevel];
            levels[currentLevel].SetCollectibles(currentLvl.LightCollectibles, currentLvl.ShadowCollectibles);
        }

        for (int i = 0; i < levels.Count; i++)
        {
            if (i != currentLevel) levels[i].DisableThisLevelOnly();
        }
        levels[currentLevel].EnableLevel();
    }

    private void Update()
    {
        timeSinceBegin += Time.deltaTime; //Compter de temps pour la collecte de metadonnées
        TextMeshProUGUI timeText = GameObject.Find("SpeedRunTime").GetComponent<TextMeshProUGUI>();
        if (timeText != null && PlayerPrefs.GetInt("SpeedRun") == 1)
        {
            float time = totalTimePlayed + timeSinceBegin;
            int secondes = Mathf.FloorToInt(time) % 60;
            int minutes = Mathf.FloorToInt(time) / 60;
            int mili = Mathf.FloorToInt(time * 1000) % 1000;
            timeText.text = minutes.ToString() + ":" + secondes.ToString() + ":" + mili.ToString();
        }

        // Position moyenne des deux joueurs
        //if (Input.GetButtonDown("Start_G"))
        if (!GameManager.Instance.Loading && InputManager.GetActionPressed(0, InputAction.Pause))
        {
            pauseMenu.gameObject.SetActive(true);
            pauseMenu.OpenPauseMenu();
            pauseMenu.StopAllSounds(musicManager, levels[currentLevel]);
        }

        //if (Input.GetButtonDown("Select_G"))
        // if (InputManager.GetActionPressed(0, InputAction.Restart))
        // {
        //     GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Die();
        // }

        #region CheatCodes
        //next level
        if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.N))
        {
            if (currentLevel + 1 < levels.Count)
            {
                levels[currentLevel].gameObject.SetActive(false);
                ChangeLevel(currentLevel + 1, true);
                currentSpawns = levels[currentLevel].playerSpawns[0];
                SpawnPlayers();
            }
        }
        //previous level
        if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.B))
        {
            if (currentLevel - 1 >= 0)
            {
                levels[currentLevel].gameObject.SetActive(false);
                ChangeLevel(currentLevel - 1, true);
                currentSpawns = levels[currentLevel].playerSpawns[0];
                SpawnPlayers();
            }
        }
        //kill players
        if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.K) && !GameManager.Instance.IsInCutscene)
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
    public void ChangeLevel(int newCurrentLevel, bool tpPlayers)
    {
        // Mise à jour des infos concernant le niveau courant
        if (GameManager.Instance.CurrentChapter != -1)
            ValidateCollectibles();
        GameManager.Instance.SetLevelCompleted(GameManager.Instance.CurrentChapter, currentLevel);

        if (musicManager.currentPlayingTheme != null)
            musicManager.ManageMusicChange(currentLevel, newCurrentLevel);

        List<LevelManager> levelsToDisable = new List<LevelManager>();
        foreach (LevelManager lm in levels[currentLevel].roomsToEnable)
        {
            if (!levels[newCurrentLevel].roomsToEnable.Contains(lm) && levels[newCurrentLevel] != lm)
            {
                levelsToDisable.Add(lm);
            }
        }
        foreach (LevelManager lm in levelsToDisable)
        {
            lm.gameObject.SetActive(false);
        }

        // On désactive le niveau actuel et ses voisins
        // levels[currentLevel].DisableLevel();

        levels[newCurrentLevel].virtualCamera.gameObject.SetActive(true);
        levels[currentLevel].virtualCamera.gameObject.SetActive(false);


        currentLevel = newCurrentLevel;

        Camera.main.GetComponent<CameraManager>().cameraTarget.GetComponent<CameraTarget>().Offset = levels[currentLevel].cameraOffset;

        // On active la nouvelle room et ses voisins
        levels[currentLevel].EnableLevel();

        CollectMetaData();

        if (GameManager.Instance.CurrentChapter != -1)
        {
            Level currentLvl = GameManager.Instance.GetCurrentChapter().GetLevels()[currentLevel];
            levels[currentLevel].SetCollectibles(currentLvl.LightCollectibles, currentLvl.ShadowCollectibles);
        }

        if (tpPlayers)
        {
            StartCoroutine(TpPlayersWithTransitionScreen());
        }

        SaveManager.Instance.WriteSaveFile();
    }

    /// <summary>
    /// Tps the players to the current spawn point and displays a transition screen
    /// </summary>
    /// <returns></returns>
    public IEnumerator TpPlayersWithTransitionScreen()
    {
        //on affiche l'écran de transition de mort
        GameObject transitionScreen = (GameObject)Resources.Load("SwipeTransition"); //load le prefab
        transitionScreen = Instantiate(transitionScreen, gameObject.transform); //l'affiche

        //tant que l'ecran n'a pas fini de fade au noir
        while (!transitionScreen.GetComponent<TransitionScreen>().finishedFadingIn)
        {
            yield return null;
        }

        currentSpawns = levels[currentLevel].playerSpawns[0];
        SpawnPlayers();
    }

    public void FinishChapter()
    {
        ValidateCollectibles();
        GameManager.Instance.SetLevelCompleted(GameManager.Instance.CurrentChapter, currentLevel);
        //Save the metaData
        CollectMetaData();
        SaveManager.Instance.WriteSaveFile();
        GameManager.Instance.LoadMenu("MainMenu", new LoadingMenuInfo(2, GameManager.Instance.CurrentChapter));
    }

    public void ValidateCollectibles()
    {
        //Validate light collectibles
        for (int i = 0; i < levels[currentLevel].lightCollectibles.Count; i++)
        {
            Collectible collectible = levels[currentLevel].lightCollectibles[i].GetComponent<Collectible>();
            if (collectible.isPickedUp)
            {
                collectible.isValidated = true;
                GameManager.Instance.SaveCollectibleTaken(GameManager.Instance.CurrentChapter, currentLevel, Collectible.Type.Light, i);
            }
        }
        /*foreach (GameObject go in levels[currentLevel].lightCollectibles)
        {
            Collectible collectible = go.GetComponent<Collectible>();
            if (collectible.isPickedUp)
            {
                collectible.isValidated = true;
                GameManager.Instance.SaveCollectibleTaken(GameManager.Instance.CurrentChapter, currentLevel, Collectible.Type.Light, go.transform.GetSiblingIndex());
            }
        }*/


        for (int i = 0; i < levels[currentLevel].shadowCollectibles.Count; i++)
        {
            Collectible collectible = levels[currentLevel].shadowCollectibles[i].GetComponent<Collectible>();
            if (collectible.isPickedUp)
            {
                collectible.isValidated = true;
                GameManager.Instance.SaveCollectibleTaken(GameManager.Instance.CurrentChapter, currentLevel, Collectible.Type.Shadow, i);
            }
        }
        //Validate shadow collectibles
        /*foreach (GameObject go in levels[currentLevel].shadowCollectibles)
        {
            Collectible collectible = go.GetComponent<Collectible>();
            if (collectible.isPickedUp)
            {
                collectible.isValidated = true;
                GameManager.Instance.SaveCollectibleTaken(GameManager.Instance.CurrentChapter, currentLevel, Collectible.Type.Shadow, go.transform.GetSiblingIndex());
            }
        }*/
    }

    public void CollectMetaData()
    {
        GameManager.Instance.AddMetaFloat(MetaTag.TOTAL_TIME_PLAYED, timeSinceBegin); //collecte du temps de jeu
        timeSinceBegin = 0;
        totalTimePlayed = GameManager.Instance.GetMetaFloat(MetaTag.TOTAL_TIME_PLAYED);
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
        // Reset l'offset de la camera
        Camera.main.GetComponent<CameraManager>().cameraTarget.GetComponent<CameraTarget>().Offset = levels[currentLevel].cameraOffset;

        //tant que l'ecran n'a pas fini de fade au noir
        while (!transitionScreen.GetComponent<TransitionScreen>().finished)
        {
            yield return null;
        }

        //on reactive les inputs des joueurs
        foreach (PlayerController p in players)
        {
            p.dead = false;
            p.dying = false;
            p.input.active = true;
        }

        resetingLevel = false;
    }

    public void ShakeFor(float amplitude, float frequency, float time)
    {
        StartCoroutine(ShakeForAsync(amplitude, frequency, time));
    }

    public IEnumerator ShakeForAsync(float amplitude, float frequency, float time)
    {
        StartCameraShake(amplitude, frequency);
        yield return new WaitForSeconds(time);
        StopCameraShake();
    }

    public void StartCameraShake(float amplitude, float frequency)
    {
        levels[currentLevel].virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        levels[currentLevel].virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
    }

    public void StopCameraShake()
    {
        levels[currentLevel].virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
    }
}
