using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevelTrigger : MonoBehaviour
{
    public int nbNecessaryPlayers;
    public GameObject newPlayerSpawns;
    public bool finishChapter = false;
    [Header("Define whether the players will be tp'd to the \"newPlayerSpawn\" or not")]
    public bool tpPlayers;

    private ChapterManager chapterManager;
    private int nbPlayerInTheTrigger = 0;
    private int newCurrentLevel;

    void Start()
    {
        chapterManager = GameObject.Find("ChapterManager").GetComponent<ChapterManager>();

        if (newPlayerSpawns == null && !finishChapter)
        {
            Debug.LogError(name + " : Aucun spawnPoint assigné à " + Utils.GetFullName(transform)
                + ". Le spawn des joueurs n'a pas changé");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            nbPlayerInTheTrigger++;

            //si tous les joueurs sont dans le trigger
            if (nbPlayerInTheTrigger == nbNecessaryPlayers)
            {
                //si c'est la fin du chapitre
                if (finishChapter)
                {
                    GameObject.Find("MusicManager").GetComponent<MusicManager>().StopTheme();
                    chapterManager.FinishChapter();
                }
                //sinon on passe au niveau suivant
                else
                {
                    chapterManager.ChangeLevel(newPlayerSpawns.GetComponentInParent<LevelManager>().id, tpPlayers);
                    if (newPlayerSpawns != null)
                        chapterManager.CurrentSpawns = newPlayerSpawns;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            nbPlayerInTheTrigger--;
        }
    }

    private void OnDisable()
    {
        // nbPlayerInTheTrigger = 0;
    }
}
