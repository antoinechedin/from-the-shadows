using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToBlackScreen : MonoBehaviour
{
    public bool finishedFadingIn = false;
    public bool finished = false;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (finishedFadingIn)
        {
            animator.SetBool("finishedFadingIn", true);
            if (finished)
            {
                Destroy(gameObject);
            }
        }
    }
}
