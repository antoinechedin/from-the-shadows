using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ghost))]
public class GhostAttackListener : AttackListener
{
    private Ghost ghost;

    private void Awake()
    {
        ghost = GetComponent<Ghost>();
    }

    public override void ReceiveAttack(Vector2 from, AttackType type)
    {
        switch (type)
        {
            case AttackType.Player:
                ghost.Die(from);
                break;
            default:
                break;
        }
    }
}
