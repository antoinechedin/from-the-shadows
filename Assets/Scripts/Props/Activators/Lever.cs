using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Lever : Activator
{
    public AudioClip soundOn;
    public AudioClip soundOff;
    public Material activeMat;
    public Material inactiveMat;
    public bool hasTimer;
    public float timer;

    private SoundPlayer soundPlayer;
    private GameObject child;

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

    /// <summary>
    /// Activate the lever
    /// </summary>
    /// <param name="ignoreTimer"> Ignore timer reset (when the activator is active at the beginning of the level</param>
    protected void On(bool ignoreTimer)
    {
        if (Activate != null)
        {
            Activate();
            active = true;
            if (child != null)
            {
                child.GetComponent<MeshRenderer>().material = activeMat;
            }
            if (hasTimer && !ignoreTimer)
            {
                StartCoroutine(DeactivateAfterTimer());
            }
        }
    }

    /// <summary>
    /// Deactivate the lever
    /// </summary>
    protected void Off()
    {
        if (Deactivate != null)
        {
            Deactivate();
            active = false;
            if (child != null)
            {
                child.GetComponent<MeshRenderer>().material = inactiveMat;
            }
        }
    }

    /// <summary>
    /// Deactivate the activator at the end of the timer
    /// </summary>
    /// <returns></returns>
    protected IEnumerator DeactivateAfterTimer()
    {
        yield return new WaitForSeconds(timer);
        Off();
    }
}
