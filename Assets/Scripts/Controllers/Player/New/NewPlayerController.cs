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
        grounded = controller.collisions.bellow;
        UpdateSpriteColor();
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
