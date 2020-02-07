using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : ActivatorListener
{
    private bool active;
    public GameObject lightSource;

    // Start is called before the first frame update
    void Start()
    {
        //lightSource = transform.Find("LightSource").gameObject;
    }

    public override void OnActivate()
    {
        active = true;
        lightSource.SetActive(active);
    }

    public override void OnDeactivate()
    {
        active = false;
        lightSource.SetActive(active);
    }
}
