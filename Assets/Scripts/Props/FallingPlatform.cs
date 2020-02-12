using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour, IResetable
{
    public float timerBeforeFalling;
    public float timerBeforeSpawning;
    public AudioClip fallSound;

    private float shakeIntensity = 0.02f;
    private float shakeSpeed = 50;
    private Vector3 startingPosition;
    private bool isShaking = false;
    private bool isFalling;
    private GameObject child;
    private Vector3 fallingPosition;
    private Color targetColor;
    private Color startingColor;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        child = transform.Find("Child").gameObject;
        startingColor = child.GetComponent<MeshRenderer>().material.color;
        targetColor = startingColor;
        fallingPosition = transform.position;
    }

    public void Update()
    {
        if (isShaking)
        {
            transform.position = new Vector3(transform.position.x + Mathf.Sin(Time.time * shakeSpeed) * shakeIntensity,
                                         transform.position.y,
                                         transform.position.z);
        }

        Color color = child.GetComponent<MeshRenderer>().material.color;
        child.GetComponent<MeshRenderer>().material.color = Color.Lerp(color, targetColor, Time.deltaTime * 10);
        transform.position = Vector3.Lerp(transform.position, fallingPosition, Time.deltaTime * 5);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isShaking = true;
            Invoke("Fall", timerBeforeFalling);
        }
    }

    public void Fall()
    {
        if (child != null)
            child.GetComponent<Collider2D>().enabled = false;
        targetColor = new Color(startingColor.r, startingColor.g, startingColor.b, 0);
        fallingPosition = transform.position - new Vector3(0, 5, 0);
        Invoke("Reset", timerBeforeSpawning);
    }

    public void Reset()
    {
        isShaking = false;
        transform.position = startingPosition;
        fallingPosition = startingPosition;
        child.GetComponent<Collider2D>().enabled = true;
        targetColor = startingColor;

    }
}
