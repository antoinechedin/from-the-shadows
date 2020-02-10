using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : ActivatorListener
{
    private bool active;
    public GameObject lightSource;
    public AudioClip soundOn;
    public AudioClip soundOff;
    private SoundPlayer soundPlayer;

    void Awake()
    {
        soundPlayer = GetComponent<SoundPlayer>();
    }

    public override void OnActivate()
    {
        active = true;
        lightSource.SetActive(active);
        if (soundPlayer != null)
            soundPlayer.PlaySoundAtLocation(soundOn, 1);
    }

    public override void OnDeactivate()
    {
        active = false;
        lightSource.SetActive(active);
        if (soundPlayer != null)
            soundPlayer.PlaySoundAtLocation(soundOff, 1);
    }
}
