using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : ActivatorListener
{

    public GameObject lightSource;
    public AudioClip soundOn;
    public AudioClip soundOff;
    public bool activeAtStart;

    private SoundPlayer soundPlayer;
    private Vector3 targetScale = Vector3.zero;

    void Start()
    {
        soundPlayer = GetComponent<SoundPlayer>();
        if (activeAtStart)
        {
            OnActivate();
        }
    }

    private void Update()
    {
        lightSource.transform.localScale = Vector3.Lerp(lightSource.transform.localScale, targetScale, Time.deltaTime * 10);
    }

    public override void OnActivate()
    {
        targetScale = Vector3.one;
        if (soundPlayer != null)
            soundPlayer.PlaySoundAtLocation(soundOn, 1);
    }

    public override void OnDeactivate()
    {
        targetScale = Vector3.zero;
        if (soundPlayer != null)
            soundPlayer.PlaySoundAtLocation(soundOff, 1);
    }
}
