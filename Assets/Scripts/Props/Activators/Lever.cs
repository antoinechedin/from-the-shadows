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
    public Material timerMat;
    public bool hasTimer;
    public float timer;
    public bool activeAtStart;
    public bool doNotReset = false;

    private bool canPlayer1Activate = false;
    private bool canPlayer2Activate = false;
    private AudioSource audioSource;
    private GameObject child;
    private bool isMute = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        child = transform.Find("Child").gameObject;
        active = activeAtStart;
        if (activeAtStart)
        {
            On(true);
            if (GetComponentInChildren<Animator>() != null)
            {
                GetComponentInChildren<Animator>().SetBool("OFF", false);
            }
        }
        else
        {
            Off();
            if (GetComponentInChildren<Animator>())
            {
                GetComponentInChildren<Animator>().SetBool("OFF", true);
            }
        }

        isMute = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int idPlayer = collision.gameObject.GetComponent<PlayerInput>().id;
            if (idPlayer == 1)
                canPlayer1Activate = true;
            else if (idPlayer == 2)
                canPlayer2Activate = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int idPlayer = collision.gameObject.GetComponent<PlayerInput>().id;
            if (idPlayer == 1)
                canPlayer1Activate = false;
            else if (idPlayer == 2)
                canPlayer2Activate = false;
        }
    }

    /// <summary>
    /// Activate or deactivate the lever when a player interracts if he is in the collider
    /// </summary>
    public void Update()
    {
        if (canPlayer1Activate && Input.GetButtonDown("X_1"))
        {
            if (!active)
                On(false);
            else
                Off();                
        }
        if (canPlayer2Activate && Input.GetButtonDown("X_2"))
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
            if (GetComponentInChildren<Animator>())
            {
                GetComponentInChildren<Animator>().SetBool("OFF", false);
            }

            active = true;
            TryActivate();
            if (audioSource != null && !isMute)
                audioSource.PlayOneShot(soundOn);
            if (child != null)
                child.GetComponent<MeshRenderer>().material = activeMat;

            CancelInvoke();
            StopCoroutine("Flash");
            if (hasTimer && !ignoreTimer)
            {
                Invoke("Off", timer);
                StartCoroutine("Flash");
            }
        }
    }

    /// <summary>
    /// Deactivate the lever
    /// </summary>
    protected void Off()
    {
        if (TryDeactivate != null)
        {
            if (GetComponentInChildren<Animator>())
            {
                GetComponentInChildren<Animator>().SetBool("OFF", true);
            }

            active = false;
            StopCoroutine("Flash");
            TryDeactivate();            
            if (audioSource != null && !isMute)
                audioSource.PlayOneShot(soundOff);
            if (child != null)
            {
                child.GetComponent<MeshRenderer>().material = inactiveMat;
            }
                
        }
    }

    public void Reset()
    {
        if (!doNotReset)
        {
            isMute = true;
            if (active && !activeAtStart)
                Off();
            else if (!active && activeAtStart)
                On(true);
            isMute = false;

            canPlayer1Activate = false;
            canPlayer2Activate = false;
        }
    }

    private IEnumerator Flash()
    {
        if (child != null)
        {
            while (true)
            {
                child.GetComponent<MeshRenderer>().material = activeMat;
                yield return new WaitForSeconds(0.2f);
                child.GetComponent<MeshRenderer>().material = timerMat;
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
