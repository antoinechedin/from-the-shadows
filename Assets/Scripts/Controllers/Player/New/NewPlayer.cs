using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NewActorController))]
public class NewPlayer : MonoBehaviour
{
    public PlayerSettings settings;

    private NewActorController controller;

    private Vector2 targetVelocity;
    private Vector2 velocity;

    private void Awake()
    {
        controller = GetComponent<NewActorController>();
    }

    private void Update()
    {
        Vector2 inputAxis;
        inputAxis.x = Input.GetAxisRaw("Horizontal_G");
        inputAxis.y = Input.GetAxisRaw("Vertical_G");

        targetVelocity = inputAxis.normalized * settings.moveSpeed;
        if (inputAxis.x != 0)
        {
            float acceleration = settings.moveSpeed / settings.groundAccelerationTime; 
            velocity.x = Mathf.MoveTowards(velocity.x, targetVelocity.x, acceleration * Time.deltaTime);
        }
        else
        {
            float deceleration = settings.moveSpeed /settings.groundDecelerationTime;
            velocity.x = Mathf.MoveTowards(velocity.x, targetVelocity.x, deceleration * Time.deltaTime);
        }
        
        velocity.y = targetVelocity.y;
    }

    private void FixedUpdate()
    {
        velocity = controller.Move(velocity, Time.fixedDeltaTime);
    }
}
