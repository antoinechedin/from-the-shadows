using UnityEngine;

[CreateAssetMenu(fileName = "Default Player", menuName = "Settings/Physics", order = 1)]
public class PhysicsSettings : ScriptableObject
{
    /// <summary>
    /// Maximum speed in unit/second that the character can moves.
    /// </summary>
    public float moveSpeed = 8;
    /// <summary>
    /// Height in unit that the player can jump
    /// </summary>
    public float jumpHeight = 3;
    /// <summary>
    /// Double jump height in unit. The double jump is enable in PlayerController
    /// </summary>
    public float doubleJumpHeight= 1.5f;
    /// <summary>
    /// Gravity in unit*unit/second
    /// </summary>
    public float gravity = 40;
    /// <summary>
    /// Max fall speed in unit/second that the player can fall
    /// </summary>
    public float maxFallSpeed = 15;

    /// <summary>
    /// Max angle in degree the player can walk on
    /// </summary>
    public float maxSlopeAngle = 60;

    /// <summary>
    /// Time in second needed for the player to reach max speed on the ground
    /// </summary>
    public float groundAccelerationTime = 0.07f;
    /// <summary>
    ///  Time in second needed for the player to stop themself on the ground
    /// </summary>
    public float groundDecelerationTime = 0.07f;
    /// <summary>
    ///  Time in second needed for the player to reach max speed while in the air
    /// </summary>
    public float airAccelerationTime = 0.14f;
    /// <summary>
    /// Time in second needed for the player to stop themself while the air
    /// </summary>
    public float airDecelerationTime = 0.14f;
}
