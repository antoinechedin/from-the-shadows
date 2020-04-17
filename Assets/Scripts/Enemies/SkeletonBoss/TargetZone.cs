using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TargetZone : MonoBehaviour
{
    public int id;

    [HideInInspector]
    public GameObject playerTarget;
    public GameObject skeleton;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == playerTarget && skeleton != null)
        {
            skeleton.GetComponent<Skeleton>().idTargetZone = id;
            Debug.Log("Zone à attaquer : "+ id);
        }
    }


}
