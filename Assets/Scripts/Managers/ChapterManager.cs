using System.Collections.Generic;
using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    public int chapterIndex;
    public List<LevelManager> levels;
    public PauseMenu pauseMenu;
    private int currentLevel = 0; //indice du niveau actuel
    private float timeSinceBegin = 0;

    // Update is called once per frame
    void Start()
    {
        if (GameManager.Instance.LoadingChapterInfo != null)
        {
            currentLevel = GameManager.Instance.LoadingChapterInfo.StartLevelIndex;
        }

        Camera.main.GetComponent<LevelCamera>().MoveTo(levels[currentLevel].cameraPoint.position);
        SpawnPlayer(levels[currentLevel].playerSpawn.position);

        UpdateEnabledLevels();
    }

    private void Update()
    {
        timeSinceBegin += Time.deltaTime; //Compter de temps pour la collecte de metadonnées

        if (Input.GetButtonDown("Start_G"))
        {
            pauseMenu.gameObject.SetActive(true);
            pauseMenu.OpenPauseMenu();
        }
    }

    public void SpawnPlayer(Vector3 pos)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            player.transform.position = pos;
        }
    }

    /// <summary>
    /// decrease the current level index and teleports the cameara to the position avec the previous level
    /// </summary>
    public void PreviousLevel()
    {
        currentLevel--;

        //Activation du niveau courant et désactivation des autres
        UpdateEnabledLevels();

        if(currentLevel >= 0) //on bouge la cam dans le tableau précédent
        {
            Camera.main.GetComponent<LevelCamera>().MoveTo(levels[currentLevel].cameraPoint.position);
        }
    }

    /// <summary>
    /// increase the current level index and teleports the cameara to the position avec the next level
    /// </summary>
    public void NextLevel()
    {
        //Mise àjour des infos concernant le niveau courant
        GameManager.Instance.SetLevelCompleted(chapterIndex, currentLevel);
        currentLevel++;

        //Activation du niveau courant et désactivation des autres
        UpdateEnabledLevels();


        if (currentLevel == levels.Count) //Le chapitre est terminé
        {
            //Save the metaData
            CollectMetaData();
            SaveManager.Instance.WriteSaveFile();
            GameManager.Instance.LoadMenu("MainMenu", new LoadingMenuInfo(2));
        }
        else //on transfert le joueur dans le tableau suivant
        {
            //appel de la fonction pour faire bouger la cam
            Camera.main.GetComponent<LevelCamera>().MoveTo(levels[currentLevel].cameraPoint.position);
        }

        SaveManager.Instance.WriteSaveFile();
    }

    public void UpdateEnabledLevels()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            if (i == currentLevel)
            {
                levels[i].EnableLevel();
            }
            else
            {
                levels[i].DisableLevel();
            }
        }
    }

    private void CollectMetaData()
    {
        GameManager.Instance.AddMetaFloat("totalTimePlayed", timeSinceBegin); //collecte du temps de jeu
        timeSinceBegin = 0;
    }
}
