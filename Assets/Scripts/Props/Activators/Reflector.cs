using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : MonoBehaviour, IResetable
{
    private bool canPlayer1Activate;
    private bool canPlayer2Activate;
    private Quaternion startRotation;
    private Quaternion currentRotation;

    public float angleRotation = 22.5f;
    public float speedRotation = 5;

    public void Start()
    {
        startRotation = transform.parent.rotation;
        currentRotation = startRotation;
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
            Rotate(angleRotation);
        }
        if (canPlayer2Activate && Input.GetButtonDown("X_2"))
        {
            Rotate(angleRotation);   
        }
        if (canPlayer1Activate && Input.GetButtonDown("Y_1"))
        {
            Rotate(-angleRotation);
        }
        if (canPlayer2Activate && Input.GetButtonDown("Y_2"))
        {
            Rotate(-angleRotation); 
        }

        transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, currentRotation, Time.deltaTime * speedRotation);
    }

    public void Rotate(float angle){
        Quaternion rot = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        currentRotation = currentRotation * rot;
    }

    public void Reset()
    {
        transform.parent.rotation = startRotation;
        currentRotation = startRotation;
    }

    public void Shuffle()
    {
        int max = Mathf.FloorToInt(360 / angleRotation);
        Rotate(Random.Range(1, max) * angleRotation);
    }
}
