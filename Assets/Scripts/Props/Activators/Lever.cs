using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Lever : Activator
{
    private void Start()
    {
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
                On(false);
            else
                Off();
        }
    }
}
