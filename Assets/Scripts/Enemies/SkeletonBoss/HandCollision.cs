using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class HandCollision : MonoBehaviour
{
    public GameObject skeleton;
    public GameObject killzone;
    [HideInInspector]
    public bool isDestructor = false;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "HurtBoss")
        {
            skeleton.GetComponent<Skeleton>().GetHurt();
            GetComponent<Animator>().SetTrigger("Die");
            DeactivateCollider();
            Invoke("ActivateCollider", 2);
        }
        /* else if (collider.gameObject.tag == "Enemy")
        {
            collider.GetComponent<EnemyController>().Die();
            DeactivateCollider();
            Invoke("ActivateCollider", 2);
        } */
        if (collider.gameObject.GetComponent<DestructiblePlatform>() != null && isDestructor)
        {
            DeactivateCollider();
            collider.gameObject.GetComponent<DestructiblePlatform>().StartCoroutine("Destruct");
            Invoke("ActivateCollider", 2);
        }

        if (collider.gameObject.GetComponent<RotatingPlatform>() != null)
        {
            DeactivateCollider();
            collider.gameObject.GetComponent<RotatingPlatform>().OnHit();
            Invoke("ActivateCollider", 1);
        }

        if (collider.gameObject.tag == "stopBoss")
        {
            GetComponent<Animator>().SetTrigger("Die");
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
