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

        if (newPlayerSpawns == null)
        {
            Debug.Log(name + " : Aucun spawnPoint assigné à ce trigger. Le spawn des joueurs n'a pas changé");
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
                    chapterManager.FinishChapter();
                }
                //sinon on passe au niveau suivant
                else
                {
                    chapterManager.ChangeLevel(newPlayerSpawns.GetComponentInParent<LevelManager>().id);
                    if (newPlayerSpawns != null)
                        chapterManager.CurrentSpawns = newPlayerSpawns;

                    //si la téléportation des joueurs est activée
                    if (tpPlayers)
                    {
                        StartCoroutine(TpPlayers());
                    }
                }
            }
        }
    }

    public IEnumerator TpPlayers()
    {
        GameObject transitionScreen = (GameObject)Resources.Load("SwipeTransition"); //load le prefab
        transitionScreen = Instantiate(transitionScreen, gameObject.transform); //l'affiche

        //tant que l'ecran n'a pas fini de fade au noir
        while (!transitionScreen.GetComponent<TransitionScreen>().finishedFadingIn)
        {
            yield return null;
        }
        //tp les joueurs
        chapterManager.SpawnPlayers();
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
        nbPlayerInTheTrigger = 0;
    }
}
