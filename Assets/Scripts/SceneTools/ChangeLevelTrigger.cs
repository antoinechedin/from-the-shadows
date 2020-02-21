using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevelTrigger : MonoBehaviour
{
    public enum Type { Previous, Next };

    public Type type;
    public int nbNecessaryPlayers;
    public GameObject newPlayerSpawns;
    [Header("(OPTIONAL. -1 by default. Set it to 1000 to finish the chapter)")]
    public int newCurrentLevel = -1;

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
                    if (newCurrentLevel == -1)
                        chapterManager.NextLevel();
                    else
                        chapterManager.NextLevel(newCurrentLevel);

                }
                else if (type == Type.Previous)
                {
                    if (newCurrentLevel == -1)
                        chapterManager.PreviousLevel();
                    else
                        chapterManager.PreviousLevel(newCurrentLevel);
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
