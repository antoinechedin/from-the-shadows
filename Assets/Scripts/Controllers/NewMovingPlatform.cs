using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NewSolidController))]
public class NewMovingPlatform : MonoBehaviour
{
    private float timer;
    private Vector2 originalPos;

    public Vector2 direction;

    private NewSolidController controller;

    private void Awake()
    {
        controller = GetComponent<NewSolidController>();
        originalPos = transform.position;
    }

    private float MoveFunc(float x)
    {
        return Mathf.Sin(Mathf.PI * x / 3f);
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Vector2 newPos = originalPos;
        newPos += direction * MoveFunc(timer);

        controller.Move(newPos - (Vector2)transform.position);
    }
}
