using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void HandleInput(PlayerController player, PlayerInput input);
    void Update(PlayerController playerController);
    void FixedUpdate(PlayerController playerController);
}

public class PlayerStanding : IPlayerState
{

    //public bool turning = false;
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

        }

        if (input.pressedJump)
        {
            player.state = new PlayerAirborne(true, false, player);

            player.velocity.y = Mathf.Sqrt(2 * player.settings.jumpHeight * player.settings.gravity);
            player.actor.collisions.bellow = false;

            GameManager.Instance.AddMetaInt(input.id == 1 ? MetaTag.PLAYER_1_JUMP : MetaTag.PLAYER_2_JUMP, 1);

            // Set Animator Jump
            player.animator.SetTrigger("Jump");
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
            player.state = new PlayerAirborne(false, false, player);

            // Set Animator Airborne -> We are falling
            player.animator.SetTrigger("Airborne");
        }

        // Speed Animation
        //player.animator.SetFloat("RunSpeedMultiplier", Mathf.Abs(player.input.moveAxis.x) + 0.1f);
        player.animator.SetFloat("speedBlend", Mathf.Abs(player.velocity.x) / player.settings.moveSpeed);
        
        if (Mathf.Abs(player.velocity.x) > 0 && player.input.moveAxis.x != 0 && Mathf.Sign(player.input.moveAxis.x) != player.facing)
        {
            if (Mathf.Sign(player.input.moveAxis.x) == 1)
                player.animator.transform.eulerAngles = Vector3.up * 90;
            else
                player.animator.transform.eulerAngles = Vector3.up * -90;
            if(!player.animator.GetCurrentAnimatorStateInfo(0).IsName("Turn"))
                player.animator.SetTrigger("turn");
        }
        
        else
        {
            if (player.facing == 1)
                player.animator.transform.eulerAngles = Vector3.up * 90;
            else
                player.animator.transform.eulerAngles = Vector3.up * -90;
        }
    }

    public void FixedUpdate(PlayerController player)
    {

    }
}

public class PlayerAirborne : IPlayerState
{
    public bool canJump;
    public bool canDoubleJump;
    public bool canStopJump;
    public float coyoteTimer;
    public float coyoteDuration = 0.087f;
    public float lastLedgeGrabTimer;
    public float lastLedgeGrabDuration = 0.07f;
    float stopJumpSpeed;

    public PlayerAirborne(bool jump, bool dropLedge, PlayerController player)
    {
        canJump = true;
        canStopJump = jump;
        stopJumpSpeed = player.settings.gravity / 7f;
        canDoubleJump = player.input.doubleJump;
        coyoteTimer = 0;

        if (jump || dropLedge)
        {
            canJump = false;
        }

        if (dropLedge) lastLedgeGrabTimer = 0;
        else lastLedgeGrabTimer = lastLedgeGrabDuration;
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
            }
            else if (canDoubleJump)
            {
                player.velocity.y = Mathf.Sqrt(2 * player.settings.doubleJumpHeight * player.settings.gravity);

                canDoubleJump = false;
                canStopJump = true;

                // Set Animator Jump -> Simple Jump
                player.animator.SetTrigger("Jump");
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

            // Landing
            player.animator.SetTrigger("Land");
            return;
        }

        if (lastLedgeGrabTimer >= lastLedgeGrabDuration)
        {
            if (player.velocity.y < 0 && player.actor.LedgeGrab(player.facing, false))
            {
                player.state = new PlayerLedgeGrab(player);
                player.velocity = Vector2.zero;

                player.animator.SetTrigger("LedgeGrab");
            }
        }
        else
        {
            lastLedgeGrabTimer += Time.deltaTime;
        }
    }

    public void FixedUpdate(PlayerController player)
    {
        if (player.facing == 1)
            player.animator.transform.eulerAngles = Vector3.up * 90;
        else
            player.animator.transform.eulerAngles = Vector3.up * -90;
    }
}

public class PlayerLedgeGrab : IPlayerState
{
    public PlayerLedgeGrab(PlayerController player)
    {
        player.velocity = Vector2.zero;
    }

    public void HandleInput(PlayerController player, PlayerInput input)
    {
        if (input.moveAxis.y < -0.7f)
        {
            player.state = new PlayerAirborne(false, true, player);
            player.animator.SetTrigger("LedgeDrop");
        }

        if (input.pressedJump)
        {
            player.state = new PlayerAirborne(true, false, player);

            player.velocity.y = Mathf.Sqrt(2 * player.settings.jumpHeight * player.settings.gravity);
            player.actor.collisions.bellow = false;

            GameManager.Instance.AddMetaInt(input.id == 1 ? MetaTag.PLAYER_1_JUMP : MetaTag.PLAYER_2_JUMP, 1);

            // Set Animator Jump
            player.animator.SetTrigger("Jump");
        }
    }

    public void Update(PlayerController player)
    {
        if (!player.actor.LedgeGrab(player.facing, true))
        {
            player.state = new PlayerAirborne(false, true, player);
            player.animator.SetTrigger("LedgeDrop");
        }
    }

    public void FixedUpdate(PlayerController player)
    {

    }
}
