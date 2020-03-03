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
        Invoke("TriggerAttack", 3);
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("HorizontalAttack");
    }
}
