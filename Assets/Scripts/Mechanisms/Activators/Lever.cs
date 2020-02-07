using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Lever : Activator
{
    bool active;
    private void Start()
    {
        if (Deactivate != null)
            Deactivate();

        active = false;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetButtonDown("X_G"))
        {
            if (!active)
            {
                if (Activate != null) Activate();
                active = true;
            }
            else
            {
                if (Deactivate != null) Deactivate();
                active = false;
            }
        }
    }
}
