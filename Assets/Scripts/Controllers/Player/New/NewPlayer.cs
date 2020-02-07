using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NewActorController))]
public class NewPlayer : MonoBehaviour
{
    public PlayerSettings settings;
    private NewActorController controller;

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
            controller.MoveX(xAxis * settings.moveSpeed * Time.deltaTime);
        }
    }
}
