using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float speed = 2f;
    public int directionX = 1;

    void Update()
    {
        if (directionX == 1)
            this.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        else
            this.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
    }
}
