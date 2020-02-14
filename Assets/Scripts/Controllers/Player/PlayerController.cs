using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActorController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public PhysicsSettings settings;

    public ActorController controller;
    public PlayerInput playerInput;

    public IPlayerControllerState state;
    public Vector2 targetVelocity;
    public Vector2 velocity;

    private void Awake()
    {
        controller = GetComponent<ActorController>();
        playerInput = GetComponent<PlayerInput>();
        state = new PlayerStanding();
    }

    private void Update()
    {        
        state.HandleInput(this, playerInput);
        state.Update(this);

        if (controller.collisions.bellow || controller.collisions.above)
        {
            if (!controller.collisions.slidingSlope)
                velocity.y = 0;
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
            if (controller.collisions.bellow)
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
