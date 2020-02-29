using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receptor : Activator, IResetable
{
    public Material inactiveMat;
    public Material activeMat;

    private GameObject child;

    // Start is called before the first frame update
    void Start()
    {
        child = transform.Find("Child").gameObject;
        if (child != null)
        {
            child.GetComponent<MeshRenderer>().material = inactiveMat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("allo");
    }

    public void On()
    {
        if (TryActivate != null && !active)
        {
            active = true;
            TryActivate();
            if (child != null)
            {
                child.GetComponent<MeshRenderer>().material = activeMat;
            }
        }
    }

    public void Off()
    {
        if (TryDeactivate != null && active)
        {
            active = false;
            TryDeactivate();
            if (child != null)
            {
                child.GetComponent<MeshRenderer>().material = inactiveMat;
            }
        }
    }

    public void Reset()
    {
        Off();
    }
}
