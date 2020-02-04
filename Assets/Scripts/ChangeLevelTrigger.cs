using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevelTrigger : MonoBehaviour
{
    public enum Type { Previous, Next };

    public Type type;
    public int nbNecessaryPlayers;

    private ChapterManager chapterManager;
    private int nbPlayerInTheTrigger = 0;

    // Start is called before the first frame update
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
}
