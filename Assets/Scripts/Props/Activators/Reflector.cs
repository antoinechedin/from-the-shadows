using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : MonoBehaviour, IResetable
{
    private bool canPlayer1Activate;
    private bool canPlayer2Activate;
    private Quaternion startRotation;

    public void Start()
    {
        startRotation = transform.parent.rotation;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int idPlayer = collision.gameObject.GetComponent<PlayerInput>().id;
            if (idPlayer == 1)
                canPlayer1Activate = true;
            else if (idPlayer == 2)
                canPlayer2Activate = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int idPlayer = collision.gameObject.GetComponent<PlayerInput>().id;
            if (idPlayer == 1)
                canPlayer1Activate = false;
            else if (idPlayer == 2)
                canPlayer2Activate = false;
        }
    }

    public void Update()
    {
        if (canPlayer1Activate && Input.GetButtonDown("X_1"))
        {
            Rotate(22.5f);
        }
        if (canPlayer2Activate && Input.GetButtonDown("X_2"))
        {
            Rotate(22.5f);   
        }
        if (canPlayer1Activate && Input.GetButtonDown("Y_1"))
        {
            Rotate(-22.5f);
        }
        if (canPlayer2Activate && Input.GetButtonDown("Y_2"))
        {
            Rotate(-22.5f); 
        }
    }

    public void Rotate(float angle){
        transform.parent.Rotate(new Vector3 (0, 0, angle));
    }

    public void Reset()
    {
        transform.parent.rotation = startRotation;
    }
}
