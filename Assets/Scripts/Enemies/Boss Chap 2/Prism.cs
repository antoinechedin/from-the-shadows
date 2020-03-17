using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prism : Receptor
{
    public int requireNbLaserTouching;
    public Material notActivated;
    public Material activated;
    public List<GameObject> indicators;

    public override void AddLaser(GameObject addedlaser) 
    {
        base.AddLaser(addedlaser);
        TestNbLaserTouching();
    }

    public override void RemoveLaser(GameObject removedLaser)
    {
        base.RemoveLaser(removedLaser);
        TestNbLaserTouching();
    }

    /// <summary>
    /// Test to see if the number of lasers touching the prism is equal to the required number. If it's true, fire a big laser.
    /// </summary>
    public void TestNbLaserTouching()
    {
        //reset de la couleur des faces
        Material[] activatedArray = new Material[] { activated, activated, activated, activated, activated, activated };
        Material[] notActivatedArray = new Material[] { notActivated, notActivated, notActivated, notActivated, notActivated, notActivated };
        foreach (GameObject gameObject in indicators)
        {
            gameObject.GetComponent<MeshRenderer>().materials = notActivatedArray;
        }

        for (int i = 0; i < lasersTouching.Count; i++)
        {
            indicators[i].GetComponent<MeshRenderer>().materials = activatedArray;
        }


        Debug.Log(gameObject.name + " : " + lasersTouching.Count);
        if (lasersTouching.Count >= requireNbLaserTouching)
        {
            //FIRE A BIG LASER
            Debug.Log("FIRE A BIG LASER");
        }
    }
}