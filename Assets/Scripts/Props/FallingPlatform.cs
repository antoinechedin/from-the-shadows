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
    private Vector3 fallingPosition;
    private Color targetColor;
    private Color startingColor;

    private Transform mesh;
    private Collider2D plaformCollider;

    // Start is called before the first frame update
    private void Start()
    {
        if (transform.childCount < 2)
        {
            Debug.LogWarning("WARN FallingPlatform.Start: " + Utils.GetFullName(transform)
                             + " is invalid, couldn't find child mesh/collider");
        }
        else
        {
            plaformCollider = transform.GetChild(0).GetComponent<Collider2D>();
            mesh = transform.GetChild(1);
        }
        if (plaformCollider == null)
        {
            Debug.LogWarning("WARN FallingPlatform.Start: "  + Utils.GetFullName(transform.GetChild(0))
                             + " don't have 2D Collider");
        }

        if (mesh != null)
        {
            startingPosition = transform.position;
            startingColor = mesh.GetComponent<MeshRenderer>().material.GetColor("_BaseColor");
            targetColor = startingColor;
            fallingPosition = transform.position;
        }

    }

    public void Update()
    {
        if (mesh != null)
        {
            if (isShaking)
                mesh.position = new Vector3(mesh.position.x + Mathf.Sin(Time.time * shakeSpeed) * shakeIntensity,
                                            mesh.position.y,
                                            mesh.position.z);

            Color color = mesh.GetComponent<MeshRenderer>().material.GetColor("_BaseColor");
            Color fade = Color.Lerp(color, targetColor, Time.deltaTime * 10);
            mesh.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", fade);
            mesh.position = Vector3.Lerp(mesh.position, fallingPosition, Time.deltaTime * 5);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            isShaking = true;
            isFalling = true;
            Invoke("Fall", timerBeforeFalling);
        }
    }

    public void Fall()
    {
        if (plaformCollider != null)
            plaformCollider.enabled = false;
        targetColor = new Color(startingColor.r, startingColor.g, startingColor.b, 0);
        fallingPosition = mesh.position - new Vector3(0, 5, 0);
        Invoke("DeactivateChilds", 0.4f);
        Invoke("Reset", timerBeforeSpawning);
    }

    public void Reset()
    {
        CancelInvoke();
        isShaking = false;
        isFalling = false;
        if (mesh != null)
        {
            mesh.position = startingPosition;
        }
        fallingPosition = startingPosition;
        plaformCollider.enabled = true;
        targetColor = startingColor;
        ActivateChilds();
    }

    public void DeactivateChilds()
    {
        for (int i = 0; i < mesh.childCount; i++)
        {
            mesh.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void ActivateChilds()
    {
        for (int i = 0; i < mesh.childCount; i++)
        {
            mesh.GetChild(i).gameObject.SetActive(true);
        }
    }
}
