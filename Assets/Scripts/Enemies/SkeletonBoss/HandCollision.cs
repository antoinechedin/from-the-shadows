using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class HandCollision : MonoBehaviour
{
    public GameObject skeleton;
    public GameObject otherHand;
    public GameObject replacingHand;
    private HandCollision otherHandScript;
    [HideInInspector]
    public bool isDestructor = false;
    [HideInInspector]
    public bool isKillable = true;

    public AudioSource audioSource;
    public List<AudioClip> soundHandStart;

    public AudioClip soundVerticalDestruction;

    public List<AudioClip> soundDestruction;

    public void Start()
    {
        otherHandScript = GetComponent<HandCollision>();
        audioSource = GetComponent<AudioSource>();
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "StopBoss")
        {
            // When the hand is stopped by an obstacle but is not hurt
            Debug.Log("Case Stop Hand");
            StopHand();
        }
        else if (collider.gameObject.tag == "HurtBoss" && isKillable)
        {
            Debug.Log("Case Hurt Boss");

            collider.GetComponentInParent<DestructiblePlatform>().Destruct();
            // Stop the hand animation
            this.GetComponent<Animator>().SetTrigger("Die");

            if (replacingHand.activeSelf)
                Invoke("ReplaceHand", 2f);

            // When hurt, the hand can not kill or be killed
            // For the time to destroy the platforms
            otherHandScript.isKillable = false;

            // Start the skeleton animation
            skeleton.GetComponent<Skeleton>().GetHurt();


            // Reactivate the hand can be killed            
            otherHandScript.Invoke("ActivateKillable", 6);
        }
        else if (collider.gameObject.GetComponent<RotatingPlatform>() != null && isKillable)
        {
            Debug.Log("Case Rotate Spike");
            // When the hand hurt a rotating spike from behind
            // the hand can not be killed for 2 seconds to avoid the spikes
            // the platform rotates
            isKillable = false;
            collider.gameObject.GetComponent<RotatingPlatform>().OnHit();
            Invoke("ActivateKillable", 2);
        }
        else if (collider.gameObject.tag == "Player" && !isDestructor)
        {
            Debug.Log("Case kill player");
            StopHand();
            collider.GetComponent<PlayerController>().Die();            
        }

        if (collider.gameObject.GetComponent<DestructiblePlatform>() != null && isDestructor)
        {
            Debug.Log("Case destruction");
            // When the hand destroys a platform
            collider.gameObject.GetComponent<DestructiblePlatform>().StartCoroutine("Destruct");

            if(collider.gameObject.GetComponent<DestructiblePlatform>().mustPlaySound)
                audioSource.PlayOneShot(soundDestruction[Random.Range(0, soundDestruction.Count - 1)]);
        }
    }

    public void SetDestructor()
    {
        isDestructor = true;
    }

    public void ReplaceHand()
    {
        Debug.Log(replacingHand);
        replacingHand.GetComponent<Animator>().SetTrigger("ReplaceHand");
        Invoke("HandReappear", 1.5f);
    }
    public void HandReappear()
    {
        replacingHand.SetActive(false);
        GetComponent<Animator>().SetTrigger("Idle");
    }

    public void ActivateKillable()
    {
        isKillable = true;
    }

    public void Reset()
    {
        CancelInvoke();
        StopHand();
        audioSource.Stop();
        replacingHand.SetActive(true);
        replacingHand.GetComponent<Animator>().SetTrigger("Reset");
    }
    public void StopHand()
    {
        GetComponent<Animator>().SetTrigger("Idle");
        isKillable = false;

        Invoke("ActivateKillable", 1);
        isDestructor = false;
    }
}
