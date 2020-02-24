using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    public float maxOffsetY = 0;
    public float distanceMaxOffset = 10;
    public float distanceMinOffset = 4;
    private GameObject[] players = new GameObject[2];

    public float offsetY;

    private Vector3 limitLB;
    private Vector3 limitRT;
    private Vector3 posToMoveTo;
    private bool moving = false;
    private bool smooth = true;
    private bool inLimits = true;

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distancePlayerY = Mathf.Abs(players[0].transform.position.y - players[1].transform.position.y); // Distance entre les joueurs

        offsetY = Mathf.Abs(distancePlayerY - distanceMaxOffset) / distanceMaxOffset * maxOffsetY;


        if (moving == true) //si on a dit à la caméra de bouger (aka moving est vrai) alors on bouge
        {
            posToMoveTo.y += offsetY;

            if (inLimits)
            {
                if (posToMoveTo.x < limitLB.x) posToMoveTo.x = limitLB.x;
                if (posToMoveTo.x > limitRT.x) posToMoveTo.x = limitRT.x;
                if (posToMoveTo.y < limitLB.y) posToMoveTo.y = limitLB.y;
                if (posToMoveTo.y > limitRT.y) posToMoveTo.y = limitRT.y;
                if (posToMoveTo.z < limitLB.z) posToMoveTo.z = limitLB.z;
                if (posToMoveTo.z > limitRT.z) posToMoveTo.z = limitRT.z;
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

    public void MoveTo(Vector3 pos, bool isSmooth = true)
    {
        posToMoveTo = pos;
        moving = true;
        smooth = isSmooth;
    }

    public bool StayInLimits
    {
        get { return inLimits; }
        set { inLimits = value; }
    }

}
