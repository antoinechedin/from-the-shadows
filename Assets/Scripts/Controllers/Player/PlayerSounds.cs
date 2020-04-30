using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSounds : MonoBehaviour
{
    public AudioClip[] footsteps;
    public AudioClip[] accroches;
    public AudioClip[] atterrissages;
    public AudioClip[] sauts;
    public AudioClip[] doubleSauts;
    public AudioClip[] mort;

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

    public void PlayRandomFAccroche()
    {
        if (accroches.Length > 0)
            audioSource.PlayOneShot(accroches[Random.Range(0, accroches.Length)]);
    }

    public void PlayRandomAtterrissage()
    {
        if (atterrissages.Length > 0)
            audioSource.PlayOneShot(atterrissages[Random.Range(0, atterrissages.Length)]);
    }

    public void PlayRandomJump()
    {
        if (sauts.Length > 0)
            audioSource.PlayOneShot(sauts[Random.Range(0, sauts.Length)]);
    }

    public void PlayRandomDoubleJump()
    {
        if (doubleSauts.Length > 0)
            audioSource.PlayOneShot(doubleSauts[Random.Range(0, doubleSauts.Length)]);
    }

    public void PlayRandomMort()
    {
        if (mort.Length > 0)
            audioSource.PlayOneShot(mort[Random.Range(0, mort.Length)]);
    }
}
