using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesSound : MonoBehaviour
{
    public List<AudioClip> sounds;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (audioSource != null && sounds.Count > 0)
            {
                audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Count - 1)]);
            }
        }
    }
}
