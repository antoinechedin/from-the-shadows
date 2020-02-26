using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBossHandCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerController>().Die();
        }
    }
}
