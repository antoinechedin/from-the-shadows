using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NewActorController), typeof(PlayerInput))]
public class NewPlayer : MonoBehaviour
{
    public PhysicsSettings settings;

    private NewActorController controller;
    private PlayerInput playerInput;

    private Vector2 targetVelocity;
    private Vector2 velocity;

    private void Awake()
    {
        controller = GetComponent<NewActorController>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (controller.collisions.bellow || controller.collisions.above)
        {
            if (!controller.collisions.slidingSlope)
                velocity.y = 0;
        }

        targetVelocity = playerInput.moveAxis.normalized * settings.moveSpeed;
        if (playerInput.moveAxis.x != 0)
        {
            float acceleration = settings.moveSpeed / settings.groundAccelerationTime;
            velocity.x = Mathf.MoveTowards(velocity.x, targetVelocity.x, acceleration * Time.deltaTime);
        }
        else
        {
            float deceleration = settings.moveSpeed / settings.groundDecelerationTime;
            velocity.x = Mathf.MoveTowards(velocity.x, targetVelocity.x, deceleration * Time.deltaTime);
        }

        velocity.y -= settings.gravity * Time.deltaTime;
        velocity.y = Mathf.Clamp(velocity.y, -settings.maxFallSpeed, Mathf.Infinity);
    }

    private void FixedUpdate()
    {
        controller.Move(velocity, Time.fixedDeltaTime);
        UpdateSpriteColor();
    }

    private void UpdateSpriteColor()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (controller.collisions.bellow || controller.collisionsPrevious.bellow)
            {
                sr.color = Color.green;
            }
            else
            {
                sr.color = Color.blue;
            }
        }
    }
}
