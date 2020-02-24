using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void HandleInput(PlayerController player, PlayerInput input);
    void Update(PlayerController playerController);
}

public class PlayerStanding : IPlayerState
{
    public void HandleInput(PlayerController player, PlayerInput input)
    {
        player.targetVelocity = input.moveAxis * player.settings.moveSpeed;
        if (input.moveAxis.x != 0)
        {
            float acceleration = player.settings.moveSpeed / player.settings.groundAccelerationTime;
            player.velocity.x = Mathf.MoveTowards(player.velocity.x, player.targetVelocity.x, acceleration * Time.deltaTime);
        }
        else
        {
            float deceleration = player.settings.moveSpeed / player.settings.groundDecelerationTime;
            player.velocity.x = Mathf.MoveTowards(player.velocity.x, player.targetVelocity.x, deceleration * Time.deltaTime);


            player.animator.SetBool("Idle", true);
            player.animator.SetBool("Running", false);
        }

        if (input.pressedJump)
        {
            player.state = new PlayerAirborne(true, player);

            player.velocity.y = Mathf.Sqrt(2 * player.settings.jumpHeight * player.settings.gravity);
            player.actor.collisions.bellow = false;

            GameManager.Instance.AddMetaInt(input.id == 1 ? MetaTag.PLAYER_1_JUMP : MetaTag.PLAYER_2_JUMP, 1);

            // Set Animator Jump
            player.animator.SetTrigger("Jump");
            player.animator.SetBool("Airborne", true);
        }
    }

    public void Update(PlayerController player)
    {
        if (player.actor.collisions.right || player.actor.collisions.left)
        {
            player.velocity.x = 0;
        }

        if (!player.actor.collisions.bellow)
        {
            player.state = new PlayerAirborne(false, player);

            // Set Animator Airborne -> We are falling
            player.animator.SetBool("Airborne", true);
        }

        // Animator Run Idle
        if (Mathf.Abs(player.targetVelocity.x) < 1.5f && player.input.moveAxis.x == 0)
        {
            player.animator.SetBool("Idle", true);
            player.animator.SetBool("Running", false);
        }
        else
        {
            player.animator.SetBool("Idle", false);
            player.animator.SetBool("Running", true);
        }

        // Orient Player
        if (player.velocity.x < 0)
            player.animator.transform.eulerAngles = Vector3.up * -90;
        else if (player.velocity.x > 0)
            player.animator.transform.eulerAngles = Vector3.up * 90;

        // Speed Animation
        player.animator.SetFloat("RunSpeedMultiplier", Mathf.Abs(player.input.moveAxis.x));
    }
}

public class PlayerAirborne : IPlayerState
{
    public bool canJump;
    public bool canDoubleJump;
    public bool canStopJump;
    public float coyoteTimer;
    public float coyoteDuration = 0.087f;
    float stopJumpSpeed;

    public PlayerAirborne(bool jump, PlayerController player)
    {
        canJump = !jump;
        canDoubleJump = player.input.doubleJump;
        canStopJump = jump;
        coyoteTimer = 0;
        stopJumpSpeed = player.settings.gravity / 9f;
    }

    public void HandleInput(PlayerController player, PlayerInput input)
    {
        player.targetVelocity = input.moveAxis * player.settings.moveSpeed;
        if (input.moveAxis.x != 0)
        {
            float acceleration = player.settings.moveSpeed / player.settings.airAccelerationTime;
            player.velocity.x = Mathf.MoveTowards(player.velocity.x, player.targetVelocity.x, acceleration * Time.deltaTime);
        }
        else
        {
            float deceleration = player.settings.moveSpeed / player.settings.airDecelerationTime;
            player.velocity.x = Mathf.MoveTowards(player.velocity.x, player.targetVelocity.x, deceleration * Time.deltaTime);
        }

        if (input.pressedJump)
        {
            if (canJump)
            {
                player.velocity.y = Mathf.Sqrt(2 * player.settings.jumpHeight * player.settings.gravity);
                
                canJump = false;
                canStopJump = true;

                // Set Animator Jump -> Simple Jump
                player.animator.SetTrigger("Jump");
                player.animator.SetBool("Airborne", true);
            } 
            else if(canDoubleJump)
            {
                player.velocity.y = Mathf.Sqrt(2 * player.settings.doubleJumpHeight * player.settings.gravity);

                canDoubleJump = false;
                canStopJump = true;

                // Set Animator Jump -> Simple Jump
                player.animator.SetTrigger("Jump");
                player.animator.SetBool("Airborne", true);
            }
        }

        if (canStopJump && player.velocity.y > stopJumpSpeed)
        {
            if (input.releasedJump)
            {
                canStopJump = false;
                player.velocity.y = stopJumpSpeed;
            }
        }
        else canStopJump = false;

    }

    public void Update(PlayerController player)
    {
        if (canJump)
        {
            coyoteTimer += Time.deltaTime;
            if (coyoteTimer > coyoteDuration) canJump = false;
        }

        if (player.actor.collisions.right || player.actor.collisions.left)
        {
            player.velocity.x = 0;
        }

        if (player.actor.collisions.bellow)
        {
            player.state = new PlayerStanding();

            // Set Animator Airborne -> We are Landing
            player.animator.SetBool("Airborne", false);
            player.animator.SetBool("Idle", true);
            player.animator.SetBool("Running", false);

        }

        // Animator Run Idle
        if (Mathf.Abs(player.velocity.x) < 1.1f)
        {
            player.animator.SetBool("Idle", true);
            player.animator.SetBool("Running", false);
        }
        else
        {
            player.animator.SetBool("Idle", false);
            player.animator.SetBool("Running", true);
        }

        // Orient Player
        if (player.velocity.x < 0)
            player.animator.transform.eulerAngles = Vector3.up * -90;
        else if (player.velocity.x > 0)
            player.animator.transform.eulerAngles = Vector3.up * 90;
    }
}
