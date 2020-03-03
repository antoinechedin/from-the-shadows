using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreBossTrigger : MonoBehaviour, IResetable
{
    public bool triggered = false;

    public float newTimetoReachMaxSpeed = 5f;
    public float newEndingSpeed = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            triggered = true;
        }
    }

    public void Reset()
    {
        triggered = false;
    }
}
