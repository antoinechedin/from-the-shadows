using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : ActivatorListener
{

    public GameObject lightSource;
    public AudioClip soundOn;
    public AudioClip soundOff;

    private SoundPlayer soundPlayer;

    void Start()
    {
        soundPlayer = GetComponent<SoundPlayer>();
    }

    public override void OnActivate()
    {
        lightSource.SetActive(true);
        if (soundPlayer != null)
            soundPlayer.PlaySoundAtLocation(soundOn, 1);
    }

    public override void OnDeactivate()
    {
        lightSource.SetActive(false);
        if (soundPlayer != null)
            soundPlayer.PlaySoundAtLocation(soundOff, 1);
    }
}
