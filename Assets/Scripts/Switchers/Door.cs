///<summary>
/// Change the door state
///</summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    GameObject DoorOpen; // Prefab of the open Door

    [SerializeField]
    GameObject DoorShut; // Prefab of the shut Door

    public bool isOpen = false;

    void Start()
    {
        // Set the Shut sprite to the door
        gameObject.GetComponent<SpriteRenderer>().sprite = DoorShut.GetComponent<SpriteRenderer>().sprite;
    }

    void Update()
    {
        if (isOpen)
            OpenDoor();
    }

    public void OpenDoor()
    {
        // Set the Open sprite to the door
        gameObject.GetComponent<SpriteRenderer>().sprite = DoorOpen.GetComponent<SpriteRenderer>().sprite;
    }
}
