using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private Vector2 offset;
    private GameObject[] players = new GameObject[2];

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        transform.position = (players[0].transform.position + players[1].transform.position) / 2 + (Vector3)offset;
    }

    void FixedUpdate()
    {
        transform.position = (players[0].transform.position + players[1].transform.position) / 2 + (Vector3)offset;
    }

    public Vector2 Offset
    {
        get => offset;
        set => offset = value;
    }
}
