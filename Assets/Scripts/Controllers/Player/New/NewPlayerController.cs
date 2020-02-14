using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NewActorController), typeof(PlayerInput))]
public class NewPlayerController : MonoBehaviour
{
    public PhysicsSettings settings;

    public NewActorController controller;
    private PlayerInput playerInput;

    public IPlayerControllerState state;
    public Vector2 targetVelocity;
    public Vector2 velocity;
    public bool grounded;

    private void Awake()
    {
        controller = GetComponent<NewActorController>();
        playerInput = GetComponent<PlayerInput>();
        state = new PlayerStanding();
    }

    private void Update()
    {
        grounded = controller.collisions.bellow;
        
        state.HandleInput(this, playerInput);
        state.Update(this);

        if (controller.collisions.bellow || controller.collisions.above)
        {
            if (!controller.collisions.slidingSlope)
                velocity.y = 0;
        }

        //if (playerInput.pressJump)
        // {
        //     velocity.y = Mathf.Sqrt(2 * settings.jumpHeight * settings.gravity);
        //     controller.collisions.bellow = false;
        // }

        // targetVelocity = playerInput.moveAxis.normalized * settings.moveSpeed;
        // if (playerInput.moveAxis.x != 0)
        // {
        //     float acceleration = settings.moveSpeed / settings.groundAccelerationTime;
        //     velocity.x = Mathf.MoveTowards(velocity.x, targetVelocity.x, acceleration * Time.deltaTime);
        // }
        // else
        // {
        //     float deceleration = settings.moveSpeed / settings.groundDecelerationTime;
        //     velocity.x = Mathf.MoveTowards(velocity.x, targetVelocity.x, deceleration * Time.deltaTime);
        // }

        velocity.y -= settings.gravity * Time.deltaTime;
        velocity.y = Mathf.Clamp(velocity.y, -settings.maxFallSpeed, Mathf.Infinity);
    }

    private void FixedUpdate()
    {
        controller.Move(velocity, Time.fixedDeltaTime);
        UpdateSpriteColor();
    }

    private void HandleInput()
    {

    }

    private void UpdateState()
    {

    }

    private void UpdateSpriteColor()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (grounded)
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
