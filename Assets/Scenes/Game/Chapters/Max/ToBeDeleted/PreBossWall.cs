using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreBossWall : MonoBehaviour, IResetable
{
    public bool isHorizontal = true;
    public float speed = 2f;

    private Vector3 basePosition;
    private void Start()
    {
        basePosition = this.transform.position;
    }
    void Update()
    {
        if (isHorizontal)
            this.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        else
            this.transform.position += new Vector3(0, speed * Time.deltaTime, 0);
    }

    public void Reset()
    {
        this.transform.position = basePosition;
    }
}
