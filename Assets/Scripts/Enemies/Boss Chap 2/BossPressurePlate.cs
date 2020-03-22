using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BossPressurePlate : Activator
{
    public AudioClip sound;
    public Material activeMat;
    public Material inactiveMat;
    public PrismRotator prismRotator;
    [Header("-1 = gauche. 1 = droite")]
    [Range(-1, 1)]
    public int sens;
    

    private GameObject child;
    private AudioSource audioSource;
    private int nbObjectsOnPlate;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        child = transform.Find("Child").gameObject;
        Off();
    }

    /// <summary>
    /// Activate when an Object is on the Plate or a Player
    /// </summary>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            updateNbObjectsOnPlate(+1);
        }
    }

    /// <summary>
    /// Deactivate when the Object or the Player leaves the Plate
    /// </summary>
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            updateNbObjectsOnPlate(-1);
        }
    }

    /// <summary>
    /// Activate the pressure plate
    /// </summary>
    protected void On()
    {
        prismRotator.Rotate(sens);
        if (audioSource != null)
            audioSource.PlayOneShot(sound);
        if (child != null)
            child.GetComponent<MeshRenderer>().material = activeMat;
    }

    /// <summary>
    /// Deactivate the pressure plate 
    /// </summary>
    protected void Off()
    {
        if (child != null)
            child.GetComponent<MeshRenderer>().material = inactiveMat;
    }

    void updateNbObjectsOnPlate(int i)
    {
        nbObjectsOnPlate += i;

        if (nbObjectsOnPlate == 1)
            On();
        if (nbObjectsOnPlate == 0)
        {
            Off();
        }
    }
}
