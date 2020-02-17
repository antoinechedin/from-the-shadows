using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActorController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public PhysicsSettings settings;

    [HideInInspector] public ActorController actor;
    [HideInInspector] public PlayerInput input;

    public IPlayerState state;
    public Vector2 targetVelocity;
    public Vector2 velocity;

    public bool dead = false;

    private void Awake()
    {
        actor = GetComponent<ActorController>();
        actor.maxSlopeAngle = settings.maxSlopeAngle;
        input = GetComponent<PlayerInput>();
        state = new PlayerStanding();
    }

    private void Update()
    {
        state.HandleInput(this, input);
        state.Update(this);

        if (actor.collisions.bellow || actor.collisions.above)
        {
            if (!actor.collisions.slidingSlope)
                velocity.y = 0;
        }

        velocity.y -= settings.gravity * Time.deltaTime;
        velocity.y = Mathf.Clamp(velocity.y, -settings.maxFallSpeed, Mathf.Infinity);
    }

    private void FixedUpdate()
    {
        actor.Move(velocity, Time.fixedDeltaTime);
        UpdateSpriteColor();
    }

    private void UpdateSpriteColor()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (actor.collisions.bellow)
            {
                sr.color = Color.green;
            }
            else
            {
                sr.color = Color.blue;
            }
        }
    }

    public void Die()
    {
        //lancer l'animation de mort. C'est l'animnation que settera dead = true toute seule comme une grande
    }
}