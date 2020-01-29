using System.Collections.Generic;
using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    public int chapterIndex;
    public List<LevelManager> levels;
    private int currentLevel = 0; //indice du niveau actuel
    private float timeSinceBegin = 0;

    // Update is called once per frame
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        //Initialisation of the first level
        UpdateEnabledLevels();
        Camera.main.GetComponent<LevelCamera>().MoveTo(levels[currentLevel].cameraPoint.position);
    }

    private void Update()
    {
        timeSinceBegin += Time.deltaTime; //Compter de temps pour la collecte de metadonnées
    }

    /// <summary>
    /// decrease the current level index and teleports the cameara to the position avec the previous level
    /// </summary>
    public void PreviousLevel()
    {
        //collecte des metadonnées du niveau courant avant de changer
        CollectLevelMetaData();

        currentLevel--;

        //Activation du niveau courant et désactivation des autres
        UpdateEnabledLevels();

        if (currentLevel < 0) //il faut faire revenir le joueur au choix des level.
        {
            Debug.Log("Déjà au premier tableau");
        }
        else //on transfert le joueur dans le tableau suivant
        {
            //appel de la fonction pour faire bouger la cam
            Debug.Log("On passe au level précédent : " +currentLevel);
            Camera.main.GetComponent<LevelCamera>().MoveTo(levels[currentLevel].cameraPoint.position);
        }
    }

    /// <summary>
    /// increase the current level index and teleports the cameara to the position avec the next level
    /// </summary>
    public void NextLevel()
    {
        //collecte des metadonnées du niveau courant avant de changer
        CollectLevelMetaData();

        currentLevel++;

        //Activation du niveau courant et désactivation des autres
        UpdateEnabledLevels();


        if (currentLevel == levels.Count) //il faut faire revenir le joueur au choix des level.
        {
            Debug.Log("Dernier level terminé, direction le menu de selection de niveau");
            //fondu au noir
            //loadScene
        }
        else //on transfert le joueur dans le tableau suivant
        {
            //appel de la fonction pour faire bouger la cam
            Debug.Log("On passe au level suivant : " +currentLevel);
            Camera.main.GetComponent<LevelCamera>().MoveTo(levels[currentLevel].cameraPoint.position);
        }
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

    private void CollectLevelMetaData()
    {
        GameManager.Instance.MetaTotalTimePlayed(timeSinceBegin); //temps de jeu total
        GameManager.Instance.MetaAddTimeToLevel(string.Concat(chapterIndex.ToString()+"_", currentLevel.ToString()), timeSinceBegin); //temps de jeu du level
        timeSinceBegin = 0;
    }
}
