using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public Transform[] points;
    public float timeBetweenAttacks;
    public GameObject hands;
    public GameObject player1;
    public GameObject player2;

    public int hp = 3;
    private int laneToAttack = 0;
    private string stringDirection;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("TriggerAttack", timeBetweenAttacks, timeBetweenAttacks);
    }

    public void TriggerAttack()
    {
        FindTarget();
        string trigger = "Attack" + stringDirection + laneToAttack;
        hands.GetComponent<Animator>().SetTrigger(trigger);
    }

    public void FindTarget()
    {
        float min = Mathf.Infinity;
        for (int i = 0; i < points.Length / 2; i++) {
            float minLeft = Mathf.Min(Vector3.Distance(player1.transform.position, points[i * 2].position),
                                      Vector3.Distance(player2.transform.position, points[i * 2].position));
            float minRight = Mathf.Min(Vector3.Distance(player1.transform.position, points[i * 2 + 1].position),
                                       Vector3.Distance(player2.transform.position, points[i * 2 + 1].position));
            if (minLeft < min && minLeft < minRight)
            {
                min = minLeft;
                laneToAttack = i;
                stringDirection = "Left";
            } else if (minRight < min && minRight < minLeft)
            {
                min = minRight;
                laneToAttack = i;
                stringDirection = "Right";
            }
        }
    }
    
    public void GetHurt()
    {
        Debug.Log("aie");
        transform.Find("SkeletonFBX").GetComponent<Animator>().SetTrigger("Battlecry");
        hp--;
        if (hp == 0)
            Die();
    }

    public void Die()
    {
        Debug.Log("dead");
        transform.Find("SkeletonFBX").GetComponent<Animator>().SetTrigger("Die");
        CancelInvoke();
    }
}
