using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receptor : Activator, IResetable
{
    public Material inactiveMat;
    public Material activeMat;

    private GameObject child;
    private List<GameObject> currentLasers;

    // Start is called before the first frame update
    void Start()
    {
        child = transform.Find("Child").gameObject;
        if (child != null)
        {
            child.GetComponent<MeshRenderer>().material = inactiveMat;
        }
    }

    public virtual void On()
    {

        //if pas dans liste
            //ajout dans liste
            if (TryActivate != null && !active) //&& if Count > 0
            {
                active = true;
                TryActivate();
                if (child != null)
                {
                    child.GetComponent<MeshRenderer>().material = activeMat;
                }
            }
    }

    public virtual void Off()
    {
        //enlevage de liste

        //if list.COunt == 0
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
