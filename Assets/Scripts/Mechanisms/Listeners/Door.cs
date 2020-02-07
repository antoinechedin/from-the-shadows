using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Door : ActivatorListener
{
    public bool open;

    private void Start()
    {
        GetComponent<BoxCollider2D>().enabled = open;
    }

    public override void OnActivate()
    {
        open = !open;
        GetComponent<BoxCollider2D>().enabled = open;
        GetComponent<MeshRenderer>().enabled = open;
    }

    public override void OnDeactivate()
    {
        open = !open;
        GetComponent<BoxCollider2D>().enabled = open;
        GetComponent<MeshRenderer>().enabled = open;
    }
}
