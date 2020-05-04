using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PressurePlate : Activator
{
    public List<AudioClip> soundsOn;
    public List<AudioClip> soundsOff;
    public string tagInteractObject;

    private GameObject child;
    private AudioSource audioSource;
    private int nbObjectsOnPlate;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();       
        Off();
    }

    /// <summary>
    /// Activate when an Object is on the Plate or a Player
    /// </summary>
    public void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag(tagInteractObject))
        {
            UpdateNbObjectsOnPlate(+1);
        }            
    }

    /// <summary>
    /// Deactivate when the Object or the Player leaves the Plate
    /// </summary>
    public void OnTriggerExit2D(Collider2D collision)
    {        
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag(tagInteractObject))
        {
            UpdateNbObjectsOnPlate(-1);
        }
    }

    /// <summary>
    /// Activate the pressure plate
    /// </summary>
    protected void On()
    {
        if (TryActivate != null)
        {
            active = true;
            TryActivate();            
            if (audioSource != null  && soundsOn.Count > 0)
                audioSource.PlayOneShot(soundsOn[Random.Range(0, soundsOn.Count-1)]);

            GetComponent<Animator>().SetTrigger("On");

        }
    }

    /// <summary>
    /// Deactivate the pressure plate 
    /// </summary>
    protected void Off()
    {
        if (TryDeactivate != null)
        {
            active = false;
            TryDeactivate();
            if (audioSource != null  && soundsOff.Count > 0)
                audioSource.PlayOneShot(soundsOff[Random.Range(0, soundsOff.Count-1)]);

            GetComponent<Animator>().SetTrigger("Off");
        }
    }

    void UpdateNbObjectsOnPlate(int i)
    {
        nbObjectsOnPlate += i;

        if(i > 0 && nbObjectsOnPlate == 1)
        {
            On();
        }
        if(nbObjectsOnPlate == 0)
        {
            Off();
        }
    }    
}
