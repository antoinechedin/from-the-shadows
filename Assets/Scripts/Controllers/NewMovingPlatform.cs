using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NewSolidController))]
public class NewMovingPlatform : MonoBehaviour
{
    private float timer;
    private Vector2 originalPos;

    private NewSolidController controller;

    private void Awake() {
        controller = GetComponent<NewSolidController>();
        originalPos = transform.position;
    }

    private float MoveFunc(float x)
    {
        return Mathf.Sin(Mathf.PI * x / 3f) * 2;
    }

    private void Update() {
        timer += Time.deltaTime; 
    }

    private void FixedUpdate() {
        Vector2 newPos = originalPos;
        newPos += Vector2.right * MoveFunc(timer * 3) / 3f;
        newPos += Vector2.up * MoveFunc(timer / 3f * 3);

        controller.Move(newPos - (Vector2)transform.position);
    }
}
