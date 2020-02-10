using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NewActorController))]
public class NewPlayer : MonoBehaviour
{
    public PlayerSettings settings;
    private NewActorController controller;
    
    private Vector2 velocity;

    private void Awake()
    {
        controller = GetComponent<NewActorController>();
        if (settings == null)
        {
            Debug.LogError("ERROR:PLAYER: Player \"" + Utils.GetFullName(transform) + "\" hasn't any player settings");
        }
    }

    private void Update()
    {
        if (settings != null)
        {
            float xAxis = Input.GetAxisRaw("Horizontal_G");
            float yAxis = Input.GetAxisRaw("Vertical_G");
            // xAxis = -1f;
            // yAxis = -1f;
            velocity = (Vector2.right * xAxis + Vector2.up * yAxis) * settings.moveSpeed;
        }
    }

    private void FixedUpdate() {
        controller.MoveX(velocity.x * Time.fixedDeltaTime);
        controller.MoveY(velocity.y * Time.fixedDeltaTime);
    }
}
