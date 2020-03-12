using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCursor : MonoBehaviour
{
    public List<Transform> positions;
    public float speed;

    private Vector3 targetPos;

    private void Start()
    {
        SetTargetPos(1);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);
    }

    public void SetTargetPos(int index)
    {
        targetPos = positions[index].position;
    }
}
