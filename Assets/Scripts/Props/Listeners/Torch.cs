using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : ActivatorListener, IResetable
{

    public GameObject lightSource;
    public AudioClip soundOn;
    public AudioClip soundOff;
    public bool activeAtStart;
    public float lightRadius;

    private bool isMute = true;
    public bool active;
    private SoundPlayer soundPlayer;
    private float targetRadius = 0.01f;

    private void Awake()
    {
        lightSource = transform.Find("LightSource").gameObject;
    }

    void Start()
    {
        soundPlayer = GetComponent<SoundPlayer>();
        if (activeAtStart)
        {
            OnActivate();
        }
        isMute = false;
        active = activeAtStart;
        lightSource.GetComponent<NewLightSource>().lightRadius = 0;
    }

    private void Update()
    {
        lightSource.GetComponent<NewLightSource>().lightRadius = Mathf.Lerp(lightSource.GetComponent<NewLightSource>().lightRadius, targetRadius, Time.deltaTime * 10);
    }

    public override void OnActivate()
    {
        targetRadius = lightRadius;
        active = true;
        if (soundPlayer != null && !isMute)
            soundPlayer.PlaySoundAtLocation(soundOn, 1);
    }

    public override void OnDeactivate()
    {
        targetRadius = 0.01f;
        active = false;
        if (soundPlayer != null && !isMute)
            soundPlayer.PlaySoundAtLocation(soundOff, 1);
    }

    public void Reset()
    {
        isMute = true;
        if (active && !activeAtStart)
            OnDeactivate();
        else if (!active && activeAtStart)
            OnActivate();
        isMute = false;
    }
}
