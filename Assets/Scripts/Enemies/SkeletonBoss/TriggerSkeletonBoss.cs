using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSkeletonBoss : MonoBehaviour, IResetable
{
    private bool isTriggered = false;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && !isTriggered)
        {
            isTriggered = true;
            FindObjectOfType<ChapterManager>().ShakeFor(0.6f, 1f, 5f);
            transform.parent.GetComponent<Skeleton>().Appear();
        }
    }

    public void Reset()
    {
        // Do we want the boss to re-appear at every Reset ?
        // isTriggered = false;
    }
}
