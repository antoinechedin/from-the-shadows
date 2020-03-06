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
        switch (type)
        {
            case AttackType.Player:
                player.velocity = (((Vector2)player.transform.position - from).normalized + Vector2.up).normalized * 15;
                player.actor.collisions.bellow = false;
                break;
            default:
                break;
        }
    }
}
