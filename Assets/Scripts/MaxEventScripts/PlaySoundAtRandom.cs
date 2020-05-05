using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundAtRandom : MonoBehaviour
{
    public float min = 0f;
    public float max = 0f;

    public List<AudioClip> randomSounds;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Invoke("StartCoroutineDelayed", 5f);
    }

    public void StartCoroutineDelayed()
    {
        StartCoroutine(PlayRandomSound());
    }

    IEnumerator PlayRandomSound()
    {
        AudioClip randomlyPlayedSound = randomSounds[Random.Range(0, randomSounds.Count)];
        audioSource.PlayOneShot(randomlyPlayedSound);

        float randomTimeToWait = Random.Range(min, max);

        yield return new WaitForSeconds(randomlyPlayedSound.length + randomTimeToWait);

        StartCoroutine(PlayRandomSound());
    }
}
