using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireSound : MonoBehaviour
{
    public AudioClip[] footsteps;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomFootstep()
    {
        if (footsteps.Length > 0)
            audioSource.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)]);
    }
}

