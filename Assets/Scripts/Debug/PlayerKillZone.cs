using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillZone : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            int pId = collision.GetComponent<PlayerController>().input.id;
            Debug.Log("Le joueur " + pId + " est mort");
            GameObject.FindObjectOfType<ChapterManager>().ResetLevel(pId);
        }
    }
}
