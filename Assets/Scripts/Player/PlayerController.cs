using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActorController))]
public class PlayerController : MonoBehaviour
{
    public float groundSpeed = 8f;
    public float gravity = 40f;
    public float jumpImpulse = 17f;

    [HideInInspector] public PlayerState state;
    [HideInInspector] public Vector2 input;
    [HideInInspector] public bool jumpInput;
    [HideInInspector] public Vector2 velocity;
    private ActorController controller;

    private bool canStopJump = false;

    private void Awake()
    {
        input = new Vector2();
        velocity = new Vector2();
        controller = GetComponent<ActorController>();
    }

    public void HandleInput()
    {
        switch (state)
        {
            case PlayerState.Standing:
                velocity.x = Input.GetAxisRaw("Horizontal") * groundSpeed;
                velocity.y = 0;

                if (Input.GetButtonDown("Jump"))
                {
                    state = PlayerState.Jumping;
                    velocity.y = jumpImpulse;
                    canStopJump = true;

                    // TODO: Add jump metadata
                }
                break;

            case PlayerState.Jumping:
                velocity.x = Input.GetAxisRaw("Horizontal") * groundSpeed;

                float stopJumpSpeed = gravity / 6f;

                if (velocity.y < stopJumpSpeed) canStopJump = false;
                else
                {
                    if (canStopJump && Input.GetButtonUp("Jump"))
                    {
                        velocity.y = stopJumpSpeed;
                    }
                }
                break;
        }

        velocity.y -= gravity * Time.deltaTime;
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        controller.Move(velocity * Time.fixedDeltaTime);

        switch (state)
        {
            case PlayerState.Standing:
                if (!controller.collisions.below)
                {
                    state = PlayerState.Jumping;
                }
                break;

            case PlayerState.Jumping:
                if (controller.collisions.below || controller.collisions.above)
                {
                    if (controller.collisions.below) state = PlayerState.Standing;
                    velocity.y = 0;
                }
                break;
        }
    }
}

public enum PlayerState
{
    Standing,
    Jumping
}
