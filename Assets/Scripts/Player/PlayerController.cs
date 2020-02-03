using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActorController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float gravity = 40f;
    public float jumpHeight = 4f;
    public float groundAccelerationTime = 0.07f;
    public float groundDecelerationTime = 0.07f;
    public float airAccelerationTime = 0.14f;
    public float airDecelerationTime = 0.14f;

    [HideInInspector] public PlayerState state;
    [HideInInspector] public Vector2 targetVelocity;
    [HideInInspector] public bool jumpInput;
    [HideInInspector] public Vector2 velocity;
    private ActorController controller;
    private PlayerSoundPlayer soundPlayer;

    private bool canStopJump = false;
    private Vector2 velocitySmoothing;

    private void Awake()
    {
        velocity = targetVelocity = velocitySmoothing = new Vector2();
        controller = GetComponent<ActorController>();
        soundPlayer = GetComponent<PlayerSoundPlayer>();
    }

    public void HandleInput()
    {
        switch (state)
        {
            case PlayerState.Standing:
                targetVelocity.x = Input.GetAxisRaw("Horizontal") * moveSpeed;

                velocity.x = Mathf.SmoothDamp(
                    velocity.x,
                    targetVelocity.x,
                    ref velocitySmoothing.x,
                    targetVelocity.x != 0 ? groundAccelerationTime : groundDecelerationTime
                );
                velocity.y = 0;

                if (Input.GetButtonDown("Jump"))
                {
                    state = PlayerState.Airborne;
                    velocity.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                    canStopJump = true;

                    // TODO: Add jump metadata
                    soundPlayer.PlaySoundAtLocation(soundPlayer.jump, 1);
                }
                break;

            case PlayerState.Airborne:
                targetVelocity.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
                velocity.x = Mathf.SmoothDamp(
                    velocity.x,
                    targetVelocity.x,
                    ref velocitySmoothing.x,
                    targetVelocity.x != 0 ? airAccelerationTime : airDecelerationTime
                );

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
        controller.Move(velocity * Time.fixedDeltaTime);

        switch (state)
        {
            case PlayerState.Standing:
                if (controller.collisions.left || controller.collisions.right)
                {
                    //velocity.x = 0;
                }
                if (!controller.collisions.below)
                {
                    state = PlayerState.Airborne;
                }
                break;

            case PlayerState.Airborne:
                if (controller.collisions.left || controller.collisions.right)
                {
                    //velocity.x = 0;
                }
                if (controller.collisions.below || controller.collisions.above)
                {
                    if (controller.collisions.below)
                    {
                        state = PlayerState.Standing;

                        soundPlayer.PlaySoundAtLocation(soundPlayer.landing, 1);
                    }

                    velocity.y = 0;
                }
                break;
        }
    }

    private void FixedUpdate()
    {

    }
}

public enum PlayerState
{
    Standing,
    Airborne
}
