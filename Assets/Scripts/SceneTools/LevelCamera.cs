using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    private Vector3 posToMoveTo;
    private bool moving = false;

    // Update is called once per frame
    void Update()
    {
        if (moving == true) //si on a dit à la caméra de bouger (aka moving est vrai) alors on bouge
        {
            //déplacement de la caméra
            Vector3 targetPosition = posToMoveTo;
            Vector3 velocity = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.06f);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f) //Si la caméra est arrivée à la positon
            {
                moving = false;
            }
        }

        //sinon on ne fait rien
    }

    public void MoveTo(Vector3 pos)
    {
        posToMoveTo = pos;
        moving = true;
    }
}
