using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prism : Receptor
{
    public int requireNbLaserTouching;

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
        Debug.Log(gameObject.name + " : " + lasersTouching.Count);
        if (lasersTouching.Count >= requireNbLaserTouching)
        {
            //FIRE A BIG LASER
            Debug.Log("FIRE A BIG LASER");
        }
    }
}