using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HandCollision : MonoBehaviour
{
    public GameObject skeleton;
    public GameObject killzone;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "HurtBoss")
        {
            skeleton.GetComponent<Skeleton>().GetHurt();
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
