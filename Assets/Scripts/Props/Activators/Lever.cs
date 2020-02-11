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
    public bool activeAtStart;

    private bool active;
    private bool canBeActivated;
    private SoundPlayer soundPlayer;
    private GameObject child;
    private bool isMute = true;

    private void Start()
    {
        soundPlayer = GetComponent<SoundPlayer>();
        child = transform.Find("Child").gameObject;
        active = activeAtStart;
        if (activeAtStart)
            On(true);
        else
            Off();

        isMute = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            canBeActivated = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            canBeActivated = false;
    }

    /// <summary>
    /// Activate or deactivate the lever when a player interracts if he is in the collider
    /// </summary>
    public void Update()
    {
        if (canBeActivated && Input.GetButtonDown("X_G"))
        {
            if (!active)
                On(false);
            else
                Off();                
        }
    }

    /// <summary>
    /// Activate the lever
    /// </summary>
    /// <param name="ignoreTimer"> Ignore timer reset (when the lever is active at the beginning of the level</param>
    protected void On(bool ignoreTimer)
    {
        if (Activate != null)
        {
            Activate();
            active = true;
            if (soundPlayer != null && !isMute)
                soundPlayer.PlaySoundAtLocation(soundOn, 1f);
            if (child != null)
                child.GetComponent<MeshRenderer>().material = activeMat;

            if (hasTimer && !ignoreTimer)
                StartCoroutine(DeactivateAfterTimer());

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
            if (soundPlayer != null && !isMute)
                soundPlayer.PlaySoundAtLocation(soundOff, 1f);
            if (child != null)
                child.GetComponent<MeshRenderer>().material = inactiveMat;
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

    private void Reset()
    {
        isMute = true;
        if (active && !activeAtStart)
            Off();
        else if (!active && activeAtStart)
            On(true);
        isMute = false;
    }
}
