using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActorController), typeof(PlayerSoundPlayer))]
[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    public int id = 1;

    public PhysicsSettings settings;
    public bool doubleJump;

    [HideInInspector] public PlayerState state;
    [HideInInspector] public Vector2 targetVelocity;
    [HideInInspector] public bool jumpInput;
    [HideInInspector] public Vector2 velocity;
    private Vector2 oldPosition;
    private ActorController controller;
    private PlayerSoundPlayer soundPlayer;
    private Animator animator;

    private bool canStopJump;
    private bool canDoubleJump;
    private Vector2 velocitySmoothing;

    private void Awake()
    {
        controller = GetComponent<ActorController>();
        soundPlayer = GetComponent<PlayerSoundPlayer>();
        animator = GetComponent<Animator>();

        oldPosition = transform.position;
        velocity = targetVelocity = velocitySmoothing = new Vector2();
        canStopJump = false;
        canDoubleJump = doubleJump;

        controller.maxClimbAngle = settings.maxSlopeAngle;
        controller.maxDescendAngle = settings.maxSlopeAngle;
    }

    public void HandleInput()
    {
        switch (state)
        {
            case PlayerState.Standing:
                targetVelocity.x = Input.GetAxisRaw("Horizontal_" + id) * settings.moveSpeed;

                velocity.x = Mathf.SmoothDamp(
                    velocity.x,
                    targetVelocity.x,
                    ref velocitySmoothing.x,
                    targetVelocity.x != 0 ? settings.groundAccelerationTime : settings.groundDecelerationTime
                );
                velocity.y = 0;

                if (Input.GetButtonDown("A_" + id))
                {
                    animator.SetTrigger("Jump");

                    state = PlayerState.Airborne;
                    velocity.y = Mathf.Sqrt(2 * settings.jumpHeight * settings.gravity);
                    canStopJump = true;

                    GameManager.Instance.AddMetaInt("jumpNumber" + id, 1);
                    //TODO : save for each player, using player index

                    soundPlayer.PlaySoundAtLocation(soundPlayer.jump, 1);
                }
                break;

            case PlayerState.Airborne:
                targetVelocity.x = Input.GetAxisRaw("Horizontal_" + id) * settings.moveSpeed;
                velocity.x = Mathf.SmoothDamp(
                    velocity.x,
                    targetVelocity.x,
                    ref velocitySmoothing.x,
                    targetVelocity.x != 0 ? settings.airAccelerationTime : settings.airDecelerationTime
                );

                float stopJumpSpeed = settings.gravity / 6f;

                if (velocity.y < stopJumpSpeed) canStopJump = false;
                else
                {
                    if (canStopJump && Input.GetButtonUp("A_" + id))
                    {
                        velocity.y = stopJumpSpeed;
                    }
                }

                if (canDoubleJump && Input.GetButtonDown("A_" + id))
                {
                    animator.SetTrigger("Jump");
                    canDoubleJump = false;
                    velocity.y = Mathf.Sqrt(2 * settings.doubleJumpHeight * settings.gravity);
                    canStopJump = true;

                    GameManager.Instance.AddMetaInt("jumpNumber" + id, 1);
                    soundPlayer.PlaySoundAtLocation(soundPlayer.jump, 1);
                }

                break;
        }

        velocity.y -= settings.gravity * Time.deltaTime;
    }

    private void AnimControllerUpdate()
    {
        if (velocity.x > 0 && GetComponent<SpriteRenderer>().flipX) GetComponent<SpriteRenderer>().flipX = false;
        if (velocity.x < 0 && !GetComponent<SpriteRenderer>().flipX) GetComponent<SpriteRenderer>().flipX = true;
        animator.SetBool("Running", (int)velocity.x != 0);
        animator.SetBool("Airborne", state == PlayerState.Airborne);
    }

    private void Update()
    {
        HandleInput();
        controller.Move(velocity * Time.deltaTime);
        float distance = ((Vector2)transform.position - oldPosition).magnitude;
        oldPosition = transform.position;
        GameManager.Instance.AddMetaFloat("distance" + id, distance);
        //TODO : save for each player, using player index

        AnimControllerUpdate();

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
                        canDoubleJump = doubleJump;
                        state = PlayerState.Standing;

                        soundPlayer.PlaySoundAtLocation(soundPlayer.landing, 1);
                    }

                    velocity.y = 0;
                }
                break;
        }
    }
}

public enum PlayerState
{
    Standing,
    Airborne
}
