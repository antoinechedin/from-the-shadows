using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousLevelTrigger : MonoBehaviour
{
    private ChapterManager chapterManager;

    // Start is called before the first frame update
    void Start()
    {
        chapterManager = GameObject.Find("ChapterManager").GetComponent<ChapterManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            chapterManager.PreviousLevel();
        }
    }
}
