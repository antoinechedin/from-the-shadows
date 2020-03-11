using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class HandCollision : MonoBehaviour
{
    public GameObject skeleton;
    public GameObject killzone;
    public GameObject otherHand;
    private HandCollision otherHandScript;
    [HideInInspector]
    public bool isDestructor = false;
    [HideInInspector]
    public bool isKillable = true;

    public void Start()
    {
        otherHandScript = GetComponent<HandCollision>();
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "HurtBoss" && isKillable)
        {
            // Stop the hand animation
            GetComponent<Animator>().SetTrigger("Die");

            // When hurt, the hand can not kill or be killed
            // For the time to destroy the platforms
            otherHandScript.DeactivateKillZone();
            otherHandScript.isKillable = false;

            // Start the skeleton animation
            skeleton.GetComponent<Skeleton>().GetHurt();


            // Reactivate the killzone and the hand can be killed            
            otherHandScript.Invoke("ActivateKillable", 6);
            otherHandScript.Invoke("ActivateKillZone", 2);
        }
        else if (collider.gameObject.GetComponent<RotatingPlatform>() != null && isKillable)
        {
            // When the hand hurt a rotating spike from behind
            // the hand can not be killed for 2 seconds to avoid the spikes
            // the platform rotates
            Debug.Log("pouet");
            isKillable = false;
            collider.gameObject.GetComponent<RotatingPlatform>().OnHit();
            Invoke("ActivateKillable", 2);
        }

        if (collider.gameObject.GetComponent<DestructiblePlatform>() != null && isDestructor)
        {
            // When the hand destroys a platform
            collider.gameObject.GetComponent<DestructiblePlatform>().StartCoroutine("Destruct");
        }

        if (collider.gameObject.tag == "stopBoss")
        {
            // When the hand is stopped by an obstacle but is not hurt
            GetComponent<Animator>().SetTrigger("Die");
        }
    }

    public void ActivateKillZone()
    {
        if (killzone != null)
            killzone.SetActive(true);
    }

    public void DeactivateKillZone()
    {
        if (killzone != null)
            killzone.SetActive(false);
    }

    public void ActivateKillable()
    {
        Debug.Log("hand invicibility over");
        isKillable = true;
    }
}
