using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    private bool move = false;


    public float speed = 2f;
    public Transform target;

    public void StartMoving()
    {
        move = true;
    }

    public void StopMoving()
    {
        move = false;
    }


    void Update()
    {
        if(move)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
