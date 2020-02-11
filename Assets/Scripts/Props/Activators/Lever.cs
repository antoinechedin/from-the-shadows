using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Lever : Activator
{
    public AudioClip soundOn;
    public AudioClip soundOff;
    private SoundPlayer soundPlayer;

    private void Start()
    {
        soundPlayer = GetComponent<SoundPlayer>();
        child = transform.Find("Child").gameObject;
        if (!active)
            Off();
        else if (active)
            On(true);
    }

    /// <summary>
    /// Activate or deactivate the lever when a player interracts 
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetButtonDown("X_G"))
        {
            if (!active)
            {
                On(false);
                if (soundPlayer != null)
                    soundPlayer.PlaySoundAtLocation(soundOn, 1f);
            }
            else
            {
                Off();
                if (soundPlayer != null)
                    soundPlayer.PlaySoundAtLocation(soundOff, 1f);
            }                
        }
    }
}
