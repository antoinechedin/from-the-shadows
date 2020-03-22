using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillZone : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().Die();
        }
        else if (collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyController>().Die();
        }
    }
}
