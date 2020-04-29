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
    [HideInInspector]
    public GameObject particle;

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
            PlayParticles(particle);
            //if (!particle.active)
             //   particle.SetActive(true);   
            
            // When double attack, activate the other direction lane
            //if(skeleton.GetComponent<Skeleton>().hp == 1 && otherDirection != null && !otherDirection.GetComponent<TargetZone>().particle.active)
            if(skeleton.GetComponent<Skeleton>().hp == 1 && otherDirection != null)
            {
                // otherDirection.GetComponent<TargetZone>().particle.SetActive(true);
                PlayParticles(otherDirection.GetComponent<TargetZone>().particle);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (skeleton != null && collision.gameObject == skeleton.GetComponent<Skeleton>().playerTarget
            && skeleton.GetComponent<Skeleton>().isTargetting)
        {
            StopParticles(particle);

            // Deactivate other lane when double attack
            /*if (otherDirection != null && otherDirection.GetComponent<TargetZone>().particle.active)
            {
                otherDirection.GetComponent<TargetZone>().particle.SetActive(false);
            } */
            if (otherDirection != null)
                StopParticles(otherDirection.GetComponent<TargetZone>().particle);
        }
    }

    public void StopParticles(GameObject parent)
    {
        ParticleSystem[] pss = parent.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in pss)
        {
            if (ps.isPlaying)
            {
                Debug.Log("STOP");
                ps.Stop();
            }
                
        }
    }

    void PlayParticles(GameObject parent)
    {
        ParticleSystem[] pss = parent.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in pss)
        {
            if (ps.isStopped)
            {
                ps.Play();
                Debug.Log("PLAY");
            }                
        }
    }
}
