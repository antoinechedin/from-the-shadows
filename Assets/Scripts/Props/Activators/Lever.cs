using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Lever : Activator, IResetable
{
    public AudioClip soundOn;
    public AudioClip soundOff;
    public Material activeMat;
    public Material inactiveMat;
    public bool hasTimer;
    public float timer;
    public bool activeAtStart;


    private bool canBeActivated;
    private int idPlayer = 0;
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
        {
            canBeActivated = true;
            idPlayer = collision.gameObject.GetComponent<PlayerController>().id;
        }
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
        if (canBeActivated && Input.GetButtonDown("X_1") && idPlayer == 1)
        {
            if (!active)
                On(false);
            else
                Off();                
        }
        if (canBeActivated && Input.GetButtonDown("X_2") && idPlayer == 2)
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
        if (TryActivate != null)
        {
            active = true;
            TryActivate();
            if (soundPlayer != null && !isMute)
                soundPlayer.PlaySoundAtLocation(soundOn, 1f);
            if (child != null)
                child.GetComponent<MeshRenderer>().material = activeMat;

            if (hasTimer && !ignoreTimer)
                Invoke("Off", timer);
        }
    }

    /// <summary>
    /// Deactivate the lever
    /// </summary>
    protected void Off()
    {
        if (TryDeactivate != null)
        {
            active = false;
            TryDeactivate();            
            if (soundPlayer != null && !isMute)
                soundPlayer.PlaySoundAtLocation(soundOff, 1f);
            if (child != null)
                child.GetComponent<MeshRenderer>().material = inactiveMat;
        }
    }

    public void Reset()
    {
        isMute = true;
        if (active && !activeAtStart)
            Off();
        else if (!active && activeAtStart)
            On(true);
        isMute = false;
    }
}
