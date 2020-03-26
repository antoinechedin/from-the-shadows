using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receptor : Activator, IResetable
{
    public Material inactiveMat;
    public Material activeMat;

    private GameObject child;
    protected List<GameObject> lasersTouching = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (transform.Find("Child") != null)
        {
            child = transform.Find("Child").gameObject;
        }

        if (child != null)
        {
            child.GetComponent<MeshRenderer>().material = inactiveMat;
        }
    }

    public virtual void On(GameObject touchingGo)
    {
        if (!lasersTouching.Contains(touchingGo))
        {
            //ajout dans la liste
            AddLaser(touchingGo);

            if (TryActivate != null && !active && lasersTouching.Count > 0)
            {
                active = true;
                TryActivate();
                if (child != null)
                {
                    child.GetComponent<MeshRenderer>().material = activeMat;
                }
            }
        }
    }

    public virtual void Off(GameObject leavingGo)
    {
        if (lasersTouching.Contains(leavingGo))
        {
            //enlevage de liste
            RemoveLaser(leavingGo);

            if (lasersTouching.Count == 0)
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
        }
    }

    public virtual void AddLaser(GameObject addedlaser)
    {
        lasersTouching.Add(addedlaser);
    }

    public virtual void RemoveLaser(GameObject removedLaser)
    {
        lasersTouching.Remove(removedLaser);
    }

    public virtual void Reset()
    {
        Off(gameObject); 
    }
}
