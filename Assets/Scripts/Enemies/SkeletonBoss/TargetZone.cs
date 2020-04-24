using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TargetZone : MonoBehaviour
{
    public int id;

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
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (skeleton != null && collision.gameObject == skeleton.GetComponent<Skeleton>().playerTarget
            && skeleton.GetComponent<Skeleton>().isTargetting)
        {
            if (particle.active)
                particle.SetActive(false);
        }
    }
}
