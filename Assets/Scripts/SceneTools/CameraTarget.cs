using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private Vector2 offset;
    private GameObject audioListener;
    private GameObject[] players = new GameObject[2];

    public bool isSolo = false;

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        if (isSolo)
            transform.position = players[0].transform.position + (Vector3)offset;
        else
            transform.position = (players[0].transform.position + players[1].transform.position) / 2 + (Vector3)offset;

        audioListener = GetComponentInChildren<AudioListener>().gameObject;
    }

    void FixedUpdate()
    {
        if (isSolo)
            transform.position = players[0].transform.position + (Vector3)offset;
        else
            transform.position = (players[0].transform.position + players[1].transform.position) / 2 + (Vector3)offset;
    }

    public Vector2 Offset
    {
        get => offset;
        set
        {
            offset = value;
            if (audioListener != null) audioListener.transform.localPosition = -value;
        }
    }
}
