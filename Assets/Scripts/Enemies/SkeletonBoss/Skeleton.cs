using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.Find("SkeletonFBX").gameObject.GetComponent<Animator>();
        Invoke("TriggerAttackLeft", 4);
        Invoke("TriggerAttackRight", 10);
    }

    public void TriggerAttackLeft()
    {
        animator.SetTrigger("AttackLeft");
    }

    public void TriggerAttackRight()
    {
        animator.SetTrigger("AttackRight");
    }
}
