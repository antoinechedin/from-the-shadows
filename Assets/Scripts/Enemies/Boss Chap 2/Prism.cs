using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prism : Receptor
{
    private int requireNbLaserTouching;
    public override void On(GameObject touchingGo)
    {
        if (!lasersTouching.Contains(touchingGo))
        {
            //ajout dans la liste
            lasersTouching.Add(touchingGo);
            TestNbLaserTouching();
        }
    }

    public override void Off(GameObject leavingGo)
    {
        //enlevage de liste
        if (lasersTouching.Contains(leavingGo))
        {
            lasersTouching.Remove(leavingGo);
            TestNbLaserTouching();
        }
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
        }
    }
}