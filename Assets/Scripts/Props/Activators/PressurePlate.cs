using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PressurePlate : Activator
{
    public AudioClip sound;
    public Material activeMat;
    public Material inactiveMat;
    public bool hasTimer;
    public float timer;

    private GameObject child;
    private SoundPlayer soundPlayer;

    private void Start()
    {
        soundPlayer = GetComponent<SoundPlayer>();       
        child = transform.Find("Child").gameObject;
        Off();
    }

    /// <summary>
    /// Activate when an Object is on the Plate or a Player
    /// </summary>
    public void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Object"))
        {
            On();
            if (soundPlayer != null)
                soundPlayer.PlaySoundAtLocation(sound, 1f);
        }            
    }

    /// <summary>
    /// Deactivate when the Object or the Player leaves the Plate
    /// </summary>
    public void OnTriggerExit2D(Collider2D collision)
    {        
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Object"))
        {
            if (hasTimer)
                Invoke("Off", timer);
            else
                Off();
        }
    }

    /// <summary>
    /// Activate the pressure plate
    /// </summary>
    protected void On()
    {
        if (Activate != null)
        {
            Activate();
            if (child != null)
                child.GetComponent<MeshRenderer>().material = activeMat;
        }
    }

    /// <summary>
    /// Deactivate the pressure plate 
    /// </summary>
    protected void Off()
    {
        if (Deactivate != null)
        {
            Deactivate();
            if (child != null)            
                child.GetComponent<MeshRenderer>().material = inactiveMat;            
        }
    }
}
