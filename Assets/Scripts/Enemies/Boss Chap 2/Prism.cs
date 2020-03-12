using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prism : Receptor
{
    public override void On(GameObject touchingGo) {
        base.On(touchingGo);
        Debug.Log(lasersTouching.Count);
    }

    public override void Off(GameObject touchingGo)
    {
        base.Off(touchingGo);
        Debug.Log(lasersTouching.Count);
    }
}