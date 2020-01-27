using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays a scaled AudioClip on the attached AudioSource 
    /// </summary>
    /// <param name="audioClip"> Sound to play</param>
    /// <param name="volumeScale"> Volume scale (0 - 1) compared to the AudioSource </param>
    public void PlaySound(AudioClip audioClip, float volumeScale)
    {
        audioSource.PlayOneShot(audioClip, volumeScale);
    }

    /// <summary>
    /// Plays an AudioClip, eventually looping
    /// </summary>
    /// <param name="audioClip"> Clip to play </param>
    /// <param name="looping"> True if the clip is looping </param>
    public void PlayMusic(AudioClip audioClip, bool looping)
    {
        audioSource.loop = looping;
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    /// <summary>
    /// Plays an AudioClip at the position of the SoundPlayer in world space
    /// </summary>
    /// <param name="audioClip"> Clip to play </param>
    /// <param name="volume"> Play volume </param>
    public void PlaySoundAtLocation(AudioClip audioClip, float volume)
    {
        AudioSource.PlayClipAtPoint(audioClip, gameObject.transform.position, volume);
    }
}
