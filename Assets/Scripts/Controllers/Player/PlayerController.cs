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
    [HideInInspector] public Animator animator;
    public IPlayerState state;
    public IPlayerState attackState;
    public Vector2 targetVelocity;
    public Vector2 velocity;
    public int xVelocitySign;

    public bool dead = false;
    public bool dying = false;

    private void Awake()
    {
        actor = GetComponent<ActorController>();
        actor.maxSlopeAngle = settings.maxSlopeAngle;
        input = GetComponent<PlayerInput>();
        state = new PlayerStanding();
        attackState = new MeleAttackState();
        animator = GetComponentInChildren<Animator>();
        xVelocitySign = 1;
    }

    private void Update()
    {
        state.HandleInput(this, input);
        if (attackState != null) attackState.HandleInput(this, input);
    }

    private void FixedUpdate()
    {
        if (actor.collisions.bellow || actor.collisions.above)
        {
            if (!actor.collisions.slidingSlope)
                velocity.y = 0;
        }

        if (state is PlayerLedgeGrab)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y -= settings.gravity * Time.deltaTime;
            velocity.y = Mathf.Clamp(velocity.y, -settings.maxFallSpeed, Mathf.Infinity);
        }

        if (velocity.x > 0)
        {
            if (xVelocitySign != 1)
            {
                xVelocitySign = 1;
            }

        }
        else if (velocity.x < 0)
        {
            if (xVelocitySign != -1)
            {
                xVelocitySign = -1;
            }
        }

        actor.Move(velocity, Time.fixedDeltaTime);

        state.FixedUpdate(this);
        if (attackState != null) attackState.FixedUpdate(this);

        GameManager.Instance.AddMetaFloat(
            input.id == 1 ? MetaTag.PLAYER_1_DISTANCE : MetaTag.PLAYER_2_DISTANCE,
            actor.collisions.move.magnitude
        );
    }

    public void Die()
    {
        if (!dying)
        {
            input.active = false;
            dying = true;
            dead = true;
            GameObject.FindObjectOfType<ChapterManager>().ResetLevel(input.id);
        }
    }

    public void SpawnAt(Vector3 position)
    {
        transform.position = position;
        state = new PlayerStanding();
        velocity = Vector2.zero;
        targetVelocity = Vector2.zero;
        xVelocitySign = 1;

        animator.Rebind();
        animator.SetBool("Light", input.doubleJump);
    }
}