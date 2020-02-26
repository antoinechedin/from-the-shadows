using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBossAI : MonoBehaviour
{
    public float attackCooldown = 5f;
    public float firstAttackDelay = 3f;

    private GameObject leftHandCollider;
    private GameObject rightHandCollider;

    private Animator animator;

    void Start()
    {
        rightHandCollider = this.transform.GetChild(1).gameObject;
        leftHandCollider = this.transform.GetChild(2).gameObject;
        animator = this.GetComponent<Animator>();

        StartCoroutine(FirstAttack());
    }

    IEnumerator FirstAttack()
    {
        yield return new WaitForSeconds(firstAttackDelay);

        int random = Random.Range(1, 7);
        animator.SetInteger("AttackId", random);

        StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(attackCooldown + 4.3f);

        int random = Random.Range(1, 7);
        animator.SetInteger("AttackId", random);

        StartCoroutine(AttackLoop());
    }

    private void EnableHandCollider(int handId)
    {
        if (handId == 0)
            rightHandCollider.SetActive(true);
        else
            leftHandCollider.SetActive(true);
    }

    private void DisableHandCollider()
    {
        rightHandCollider.SetActive(false);
        leftHandCollider.SetActive(false);
    }
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.SetInteger("AttackId", 0);
        }
    }
}
