using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TargetZone : MonoBehaviour
{
    public int id;
    public GameObject otherDirection;

    [HideInInspector]
    public GameObject skeleton;
    private GameObject particle;

    void Start()
    {
        particle = transform.GetChild(0).gameObject;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (skeleton != null && collision.gameObject == skeleton.GetComponent<Skeleton>().playerTarget
            && skeleton.GetComponent<Skeleton>().isTargetting)
        {
            skeleton.GetComponent<Skeleton>().idTargetZone = id;
            if (!particle.active)
                particle.SetActive(true);   
            
            // When double attack, activate the other direction lane
            if(skeleton.GetComponent<Skeleton>().hp == 1 && otherDirection != null && !otherDirection.GetComponent<TargetZone>().particle.active)
            {
                otherDirection.GetComponent<TargetZone>().particle.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (skeleton != null && collision.gameObject == skeleton.GetComponent<Skeleton>().playerTarget
            && skeleton.GetComponent<Skeleton>().isTargetting)
        {
            if (particle.active)
                particle.SetActive(false);

            // Deactivate other lane when double attack
            if (otherDirection != null && otherDirection.GetComponent<TargetZone>().particle.active)
            {
                otherDirection.GetComponent<TargetZone>().particle.SetActive(false);
            }
        }
    }
}
