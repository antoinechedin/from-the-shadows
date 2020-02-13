using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    private Vector3 limitLB;
    private Vector3 limitRT;
    private Vector3 posToMoveTo;
    private bool moving = false;
    private bool smooth = true;
    private bool inLimits = true;

    // Update is called once per frame
    void Update()
    {
        if (moving == true) //si on a dit à la caméra de bouger (aka moving est vrai) alors on bouge
        {
            if (inLimits)
            {
                if (posToMoveTo.x < limitLB.x) posToMoveTo.x = limitLB.x;
                if (posToMoveTo.x > limitRT.x) posToMoveTo.x = limitRT.x;
                if (posToMoveTo.y < limitLB.y) posToMoveTo.y = limitLB.y;
                if (posToMoveTo.y > limitRT.y) posToMoveTo.y = limitRT.y;
            }

            //déplacement de la caméra
            Vector3 velocity = Vector3.zero;
            if (smooth)
                transform.position = Vector3.SmoothDamp(transform.position, posToMoveTo, ref velocity, 0.06f);
            else
                transform.position = posToMoveTo;

            if (Vector3.Distance(transform.position, posToMoveTo) < 0.01f) //Si la caméra est arrivée à la positon
            {
                moving = false;
            }
            smooth = false;
        }

        //sinon on ne fait rien
    }

    public void SetLimit(Transform LB, Transform RT)
    {
        limitLB = LB.position;
        limitRT = RT.position;
    }

    public void MoveTo(Vector3 pos, bool isSmooth = true, bool stayInLimites = true)
    {
        posToMoveTo = pos;
        moving = true;
        smooth = isSmooth;
        inLimits = stayInLimites;
    }

}
