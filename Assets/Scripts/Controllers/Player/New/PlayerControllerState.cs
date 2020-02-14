using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerControllerState
{
    void HandleInput(NewPlayerController playerController, PlayerInput playerInput);
    void Update(NewPlayerController playerController);
}

public class PlayerStanding : IPlayerControllerState
{
    public void HandleInput(NewPlayerController player, PlayerInput playerInput)
    {
        player.targetVelocity = playerInput.moveAxis.normalized * player.settings.moveSpeed;
        if (playerInput.moveAxis.x != 0)
        {
            float acceleration = player.settings.moveSpeed / player.settings.groundAccelerationTime;
            player.velocity.x = Mathf.MoveTowards(player.velocity.x, player.targetVelocity.x, acceleration * Time.deltaTime);
        }
        else
        {
            float deceleration = player.settings.moveSpeed / player.settings.groundDecelerationTime;
            player.velocity.x = Mathf.MoveTowards(player.velocity.x, player.targetVelocity.x, deceleration * Time.deltaTime);
        }

        if (playerInput.pressedJump)
        {
            player.state = new PlayerAirborne(true, player);

            player.velocity.y = Mathf.Sqrt(2 * player.settings.jumpHeight * player.settings.gravity);
            player.controller.collisions.bellow = false;
        }
    }

    public void Update(NewPlayerController player)
    {
        if (player.controller.collisions.right || player.controller.collisions.left)
        {
            player.velocity.x = 0;
        }

        if (!player.controller.collisions.bellow)
        {
            player.state = new PlayerAirborne(false, player);
        }
    }
}

public class PlayerAirborne : IPlayerControllerState
{
    public bool canJump;
    public bool canDoubleJump;
    public bool canStopJump;
    public float coyoteTimer;
    public float coyoteDuration = 0.07f;
    float stopJumpSpeed;

    public PlayerAirborne(bool jump, NewPlayerController playerController)
    {
        canJump = !jump;
        canDoubleJump = playerController.playerInput.doubleJump;
        canStopJump = jump;
        coyoteTimer = 0;
        stopJumpSpeed = playerController.settings.gravity / 9f;
    }

    public void HandleInput(NewPlayerController player, PlayerInput playerInput)
    {
        player.targetVelocity = playerInput.moveAxis.normalized * player.settings.moveSpeed;
        if (playerInput.moveAxis.x != 0)
        {
            float acceleration = player.settings.moveSpeed / player.settings.airAccelerationTime;
            player.velocity.x = Mathf.MoveTowards(player.velocity.x, player.targetVelocity.x, acceleration * Time.deltaTime);
        }
        else
        {
            float deceleration = player.settings.moveSpeed / player.settings.airDecelerationTime;
            player.velocity.x = Mathf.MoveTowards(player.velocity.x, player.targetVelocity.x, deceleration * Time.deltaTime);
        }

        if (playerInput.pressedJump)
        {
            if (canJump)
            {
                player.velocity.y = Mathf.Sqrt(2 * player.settings.jumpHeight * player.settings.gravity);
                
                canJump = false;
                canStopJump = true;
            } 
            else if(canDoubleJump)
            {
                player.velocity.y = Mathf.Sqrt(2 * player.settings.doubleJumpHeight * player.settings.gravity);

                canDoubleJump = false;
                canStopJump = true;
            }
        }

        if (canStopJump && player.velocity.y > stopJumpSpeed)
        {
            if (playerInput.releasedJump)
            {
                canStopJump = false;
                player.velocity.y = stopJumpSpeed;
            }
        }
        else canStopJump = false;

    }

    public void Update(NewPlayerController playerController)
    {
        if (canJump)
        {
            coyoteTimer += Time.deltaTime;
            if (coyoteTimer > coyoteDuration) canJump = false;
        }

        if (playerController.controller.collisions.right || playerController.controller.collisions.left)
        {
            playerController.velocity.x = 0;
        }

        if (playerController.controller.collisions.bellow)
        {
            playerController.state = new PlayerStanding();
        }
    }
}
