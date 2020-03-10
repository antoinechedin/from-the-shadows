using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour, IResetable
{
    public Transform[] points;
    public float timeBetweenAttacks;
    public float timeBetweenDoubleAttacks;
    public GameObject hands;
    public GameObject player1;
    public GameObject player2;
    public GameObject leftZone;
    public GameObject middleZone;
    public GameObject rightZone;
    public GameObject leftZoneBis;
    public GameObject rightZoneBis;

    private int hp = 3;
    private int laneToAttack = 0;
    private string stringDirection;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("TriggerAttack", timeBetweenAttacks, timeBetweenAttacks);
    }

    public void TriggerAttack()
    {
        Debug.Log("attack simple");
        FindTarget();
        string trigger = "Attack" + stringDirection + laneToAttack;
        hands.transform.Find(stringDirection + "HandSkeleton").GetComponent<Animator>().SetTrigger(trigger);
    }

    public void TriggerDoubleAttack()
    {
        Debug.Log("attack double");
        FindDoubleTarget();
        hands.transform.Find("LeftHandSkeleton").GetComponent<Animator>().SetTrigger("AttackLeft" + laneToAttack);
        hands.transform.Find("RightHandSkeleton").GetComponent<Animator>().SetTrigger("AttackRight" + laneToAttack);
    }

    public void FindDoubleTarget()
    {
        float minL = Mathf.Infinity;
        float minR = Mathf.Infinity;
        int laneR = -1;
        int laneL = -1;
        for (int i = 0; i < points.Length / 2; i++)
        {
            float minLeft = Mathf.Min(Vector3.Distance(player1.transform.position, points[i * 2].position),
                                      Vector3.Distance(player2.transform.position, points[i * 2].position));
            float minRight = Mathf.Min(Vector3.Distance(player1.transform.position, points[i * 2 + 1].position),
                                       Vector3.Distance(player2.transform.position, points[i * 2 + 1].position));
            if (minLeft < minL)
            {
                minL = minLeft;
                laneL = i;
            }
            if (minRight < minR)
            {
                minR = minRight;
                laneR = i;
            }
        }

        if (laneR != 1)
            laneToAttack = laneR;
        else if (laneL != 1)
            laneToAttack = laneL;
        else
            laneToAttack = 1;
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
        transform.Find("SkeletonFBX").GetComponent<Animator>().SetTrigger("Battlecry");
        
        hp--;
        if (hp == 0)
        {
            Die();
            Invoke("DestroyMiddleZone", 3);
        }

        if (hp == 1)
        {
            //Cancel Trigger Attack
            CancelInvoke();
            InvokeRepeating("TriggerDoubleAttack", 5, timeBetweenDoubleAttacks);
        }

        if (hp == 2 || hp == 1)
        {
            if (stringDirection == "Left")
                Invoke("DestroyRightZone", 1);
            else if (stringDirection == "Right")
                Invoke("DestroyLeftZone", 1);
        }
    }

    public void Die()
    {
        transform.Find("SkeletonFBX").GetComponent<Animator>().SetTrigger("Die");
        CancelInvoke();
    }

    public void Reset()
    {
        hp = 3;

        leftZone.SetActive(true);
        rightZone.SetActive(true);
        leftZoneBis.SetActive(false);
        rightZoneBis.SetActive(false);
    }

    public void DestroyLeftZone()
    {
        transform.Find("SkeletonFBX").GetComponent<Animator>().SetTrigger("VerticalLeft");
        leftZone.SetActive(false);
        leftZoneBis.SetActive(true);
    }

    public void DestroyRightZone()
    {
        transform.Find("SkeletonFBX").GetComponent<Animator>().SetTrigger("VerticalRight");
        rightZone.SetActive(false);
        rightZoneBis.SetActive(true);
    }

    public void DestroyMiddleZone()
    {
        middleZone.SetActive(false);
    }
}
