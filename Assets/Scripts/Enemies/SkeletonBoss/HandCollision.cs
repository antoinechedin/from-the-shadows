using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HandCollision : MonoBehaviour
{
    public GameObject skeleton;
    public GameObject killzone;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "HurtBoss")
        {
            skeleton.GetComponent<Skeleton>().GetHurt();
            DeactivateCollider();
            Invoke("ActivateCollider", 2);
        }
        else if (collider.gameObject.tag == "Enemy")
        {
            collider.GetComponent<EnemyController>().Die();
            DeactivateCollider();
            Invoke("ActivateCollider", 2);
        }
    }

    public void ActivateCollider()
    {
        if (killzone != null)
            killzone.SetActive(true);
    }

    public void DeactivateCollider()
    {
        if (killzone != null)
            killzone.SetActive(false);
    }
}
