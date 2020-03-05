using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerAttackListener : AttackListener
{
    private PlayerController player;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    public override void ReceiveAttack(Vector2 from, AttackType type)
    {
        Debug.Log("Player " + player.input.id + " hit");
        switch (type)
        {
            case AttackType.Player:
                player.velocity += ((Vector2)player.transform.position - from).normalized * 10 + Vector2.up * 10;
                player.actor.collisions.bellow = false;
                break;
            default:
                break;
        }
    }
}
