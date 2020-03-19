using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimatorFunctions : MonoBehaviour
{
    public void ShakeBattlecry()
    {
        FindObjectOfType<ChapterManager>().ShakeFor(2f, 1f, 2f);
    }
}
