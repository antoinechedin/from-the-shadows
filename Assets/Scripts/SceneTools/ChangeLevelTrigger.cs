using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevelTrigger : MonoBehaviour
{
    public enum Type { Previous, Next };

    public Type type;
    public int nbNecessaryPlayers;
    public GameObject newPlayerSpawns;

    private ChapterManager chapterManager;
    private int nbPlayerInTheTrigger = 0;

    void Start()
    {
        chapterManager = GameObject.Find("ChapterManager").GetComponent<ChapterManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            nbPlayerInTheTrigger++;

            if (nbPlayerInTheTrigger == nbNecessaryPlayers)
            {
                if (type == Type.Next)
                {
                    chapterManager.NextLevel();
                }
                else if (type == Type.Previous)
                {
                    chapterManager.PreviousLevel();
                }

                if (newPlayerSpawns != null)
                {
                    chapterManager.CurrentSpawns = newPlayerSpawns;
                }
                else
                {
                    Debug.Log("Aucun spawnPoint assigné à ce trigger. Le spawn des joueurs n'a pas changé");
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
        nbPlayerInTheTrigger = 0;
    }
}
