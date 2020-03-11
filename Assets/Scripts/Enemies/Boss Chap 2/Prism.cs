using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prism : Receptor
{
    public int nbLaserToActivate;

    private int currentNbLaser;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public new void On()
    {
        Debug.Log("++");
        currentNbLaser++;

        if (currentNbLaser == nbLaserToActivate)
        {
            //faire la fonction qui tire un rayon
        }
    }

    public new void Off()
    {
        Debug.Log("--");
        currentNbLaser--;
    }

}