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
            player.grounded = false;
        }
    }

    public void Update(NewPlayerController player)
    {
        if (!player.grounded)
        {
            player.state = new PlayerAirborne(false, player);
        }
    }
}

public class PlayerAirborne : IPlayerControllerState
{
    public bool canJump;
    public bool canDoubleJump;
    public float coyoteTimer;
    public float coyoteDuration = 0.3f;

    public PlayerAirborne(bool jump, NewPlayerController playerController)
    {
        canJump = !jump;
        canDoubleJump = playerController.settings.doubleJumpHeight > 0;
        coyoteTimer = 0;
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

        if (playerInput.pressedJump && canJump)
        {
            canJump = false;
            player.velocity.y = Mathf.Sqrt(2 * player.settings.jumpHeight * player.settings.gravity);
        }
    }

    public void Update(NewPlayerController playerController)
    {
        if (canJump)
        {
            coyoteTimer += Time.deltaTime;
            if (coyoteTimer > coyoteDuration) canJump = false;
        }

        if (playerController.grounded)
        {
            playerController.state = new PlayerStanding();
        }
    }
}
