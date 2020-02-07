using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Lever : Activator
{
    public bool active;
    GameObject child;
    public Material activeMat;
    public Material inactiveMat;

    private void Start()
    {
        child = transform.Find("Cube").gameObject;
        if (!active && Deactivate != null)
        {
            Deactivate();
            child.GetComponent<MeshRenderer>().material = inactiveMat;
        }
        else if (active && Activate != null)
        {
            Activate();
            child.GetComponent<MeshRenderer>().material = activeMat;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetButtonDown("X_G"))
        {
            if (!active)
            {
                if (Activate != null) Activate();
                active = true;
                child.GetComponent<MeshRenderer>().material = activeMat;
            }
            else
            {
                if (Deactivate != null) Deactivate();
                child.GetComponent<MeshRenderer>().material = inactiveMat;
                active = false;
            }
        }
    }
}
