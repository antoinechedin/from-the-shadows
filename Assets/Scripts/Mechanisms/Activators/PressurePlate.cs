using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Activator
{
    private void Start()
    {
        if (Deactivate != null)
            Deactivate();
    }

    /// <summary>
    /// Activate when an Object is on the Plate or a Player
    /// </summary>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Object"))
            if (Activate != null)
                Activate();
    }

    /// <summary>
    /// Deactivate when the Object or the Player leaves the Plate
    /// </summary>
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Object"))
            if (Deactivate != null)
                Deactivate();
    }
}
