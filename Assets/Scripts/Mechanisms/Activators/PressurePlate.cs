using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PressurePlate : Activator
{
    GameObject plate;
    public Material activeMat;
    public Material inactiveMat;

    private void Start()
    {
        plate = transform.Find("Cube").gameObject;
        plate.GetComponent<MeshRenderer>().material = inactiveMat;
        if (Deactivate != null)
            Deactivate();
    }

    /// <summary>
    /// Activate when an Object is on the Plate or a Player
    /// </summary>
    public void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Object"))
        {
            plate.GetComponent<MeshRenderer>().material = activeMat;
            if (Activate != null)
                Activate();
        }            
    }

    /// <summary>
    /// Deactivate when the Object or the Player leaves the Plate
    /// </summary>
    public void OnTriggerExit2D(Collider2D collision)
    {        
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Object"))
        {
            plate.GetComponent<MeshRenderer>().material = inactiveMat;
            if (Deactivate != null)
                Deactivate();
        }
    }
}
