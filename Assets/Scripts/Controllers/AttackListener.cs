using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackListener : MonoBehaviour
{
    public abstract void ReceiveAttack(Vector2 from, AttackType type);
}

public enum AttackType
{
    Player,
    Monster
}
