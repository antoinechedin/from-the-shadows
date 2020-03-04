using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public Animator animator;
    public int laneToAttack;
    public string stringDirection;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.Find("SkeletonFBX").gameObject.GetComponent<Animator>();
        InvokeRepeating("TriggerAttack", 4, 6);
    }

    public void TriggerAttack()
    {
        // Construct the string trigger
        string trigger = "Attack"+stringDirection+" "+laneToAttack;

        animator.SetTrigger(trigger);
    }

}
