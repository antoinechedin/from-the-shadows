using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : ActivatorListener
{
    private void Start()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public override void OnActivate()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    public override void OnDeactivate()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}